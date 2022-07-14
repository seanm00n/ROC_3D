using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Attack : MonoBehaviour
{
    public int layerNumber = 0;
    Rigidbody r;

    public GameObject Explosion;

    // Start is called before the first frame update
    void Start()
    {
        r = GetComponent<Rigidbody>();
        r.AddForce(transform.forward * 500f);
        Destroy(gameObject, 2f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == layerNumber)
        {
            GameObject Explo = Instantiate(Explosion, other.gameObject.transform.transform.position, Quaternion.identity);
            Destroy(other.gameObject);

            Destroy(Explo,2f);
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
