using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Attack : MonoBehaviour
{
    public float skill_Damage_Value = 0;
    
    public bool playerIsParent = false;
    public int layerNumber = 0;
    Rigidbody r;

    public GameObject Explosion;

    // Start is called before the first frame update
    void Start()
    {
        if (!playerIsParent)
        {
            r = GetComponent<Rigidbody>();
            r.AddForce(transform.forward * 500f);
            Destroy(gameObject, 2f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!playerIsParent)
        {
            if (other.gameObject.layer == layerNumber)
            {
                GameObject Explo = Instantiate(Explosion, other.gameObject.transform.transform.position, Quaternion.identity);
                
                Destroy(Explo, 2f);
                Destroy(gameObject);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
