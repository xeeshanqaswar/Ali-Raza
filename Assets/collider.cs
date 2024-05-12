using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collider : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject popup;

    // Function called when the player collides with the object
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(" THIS IS CALLING FROM HERE ");
        // Check if the colliding object is tagged as "Player"
        if (other.CompareTag("Player"))
        {
            Debug.Log(" THIS IS CALLING FROM HERE ");
            // Open the popup if it is not already active
            if (!popup.activeSelf)
            {
                popup.SetActive(true);
                // You can add additional actions here, such as pausing the game or disabling player movement
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        Debug.Log(" THIS IS CALLING FROM HERE ");
        // Check if the colliding object is tagged as "Player"
        if (other.CompareTag("Player"))
        {
            Debug.Log(" THIS IS CALLING FROM HERE ");
            // Open the popup if it is not already active
            if (popup.activeSelf)
            {
              /*  popup.SetActive(false);*/
                // You can add additional actions here, such as pausing the game or disabling player movement
            }
        }
    }

    // Function to close the popup
    public void ClosePopup()
    {
        // Close the popup
        popup.SetActive(false);
        // You can add additional actions here, such as resuming the game or enabling player movement
    }
}
