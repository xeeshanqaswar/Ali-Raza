using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections.Generic;
public class ImageLoader : MonoBehaviour
{
    public List<string> imageUrls; // List of URLs for multiple images
    public List<Image> imageUI; // Reference to the UI Image component
    public List<string> allImagePaths;
    public string localImageFolder = "DownloadedImageQusetions"; // Folder to store downloaded images
    private int currentIndex = 0; // Index of the current image being displayed
    private string localFilePath;

    private void Start()
    {
        Debug.Log("Running Image Loader Start: ");
        localFilePath = Path.Combine(Application.persistentDataPath, localImageFolder);
        
        if (!Directory.Exists(localFilePath))
        {
            Directory.CreateDirectory(localFilePath);
        }
        DownloadNextImage();
    }

    private void DownloadNextImage()
    {
        Debug.Log("downloading next image: ");
        if (currentIndex < imageUrls.Count)
        {
            string imageUrl = imageUrls[currentIndex];
            string localFileName = Path.GetFileName(imageUrl);
            string localImagePath = Path.Combine(localFilePath, localFileName);
            Debug.Log("local file path for images: " + localImagePath);
            if (File.Exists(localImagePath))
            {
                // If the image is already downloaded, load it from local storage
                Debug.Log("File Exists already: ");
                foreach (var item in allImagePaths)
                {
                    string localImagePath2 = Path.Combine(localFilePath, item);
                    LoadLocalImage(localImagePath2);
                }
            }
            else
            {
                StartCoroutine(DownloadAndSetImage(imageUrl, localImagePath));
            }
        }
        else
        {
            Debug.Log("All images downloaded.");
        }
    }

    private IEnumerator DownloadAndSetImage(string imageUrl, string localImagePath)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(imageUrl);
        yield return www.SendWebRequest();
        Debug.Log("Running This Code in Iterations");
        if (www.result == UnityWebRequest.Result.Success)
        {
            Texture2D texture = DownloadHandlerTexture.GetContent(www);
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            imageUI[currentIndex].sprite = sprite;
            
            // Save the downloaded image to local storage
            byte[] imageBytes = texture.EncodeToPNG();
            File.WriteAllBytes(localImagePath, imageBytes);

            currentIndex++;
            DownloadNextImage();
        }
        else
        {
            Debug.LogError("Failed to download image: " + www.error);
        }
    }

    private void LoadLocalImage(string localImagePath)
    {
        byte[] imageBytes = File.ReadAllBytes(localImagePath);
        Texture2D texture = new Texture2D(2, 2); // Create a placeholder texture
        Debug.Log("Loading Local Images: ");
        if (texture.LoadImage(imageBytes))
        {
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            imageUI[currentIndex].sprite = sprite;
            currentIndex++;
        }
        else
        {
            Debug.LogError("Failed to load the local image.");
        }
    }
}
