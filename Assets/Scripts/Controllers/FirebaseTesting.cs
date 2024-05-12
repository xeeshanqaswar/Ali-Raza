using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirebaseTesting : MonoBehaviour
{

    /// <summary>
    /// script currently not in use will be deleted
    /// </summary>
    private void Start()
    {

        var user2 = new Points(new Lessonss("1", "1"), this.gameObject.transform.position, this.gameObject.transform.rotation, 61);
        FirebaseController.PostUser(user2, "12", () =>
        {
            FirebaseController.GetUser("12", user =>
            {
               /* Debug.Log($"{user.distance} {user.cameraTransformPosition} {user.}");*/
            });
        });
    }
}
