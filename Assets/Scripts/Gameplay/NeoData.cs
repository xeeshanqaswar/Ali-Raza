
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Online Database
/// </summary>
[CreateAssetMenu(fileName = "NeoData", menuName = "Custom/Neo Data", order = 1)]
public class NeoData : ScriptableObject
{
    /// <summary>
    /// Options data 
    /// </summary>
    [System.Serializable]
    
    public class Option
    {
        public string highlightedPoint;
        public string image;
        public bool correct;
        public string option;
    }

    /// <summary>
    /// Question data 
    /// </summary>

    [System.Serializable]
    public class Question
    {
        public string side;
        public string question;
        public Option[] options;
        public string type;
        public string camra;
    }
    /// <summary>
    /// Lesson Data 
    /// </summary>

    [System.Serializable]
    public class Lesson
    {
        public string name;
        public List<Question> questions;
        public bool isCompleted;
        public bool isUnlocked;
    }

    /// <summary>
    /// Version of the app to check the update
    /// </summary>
    [System.Serializable]
    public class Version
    {
        public int version;
    }


    // Assuming that "sections" will now be an array
    public Lesson[] lessons;
    public Version version;


    /// <summary>
    /// Lesson Collection for multiple Lesson 
    /// </summary>
    [System.Serializable]
    public class LessonCollection
    {
        public Lesson[] lessons;
    }
    public LessonCollection lesson;

    /// <summary>
    /// Json handling 
    /// </summary>
    /// <param name="json"></param>
    /// <returns></returns>
    public static NeoData CreateFromJSON(string json)
    {
        // Try to parse the JSON as an array of Data
        Lesson[] dataArray = JsonHelper.FromJson<Lesson>(json);
        Version version = JsonUtility.FromJson<Version>(json);

        if (dataArray != null)
        {
            // Create a new NeoData instance
            NeoData neoData = CreateInstance<NeoData>();
            neoData.lessons = dataArray;
            neoData.version = version;
            return neoData;
        }

        // If parsing as an array fails, try parsing as a single Data object
        Lesson dataObject = JsonUtility.FromJson<Lesson>(json);
        //Version versionObject = JsonUtility.FromJson<Version>(json);

        if (dataObject != null)
        {
            NeoData neoData = CreateInstance<NeoData>();
            neoData.lessons = new Lesson[] { dataObject };
            //neoData.version = new Version[] { versionObject };
            return neoData;
        }

        // If both attempts fail, log an error and return null
        Debug.LogError("Failed to parse JSON data");
        return null;
    }

    public List<Vector3> highlightedObjects = new List<Vector3>();

}


// Helper class for parsing JSON arrays
public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>("{\"items\":" + json + "}");
        return wrapper.items;
    }

    [System.Serializable]
    private class Wrapper<T>
    {
        public T[] items;
    }
}
