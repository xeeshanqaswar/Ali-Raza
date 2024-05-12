using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableScreen : MonoBehaviour
{
    public GameObject Screen;

    private void OnEnable()
    {
        Constants.NewDisable += DisableScreenFrom;
    }
    private void OnDisable()
    {
        Constants.NewDisable -= DisableScreenFrom;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void DisableScreenFrom()
    {
        Screen.SetActive(false);


    }
}
