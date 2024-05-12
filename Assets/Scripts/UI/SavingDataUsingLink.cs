using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;

public class SavingDataUsingLink : MonoBehaviour
{
    private string imageUrl = "https://www.google.com/url?sa=i&url=https%3A%2F%2Fwww.wired.com%2Fstory%2Fhow-to-buy-a-router%2F&psig=AOvVaw0FR-a_wg6hkRbBD22lZ8Ea&ust=1697806430498000&source=images&cd=vfe&opi=89978449&ved=0CBEQjRxqFwoTCIiekvuTgoIDFQAAAAAdAAAAABAE"; // Replace with the image URL
    private string localImagePath ; // The local path where the image will be saved

    private void Start()
    {
        localImagePath = Path.Combine(Application.persistentDataPath, "image.jpg"); // Local path for saving the image
        StartCoroutine(DownloadAndSaveImage());
    }

    private IEnumerator DownloadAndSaveImage()
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(imageUrl);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.LogError("Failed to download the image: " + www.error);
        }
        else
        {
            // Get the downloaded texture
            Texture2D texture = DownloadHandlerTexture.GetContent(www);

            // Encode the texture to bytes (e.g., PNG format)
            byte[] imageBytes = texture.EncodeToPNG();

            // Save the image to local storage
            File.WriteAllBytes(localImagePath, imageBytes);

            Debug.Log("Image downloaded and saved to: " + localImagePath);
        }
    }
}
