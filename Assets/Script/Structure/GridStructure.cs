using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridStructure : MonoBehaviour
{
    [Header("Install Position Setting")]
    public bool minus; // Minus => Down, Left
    public bool installX;
    public bool installY;
    public bool installZ;

    [Space]
    [Header("Install Position Preview")]
    public Vector3 posPreview;

    private void Start()
    {
        var localScale = transform.localScale;
        
        // Set install position.
        if (installX)
            posPreview = transform.position + new Vector3(localScale.x * 2 + 0.005f, 0, 0) * (minus ? -1 : 1);

        if (installY)
            posPreview = transform.position + new Vector3(0, localScale.y + 0.001f, 0) * (minus ? -1 : 1);

        if (installZ)
            posPreview = transform.position + new Vector3(0, 0, localScale.z * 2 + 0.05f) * (minus ? -1 : 1);
    }
}
