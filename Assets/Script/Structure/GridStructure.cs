using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridStructure : MonoBehaviour
{
    public bool Minus = false;

    public bool installX = false;
    public bool installY = false;
    public bool installZ = false;

    public Vector3 posPreview;

    // Start is called before the first frame update
    void Start()
    {
        if (Minus)
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

    // Update is called once per frame
    void Update()
    {
         
    }
}
