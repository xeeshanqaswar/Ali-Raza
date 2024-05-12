using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

public class QuestionTester : MonoBehaviour
{
    public NeoData neoData;
    public NeoDataLocal neoDataLocal;
    public Action loadDataAction;
    Coroutine CheckInternetConnectivity;



    public void OnEnable()
    {
        loadDataAction += LoaddataCarbonCopy;
    }
    public void OnDisable()
    {
        loadDataAction -= LoaddataCarbonCopy;
    }
    void Start()
    {
 /*      Loaddata();
        // LoadVersionNumber();
        LoadVersionNumberFromLocal();*/
        // StartCoroutine(DownloadJSONAndImages());
    }

    void LoadVersionNumberFromLocal()
    {
        neoData.version.version = PlayerPrefs.GetInt(Constants.versionKey);
    }

    /// <summary>
    /// Read Data from localDatabase
    /// </summary>

    void Loaddata()
    {
        if (!PlayerPrefs.HasKey(Constants.dataLoded))
        {
            RefrenceManager.instance.uIManager.InternetScreenEnable();
            if (IsInternetConnected())
            {
                Debug.Log("Calling from LoadData()");
                StartCoroutine(LoadDataFromAPI());

                RefrenceManager.instance.uIManager.LoadingScreenEnable();
                return;
            }
        }
        else
        {
            ReadDataFromLocalDataBase();
        }
    }

    void LoaddataCarbonCopy()
    {
        RefrenceManager.instance.uIManager.InternetScreenEnable();
        Debug.Log("Calling from LoaddataCarbonCopy()");

        if (RefrenceManager.instance.uIManager.loginScreen.gameObject.activeInHierarchy)
        {
            StartCoroutine(LoadDataFromAPI());
        }
        else
        {
            StartCoroutine(LoadNewDataFromAPI(true));
        }

        RefrenceManager.instance.uIManager.LoadingScreenEnable();
        return;
    }

    async Task LoadNewData()
    {
        RefrenceManager.instance.uIManager.InternetScreenEnable();

        if (IsInternetConnected())
        {
            Debug.Log("Calling from LoadNewData()");
            StartCoroutine(LoadNewDataFromAPI(true));
            RefrenceManager.instance.uIManager.LoadingScreenEnable();
            /*  neoData.version.version = int.Parse(newVersionNumber);*/
            return;
        }

     //   ReadDataFromLocalDataBase();
    }

