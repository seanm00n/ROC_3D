using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetItem : MonoBehaviour
{
    public LayerMask ExceptLayer;
    Test_Box boxItem;
    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red);
        if (Physics.Raycast(ray, out hit, 10f, ~ExceptLayer))
        {
            if (hit.collider.gameObject.layer == 9)
            if (hit.collider.gameObject.GetComponent<Test_Box>() != null)
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    boxItem = hit.collider.gameObject.GetComponent<Test_Box>();
                    boxItem.Use();
                }
            }
        }
    }
}
