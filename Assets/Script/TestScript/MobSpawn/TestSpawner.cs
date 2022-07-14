using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestSpawner : MonoBehaviour
{
    public float speed = 0.1f;
    public GameObject TestPrefab;
    public GameObject AttackTarget;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
        }
            if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject newObject = Instantiate(TestPrefab, transform.position, transform.rotation);
            if (AttackTarget)
            {
                newObject.transform.LookAt(new Vector3(AttackTarget.transform.position.x, newObject.transform.position.y, AttackTarget.transform.position.z));
                newObject.GetComponentInChildren<Test_Mob_Ai>().target = AttackTarget;
            }
            newObject.GetComponent<Rigidbody>().AddForce(newObject.transform.forward * 300);
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.position += transform.forward * speed;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.position += transform.forward * -speed;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position += transform.right * -speed;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.position += transform.right * speed;
        }


    }
}
