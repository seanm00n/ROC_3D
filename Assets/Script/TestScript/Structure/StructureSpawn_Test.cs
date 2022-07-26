using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StructureSpawn_Test : MonoBehaviour
{
    public GameObject area;
    public static bool isArea;

    GameObject skillWindow;
    public static bool StructureMode = false;
    public float distance = 15;

    public Vector3 changePosValue;
    public GameObject LiveParent;

    public LayerMask installLayer;
    public LayerMask areaLayer;

    GameObject target;

    public int selectNumber = 0;
    public GameObject[] Selected_Prefab;
    public GameObject[] Prefab;
    public GameObject[] PrefabHUI;

    public int TurretNum = 0;

    Color color;

    private void Awake()
    {
        if (GameObject.Find("InGame_UI_sample") && GameObject.Find("InGame_UI_sample").transform.Find("Skill_Upgrade") != null)
        {
            skillWindow = GameObject.Find("InGame_UI_sample").transform.Find("Skill_Upgrade").gameObject;
        }
    }

    public void ChangeNext()
    {
        Destroy(target);
        if (selectNumber < (Selected_Prefab.Length - 1))
            selectNumber++;

        else selectNumber = 0;
    }

    public void ChangePrevious()
    {
        Destroy(target);
        if (selectNumber > 0)
            selectNumber--;

        else selectNumber = Selected_Prefab.Length - 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            if (skillWindow)
            {
                if (skillWindow.activeSelf == false)
                {
                    FindObjectOfType<PlayerAttack>().enabled = true;

                    if (StructureMode == false)
                    {
                        if (area && areaLayer != 0) area.SetActive(true);
                        StructureMode = true;
                        FindObjectOfType<PlayerAttack>().enabled = false;
                    }
                    else
                    {
                        if (area && areaLayer != 0) area.SetActive(false);
                        StructureMode = false;

                    }
                }
            }
        }

        if (Input.GetAxisRaw("Mouse ScrollWheel") > 0)
        {
            if (skillWindow)
            {
                if (skillWindow.activeSelf == false)
                {
                    ChangeNext();
                }
            }
        }
        else if (Input.GetAxisRaw("Mouse ScrollWheel") < 0)
        {
            if (skillWindow)
            {
                if (skillWindow.activeSelf == false)
                {
                    ChangePrevious();
                }
            }
        }


        if (areaLayer != 0)
        {
            if (isArea == false)
            {
                if (target != null)
                {
                    Destroy(target);
                }
                return;
            }
        }
        if (StructureMode == true)
        {
            //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;
            //Debug.DrawRay(ray.origin, ray.direction * distance,Color.red);

            if (Physics.Raycast(ray, out hit, distance, installLayer))
            {
                Debug.DrawRay(hit.point, hit.normal, Color.blue);
                if (hit.collider.gameObject.GetComponent<GridStructure>() != null)
                    changePosValue = hit.collider.gameObject.GetComponent<GridStructure>().posPreview;
                else changePosValue = new Vector3(0, 0, 0);

                if (target == null && Selected_Prefab[selectNumber])
                {
                    target = Instantiate(Selected_Prefab[selectNumber], hit.point, Quaternion.identity);
                    color = target.GetComponentInChildren<Renderer>().material.color;
                }
                else if (target != null && target.transform.position != hit.point)
                {
                    if (changePosValue != new Vector3(0, 0, 0))
                    {
                        target.transform.position = changePosValue;
                    }
                    else
                        target.transform.position = hit.point;

                }
            }
            else if (target != null)
            {
                Destroy(target);
            }


            if (target != null && Selected_Prefab[selectNumber])
            {
                Install_Compatibility Install = target.GetComponent<Install_Compatibility>();
                if (Install == null)
                    Install = target.GetComponentInChildren<Install_Compatibility>();

                if (Install && Install.Compatibility == true)
                {
                    //////// 색 변화 ////////////
                    for (int i = 0; i < Install.remderers.Length; i++)
                    {
                        Install.remderers[i].material.color = color;
                    }

                    if (Input.GetKeyDown(KeyCode.Mouse2))
                    {

                        //////// 설치 ///////////////
                        GameObject install;
                        if (changePosValue != new Vector3(0, 0, 0))
                        {
                            install = Instantiate(Prefab[selectNumber], changePosValue, Quaternion.identity);
                        }
                        else
                        {
                            install = Instantiate(Prefab[selectNumber], hit.point, Quaternion.identity);
                        }
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
        else
        {

            if (target != null)
            {
                Destroy(target);
            }
        }
    }
}
