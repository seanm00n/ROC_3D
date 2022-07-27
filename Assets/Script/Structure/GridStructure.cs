using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridStructure : MonoBehaviour
{
    [Header("Install Position Setting")]
    public bool minus = false; // Minus => Down, Left
    public bool installX = false;
    public bool installY = false;
    public bool installZ = false;

    [Space]
    [Header("Install Position Preview")]
    public Vector3 posPreview;

    void Start()
    {
        // Set install position.
        if (minus)
        {
            if (installX == true)
                posPreview = transform.position - new Vector3(transform.localScale.x * 2 + 0.005f, 0,0);

            if (installY == true)
                posPreview = transform.position - new Vector3(0, transform.localScale.y + 0.001f, 0);

            if (installZ == true)
                posPreview = transform.position - new Vector3(0, 0, transform.localScale.z * 2 + 0.05f);
        }
        else
        {
            if (installX == true)
                posPreview = transform.position + new Vector3(transform.localScale.x * 2 + 0.005f, 0, 0);

            if (installY == true)
                posPreview = transform.position + new Vector3(0, transform.localScale.y + 0.001f, 0);

            if (installZ == true)
                posPreview = transform.position + new Vector3(0, 0, transform.localScale.z * 2 + 0.05f);
        }
        
    }
}
