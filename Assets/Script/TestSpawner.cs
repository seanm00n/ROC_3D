using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSpawner : MonoBehaviour
{
    public GameObject TestPrefab;
    public GameObject AttackTarget;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject newObject = Instantiate(TestPrefab, transform.position, transform.rotation);
            newObject.transform.LookAt(new Vector3(AttackTarget.transform.position.x, newObject.transform.position.y, AttackTarget.transform.position.z));
            newObject.GetComponent<Rigidbody>().AddForce(newObject.transform.forward * 300);
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.position += transform.forward * 0.1f;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.position += transform.forward * -0.1f;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position += transform.right * -0.1f;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.position += transform.right * 0.1f;
        }


    }
}
