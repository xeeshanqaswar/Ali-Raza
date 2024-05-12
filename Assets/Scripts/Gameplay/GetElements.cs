using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetElements : MonoBehaviour
{
    public int Id;
    public int SelectableID;
    public GameObject[] hoverObjects;
    public Material[] defaultMaterial;
    public Material[] hoverMaterial;
    public Transform pivot;
    public Vector3 originalScale;

    
    public void Awake()
    {
        originalScale = transform.localScale;

    }

}
