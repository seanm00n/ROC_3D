using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetItem : MonoBehaviour
{
    [Header("Except certain layer when you find item")]
    public LayerMask exceptLayer;

    TestBox boxItem;
    
    void Update()
    {
        // Find item.
        RaycastHit hit;
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red);
        if (Physics.Raycast(ray, out hit, 10f, ~exceptLayer))
        {
            if (hit.collider.gameObject.layer == 9)
            if (hit.collider.gameObject.GetComponent<TestBox>() != null)
            {
                if (Input.GetKeyDown(KeyCode.F)) // Use Item.
                {
                    boxItem = hit.collider.gameObject.GetComponent<TestBox>();
                    boxItem.Use();
                }
            }
        }
    }
}
