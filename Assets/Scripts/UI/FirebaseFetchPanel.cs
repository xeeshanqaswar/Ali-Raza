using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public class FirebaseFetchPanel : MonoBehaviour
{
    public ViewDataFirebase dataPrefab;
    public TextMeshProUGUI emptyText;
    public GameObject parent;
    [SerializeField] GameObject loadingPanel;
    public  List<GameObject> dataLocalPrefabs;
    public static Action RefreshPanelAction;


    private void Awake()
    {
        RefreshPanelAction += RefreshPanel;
    }
    void OnEnable()
    {
        
        FirebaseController.GetUsers(users =>
        {
            if (users != null)
            {
                emptyText.enabled = false;
                foreach (var user in users)
                {
                    GameObject data = Instantiate(dataPrefab.gameObject, parent.transform);
                    ViewDataFirebase data1 = data.GetComponent<ViewDataFirebase>();
                    data1.SetData(user.Value.lessons.lessonNumber, user.Value.lessons.QuesitonNumber, user.Value.cameraTransformPosition, user.Value.cameraTransformRotation.eulerAngles, user.Key.ToString());
                    dataLocalPrefabs.Add(data1.gameObject);
                }
            }else
            {
                emptyText.enabled = true;
               
            }

        });


    }

    public void OnDisable()
    {

        emptyText.enabled = false;
        for (int i = 0; i < dataLocalPrefabs.Count; i++)
        {

            Destroy(dataLocalPrefabs[i]);

        }

        dataLocalPrefabs.Clear();

    }
    public void RefreshPanel()
    {
        RefrenceManager.instance.gameManager.RunCoroutine(CoroutineRefresh(1f));
    }
    IEnumerator CoroutineRefresh(float delay)
    {
        gameObject.SetActive(false);
        loadingPanel.SetActive(true);
        yield return new WaitForSecondsRealtime(delay);
        gameObject.SetActive(true);
        loadingPanel.SetActive(false);
    }
}
