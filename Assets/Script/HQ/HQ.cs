using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HQ : MonoBehaviour, IBattle
{
    public GameObject particle;
    public GameObject dieCamera;

    public int m_health = 100;
    private bool die = false;

    public void Hit (int damage) {
        if(Player.instance.ui.gameObject.activeSelf)
        m_health -= damage;
    }

    private void Update()
    {
        if(m_health <= 0 && !die && Player.instance.ui.gameObject.activeSelf)
        {
            die = true;
            dieCamera.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Camera.main.gameObject.SetActive(false);
            Player.instance.ui.gameObject.SetActive(false);
            Player.instance.gameObject.SetActive(false);
            GameObject hqParticle = Instantiate(particle, transform.position, transform.rotation);
            while (0 < hqParticle.transform.childCount)
            {
                hqParticle.transform.GetChild(hqParticle.transform.childCount - 1).parent = null;
            }
            Destroy(hqParticle);
            Destroy(GetComponent<Renderer>());
            Destroy(GetComponent<Collider>());
        }
    }
}
