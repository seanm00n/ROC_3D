using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StructureSpawn_Test : MonoBehaviour
{
    public GameObject LiveParent;

    public LayerMask TestLayer;
    GameObject target;

    public int selectNumber = 0;
    public GameObject[] Selected_Prefab;
    public GameObject[] Prefab;
    public GameObject[] PrefabHUI;

    public int TurretNum = 0;

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
                    GameObject install = Instantiate(Prefab[selectNumber], hit.point, Quaternion.identity);
                    Turret t = install.GetComponentInChildren<Turret>();
                    if (t != null)
                    {
                        GameObject installHUI = Instantiate(PrefabHUI[TurretNum], LiveParent.transform);
                        t.Hui = installHUI;
                        Slider Turret_s = install.GetComponentInChildren<Turret>().Hpbar = installHUI.GetComponentInChildren<Slider>();
                        Turret_s.maxValue = t.HP;
                        Turret_s.value = t.HP;
                    }
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
