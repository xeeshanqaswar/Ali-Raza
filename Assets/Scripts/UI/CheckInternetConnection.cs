using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Net;
using TMPro;
using System.Net.NetworkInformation;
using System.Net.Http;
using System;

public class CheckInternetConnection : MonoBehaviour
{
    public GameObject errorPopup;
    public float checkInterval = 1.0f; // Check interval in seconds

    private void OnEnable()
    {
        StartCoroutine(CheckInternetConnectivity());
    }

    bool once;
    private IEnumerator CheckInternetConnectivity()
    {
        while (true)
        {
            bool isConnected = IsInternetConnected();

            if (isConnected)
            {
                errorPopup.SetActive(false);
                
                if (!once)
                {
                    once = true;
                    Debug.Log("this izz working ");
                    RefrenceManager.instance.questionsDatabase.loadDataAction?.Invoke();
                }
            }
            else
            {
                errorPopup.SetActive(true);
                
            }

            yield return new WaitForSeconds(checkInterval);
        }
    }

    private bool IsInternetConnected()
    {
        //return Application.internetReachability == NetworkReachability.NotReachable;
        //return Application.internetReachability != NetworkReachability.NotReachable;

        try
        {
            using (HttpClient client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromSeconds(3); // Set a timeout of 5 seconds
                HttpResponseMessage response = client.GetAsync("http://clients3.google.com/generate_204").Result;

                return response.IsSuccessStatusCode;
            }
        }
        catch
        {
            return false;
        }

    }

    public void RetryConnection()
    {
        StartCoroutine(CheckInternetConnectivity());
        once = false;
    }

}
