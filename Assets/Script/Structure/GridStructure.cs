using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridStructure : MonoBehaviour
{
    [Space]
    [Header("Install Position Preview")]
    public Vector3 posPreview;

    [Space]
    [Header("Install Direction")]
    [Space]
    public bool minus; // if minus is true => (Up -> Down, Right -> Left, Forward -> Backward)
    [Space]
    [Header("(0 or Anything = Up, 1 = Right, 2 = Forward)")]
    public float installDirection;

    private void Start()
    {
        var scale = transform.parent.localScale;

        // Set install position.
        positionSetting();
    }
    private void positionSetting() // Set install position.
    {
        Vector3 dir;

        switch (installDirection) 
        {
            case 1:
                dir = transform.parent.right;
                break;

            case 2:
                dir = transform.parent.forward;
                break;

            default:
                dir = transform.parent.up;
                break;
        }
        posPreview = transform.parent.position + (dir * (transform.parent.localScale.y + 0.001f) * (minus? -1 : 1));
    }
}
