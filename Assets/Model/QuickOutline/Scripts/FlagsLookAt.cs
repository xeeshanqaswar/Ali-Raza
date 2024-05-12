using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagsLookAt : MonoBehaviour
{
    Transform camerTransform;
    private void Awake()
    {
        camerTransform = GameObject.FindGameObjectWithTag("OverlayCamera").transform;
    }
    // Update is called once per frame
    void Update()
    {
        LookatCamera();
    }


    void LookatCamera()
    {
        var relativeUp = camerTransform.TransformDirection(Vector3.up);
        var relativePos = camerTransform.position - transform.position;
        transform.rotation = Quaternion.LookRotation(relativePos, relativeUp);
    }
}
