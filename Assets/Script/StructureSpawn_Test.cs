using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureSpawn_Test : MonoBehaviour
{
    public LayerMask TestLayer;
    GameObject target;

    public int selectNumber = 0;
    public GameObject[] Selected_Prefab;
    public GameObject[] Prefab;

    Color color;
    // Start is called before the first frame update
    public void ChangeNext()
    {
        Destroy(target);
        if (selectNumber < (Selected_Prefab.Length - 1))
            selectNumber++;

        else selectNumber = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Debug.DrawRay(ray.origin, ray.direction * 20f,Color.red);
        
        if(Physics.Raycast(ray,out hit,20f,TestLayer))
        {
            if (target == null && Selected_Prefab[selectNumber])
            {
                target = Instantiate(Selected_Prefab[selectNumber], hit.point, Quaternion.identity);
                color = target.GetComponentInChildren<Renderer>().material.color;
            }
            else if (target != null && target.transform.position != hit.point)
                    target.transform.position = hit.point;
        }
        else if(target != null)
        {
            Destroy(target);
        }

        
        if (target != null && Selected_Prefab[selectNumber])
        {
            Install_Compatibility Install = target.GetComponent<Install_Compatibility>();
            if(Install == null)
                Install = target.GetComponentInChildren<Install_Compatibility>();
            if (Install &&Install.Compatibility == true)
            {
                //////// 색 변화 ////////////
                for (int i = 0; i < Install.remderers.Length; i++) 
                { 
                    Install.remderers[i].material.color = color; 
                }

                if (Input.GetKeyDown(KeyCode.Mouse2))
                {
                
                    //////// 설치 ///////////////
                    Instantiate(Prefab[selectNumber], hit.point, Quaternion.identity);
                }
            }
            else
            {
                for (int i = 0; i < Install.remderers.Length; i++)
                {
                    Install.remderers[i].material.color = Color.red;
                }
            }
        }
    }
}
