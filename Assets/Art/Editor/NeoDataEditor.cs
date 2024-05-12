using MiniJSON;
using System.Collections;
using System.Collections.Generic;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using static VariationFactory;

[CustomEditor(typeof(NeoData))]
public class NeoDataEditor : Editor
{
    private string jsonData;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        NeoData neoData = (NeoData)target;

        if (GUILayout.Button("Load JSON Data"))
        {
            EditorCoroutineUtility.StartCoroutineOwnerless(LoadDataFromAPI(neoData));
        }
    }


    private System.Collections.IEnumerator LoadDataFromAPI(NeoData neoData)
    {
        UnityWebRequest request = UnityWebRequest.Get(Constants.apiUri);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string filePath = Application.persistentDataPath + "/Resources" + Constants.fileName1;
            jsonData = System.IO.File.ReadAllText(filePath); 
            Debug.Log("Received JSON data: " + jsonData);

            if (!string.IsNullOrEmpty(jsonData))
            {
                if (!string.IsNullOrEmpty(jsonData))
                {
                    try
                    {
                        // Deserialize JSON to your NeoData class
                        //neoData.lessons = JsonHelper.FromJson<NeoData.Lesson>(jsonData);

                        JsonUtility.FromJsonOverwrite(jsonData, neoData.lesson);
                        /*if (loadedData != null)
                        {
                            // Assign the loaded data to your ScriptableObject
                            Undo.RecordObject(neoData, "Load JSON Data");
                            neoData.lessons = loadedData.lessons;
                            EditorUtility.SetDirty(neoData);
                            AssetDatabase.SaveAssets();
                            AssetDatabase.Refresh();
                            Debug.Log("Data loaded successfully.");
                        }
                        else
                        {
                            Debug.LogWarning("Failed to parse JSON data.");
                        }*/
                    }
                    catch (System.Exception e)
                    {
                        Debug.LogError("Error loading JSON: " + e.Message);
                    }
                }
                else
                {
                    Debug.LogError("Received empty JSON data");
                }
            }
            else
            {
                Debug.LogError("Failed to load data. Error: " + request.error);
            }
        }

    }
}