    IEnumerator LoadNewDataFromAPI(bool loaded = false)
    {
        Debug.Log("This is working");
        UnityWebRequest request = UnityWebRequest.Get(Constants.apiUri);

        yield return request.SendWebRequest();

        Debug.Log("This is working 1");

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("This is working 2");
            string jsonData = request.downloadHandler.text;
            if (neoData != null)
            {
                Loaddata(jsonData);
                StartCoroutine(DownloadJSONAndImages(loaded));
            }
            else
            {
                Debug.LogError("NeoData is null");
            }
        }
        else
        {
            Debug.LogError("Failed to load data from API: " + request.error);
            StartCoroutine(LoadNewDataFromAPI(true));
        }
    }

    private void ReadDataFromLocalDataBase()
    {
       // RefrenceManager.instance.uIManager.InternetScreenDisbale();
        LoaddataLocalDataBase(ReadFromLocalDataBase());

    }

    /// <summary>
    /// Load data from API 
    /// </summary>
    /// <returns></returns>
    IEnumerator LoadDataFromAPI(bool updated=false)
    {
       
        UnityWebRequest request = UnityWebRequest.Get(Constants.apiUri);

        yield return request.SendWebRequest();

       

        if (request.result == UnityWebRequest.Result.Success)
        {
            
            string jsonData = request.downloadHandler.text;
            if (neoData != null)
            {
                Loaddata(jsonData);
                StartCoroutine(DownloadJSONAndImages(updated));
            }
            else
            {
                Debug.LogError("NeoData is null");
            }
        }
        else
        {
          
            StartCoroutine(LoadDataFromAPI());
        }
    }

    IEnumerator LoadVersionFromApi()
    {
        UnityWebRequest request = UnityWebRequest.Get(Constants.apiVersionUri);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string jsonData = request.downloadHandler.text;
            if (neoData != null)
            {
                LoadVersionNumber(jsonData);
                //Loaddata(jsonData);
            }
            else
            {
                Debug.LogError("NeoData is null");
            }
        }
        else
        {
            Debug.LogError("Failed to load data from API: " + request.error);
        }
    }


    string newVersionNumber;

    IEnumerator CompareVersionNum()
    {
        Debug.Log("CompareVersionNum()");
        CheckInternetConnectivity = StartCoroutine(CheckInternetConnectivities());
        UnityWebRequest request = UnityWebRequest.Get(Constants.apiVersionUri);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            // internet = false;
            Debug.Log("CompareVersionNum()");
            string jsonData = request.downloadHandler.text;
            NeoData.Version newVersion = JsonUtility.FromJson<NeoData.Version>(jsonData);
            newVersionNumber = newVersion.version.ToString();
            RefrenceManager.instance.uIManager.LoadingScreenDisable();

            StopCoroutine(CheckInternetConnectivity);


            if (newVersion.version == neoData.version.version)
            {
                RefrenceManager.instance.uIManager.NoUpdatePanelEnable();
            }
            else
            {
                RefrenceManager.instance.uIManager.UpdatePanelScreenEnable();
            }
        }
        else
        {
            //disable the update panels if open
            RefrenceManager.instance.uIManager.UpdatePanelScreenDisable();
            RefrenceManager.instance.uIManager.NoUpdatePanelDisable();
            RefrenceManager.instance.uIManager.LoadingScreenDisable();

            if (!RefrenceManager.instance.uIManager.InternetScreen.activeInHierarchy)
            {
                Debug.Log("Opening from CompareVersionNum()");
                RefrenceManager.instance.uIManager.NoInternetPanelEnable();
            }
        }
        
    }
    
    bool internet;

    IEnumerator CheckInternetConnectivities()
    {
        Debug.Log("Running");
        while (true)
        {
            bool isConnected = IsInternetConnected();

            if (isConnected)
            {
                //Debug.Log("Connected");
            }
            else
            {
                Debug.Log("Disconnected");
                
                //close the update panels if open
                RefrenceManager.instance.uIManager.UpdatePanelScreenDisable();
                RefrenceManager.instance.uIManager.NoUpdatePanelDisable();
                RefrenceManager.instance.uIManager.LoadingScreenDisable();

                if (!RefrenceManager.instance.uIManager.InternetScreen.activeInHierarchy)
                {
                    Debug.Log("Opening from CheckInternetConnectivities()");
                    RefrenceManager.instance.uIManager.NoInternetPanelEnable();
                }

                break;

            }

            yield return new WaitForSeconds(1);
        }

    }

    private bool IsInternetConnected()
    {
       
        try
        {
            using (HttpClient client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromSeconds(2); // Set a timeout of 5 seconds
                HttpResponseMessage response = client.GetAsync("http://clients3.google.com/generate_204").Result;

                return response.IsSuccessStatusCode;
            }
        }
        catch
        {
            return false;
        }

    }

    public async void TakeUpdate()
    {
        if (IsInternetConnected())
        {
            RefrenceManager.instance.uIManager.UpdatePanelScreenDisable();
            RefrenceManager.instance.uIManager.LessonScreenDisable();
            RefrenceManager.instance.uIManager.MenuIconDisable();
            RefrenceManager.instance.uIManager.EnableLaunchBtnBlocker(true);
            await LoadNewData();
            StartCoroutine(LoadVersionFromApi());
            RefrenceManager.instance.uIManager.lessonScreen.isDataUpdated = true;
            RefrenceManager.instance.uIManager.lessonScreen.isUpdateTaken = true;
            return;
        }
        else
        {
            RefrenceManager.instance.uIManager.InternetScreenEnable();
            RefrenceManager.instance.uIManager.UpdatePanelScreenDisable();
        }
    }
    
    bool isInternetOn = false;
    
    public void CheckUpdate()
    {
        //RefrenceManager.instance.uIManager.DisableExitPopup_NoLessonActivation();
        //StartCoroutine(CheckInternetConnection());
        if (IsInternetConnected())
        {
            Debug.Log("Internet is on : ");
            StartCoroutine(CompareVersionNum());
            RefrenceManager.instance.uIManager.LoadingScreenEnable();
        }
        else
        {
            Debug.Log("Opening from CheckUpdate()");

            //disable the update panels if open
            RefrenceManager.instance.uIManager.UpdatePanelScreenDisable();
            RefrenceManager.instance.uIManager.NoUpdatePanelDisable();

            RefrenceManager.instance.uIManager.LoadingScreenDisable();

            if (!RefrenceManager.instance.uIManager.InternetScreen.activeInHierarchy)
            {
                RefrenceManager.instance.uIManager.NoInternetPanelEnable();
            }
        }
    }


    /// <summary>
    /// Write Data into LocalDataBase
    /// </summary>
    /// <param name="jsonData"></param>
    private void WriteInLocalDataBase(string jsonData)
    {
        string filePath = Path.Combine(Application.persistentDataPath, "GameData", "gameData.json");
        Directory.CreateDirectory(Path.GetDirectoryName(filePath));
        Debug.Log("Full Path: " + filePath);
        System.IO.File.WriteAllText(filePath, jsonData);
      
    }
    
    /// <summary>
    /// Read Data From Local DataBase 
    /// </summary>
    /// <returns></returns>
    private string ReadFromLocalDataBase()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "GameData", "gameData.json");
        Directory.CreateDirectory(Path.GetDirectoryName(filePath));
        string loadedData = System.IO.File.ReadAllText(filePath);
        return loadedData;
    }
    
    /// <summary>
    /// Load Data to Scriptable for Use 
    /// </summary>
    /// <param name="jsonData"></param>
    private void Loaddata(string jsonData)
    {
         var firstLesson = neoData.lessons.FirstOrDefault();
        neoData.lessons = JsonHelper.FromJson<NeoData.Lesson>(jsonData);
        neoData.lessons = new NeoData.Lesson[] { firstLesson };
        /*  neoData.lessons = neoData.lessons.OrderBy(lesson => int.Parse(lesson.name.Split('.')[0])).ToArray();*/
        Constants.totalNumberofLessons = neoData.lessons.Length;
        RefrenceManager.instance.questionManager.InitialiseResultScreenList();
        Debug.Log("Total Number of lessons are" + Constants.totalNumberofLessons);

    }

    private void LoadVersionNumber(string jsonData)
    {
        //neoData.version = JsonHelper.FromJson<NeoData.Version>(jsonData);
        neoData.version = JsonUtility.FromJson<NeoData.Version>(jsonData);
        Debug.Log("Verision number is " + neoData.version.version);
        PlayerPrefs.SetInt(Constants.versionKey, neoData.version.version);
    }

    private void LoaddataLocalDataBase(string jsonData)
    {
        // neoData.lessons = JsonHelper.FromJson<NeoData.Lesson>(jsonData);
        JsonUtility.FromJsonOverwrite(jsonData, neoData.lesson);
        /* neoData.lessons = neoData.lessons.OrderBy(lesson => int.Parse(lesson.name.Split('.')[0])).ToArray();*/
        var firstLesson = neoData.lesson.lessons[0];
        Debug.Log("heelo");
        neoData.lesson.lessons.ToList().Clear();
        neoData.lesson.lessons[0] = firstLesson;
        Constants.totalNumberofLessons = neoData.lesson.lessons.Length;
        RefrenceManager.instance.questionManager.InitialiseResultScreenList();

    }

    /// <summary>
    /// Setting PlayerPref 
    /// </summary>
    private void SetPlayerPref()
    {
        PlayerPrefs.SetString(Constants.dataLoded, "True");
    }

    /// <summary>
    /// Check Internet Connectivity 
    /// </summary>
    /// <returns></returns>
    private bool CheckConnectivity()
    {
        return Application.internetReachability == NetworkReachability.NotReachable;
    }




    public List<Dictionary<string, object>> data;

    IEnumerator DownloadJSONAndImages(bool updated = false)
    {
        // Download the JSON data
        Debug.Log("The Method is calling :");
        
        //RefrenceManager.instance.uIManager.LessonScreenDisbale();
        
        foreach (var lesson in neoData.lessons)
        {
            foreach (var question in lesson.questions)
            {
                foreach (var option in question.options)
                {
                    if (!string.IsNullOrEmpty(option.image))
                    {
                        string imageURL = option.image;

                        yield return StartCoroutine(DownloadImage(imageURL, option));

                    }
                }

            }
            break;
        }
        
        StartCoroutine(LoadVersionFromApi());
        SetPlayerPref();

        RefrenceManager.instance.progressManager.StartNewSession();       //delete the previous progress

        RefrenceManager.instance.uIManager.InternetScreenDisable();
        RefrenceManager.instance.uIManager.LoadingScreenDisable();
       /* neoData.lessons = neoData.lessons.OrderBy(lesson => int.Parse(lesson.name.Split('.')[0])).ToArray();*/
        foreach (var lesson in neoData.lessons)
        {
            lesson.name = new string(lesson.name.Where(c => !char.IsDigit(c) && c != '.').ToArray());
        }
        LessonsData lessonsData = new LessonsData
        {
            lessons = neoData.lessons
        };

        // Convert the NeoData object back to JSON
        string updatedJSON = JsonUtility.ToJson(lessonsData);
        WriteInLocalDataBase(updatedJSON);
        ReadDataFromLocalDataBase();
        
        RefrenceManager.instance.uIManager.transparentImage.SetActive(false);
        RefrenceManager.instance.uIManager.MenuIconEnable();

        if (updated)
        {
            RefrenceManager.instance.uIManager.LessonScreenEnable();
        }

        // Save the updated JSON to a file
        /*System.IO.File.WriteAllText(updatedJSONPath, updatedJSON);*/

    }

    IEnumerator DownloadImage(string imageURL, NeoData.Option option)
    {
        if(CheckConnectivity())
        {
            RefrenceManager.instance.uIManager.NoInternetPanelEnable();
            //  RefrenceManager.instance.uIManager.InternetScreenEnable();

        }
        else {
            //  RefrenceManager.instance.internetConnection.RetryConnection();

            using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(imageURL))
            {
                yield return www.SendWebRequest();

                if (www.isNetworkError || www.isHttpError)
                {
                    Debug.LogError("Image download error: " + www.error);
                }
                else
                {
                    
                    Texture2D texture = DownloadHandlerTexture.GetContent(www);
                    
                    // Extract the filename from the URL
                    string fileName = Path.GetFileName(imageURL);

                    // Replace invalid characters in the filename
                    fileName = string.Join("_", fileName.Split(Path.GetInvalidFileNameChars()));
                    
                    // Save the image locally with the sanitized filename
                    string localImagePath = Application.persistentDataPath + "/Resources"+fileName;
                    Debug.Log("local image path is" + localImagePath);
                    System.IO.File.WriteAllBytes(localImagePath, texture.EncodeToPNG());

                    // Update the option's "image" field with the local path
                    option.image = localImagePath;
                }
            }
        }
    }

    public Sprite LoadImage(string imagePath)
    {
        if (System.IO.File.Exists(imagePath))
        {
            byte[] imageBytes = System.IO.File.ReadAllBytes(imagePath);
          
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(imageBytes);  // Load the image data into the texture
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            return sprite;

        }
        else
        {
            Debug.LogWarning("Image not found: " + imagePath);
            return null;
        }
    }
    [Serializable]
    public class LessonsData
    {
        public  NeoData.Lesson[] lessons;
    }

}



