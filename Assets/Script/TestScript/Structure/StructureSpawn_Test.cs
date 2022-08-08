using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StructureSpawn_Test : MonoBehaviour
{
    [Header("Area Setting")]
    public GameObject area;
    public static bool isArea;
    public static bool structureMode = false;
    
    [Space]
    [Header("Install possible distance")]
    public float distance = 15;

    [Space]
    [Header("Install Object Position")]
    public Vector3 changePosValue;

    [Space]
    [Header("Turret Hp Object location")]
    public GameObject liveParent;

    [Space]
    [Header("Install Possible Layer")]
    public LayerMask installLayer;

    [Space]
    [Header("Install Area Layer")]
    public LayerMask areaLayer;

    [Space]
    [Header("Using Object")]
    public int selectNumber = 0;
    public GameObject[] selectedPrefab;
    public GameObject[] prefab;
    public GameObject[] prefabHUI;

    public int TurretNum = 0;

    // Auto Setting
    Color color;
    GameObject target;
    GameObject skillWindow;

    private void Awake()
    {
        if (GameObject.Find("InGame_UI_sample") && GameObject.Find("InGame_UI_sample").transform.Find("Skill_Upgrade") != null)
        {
            skillWindow = GameObject.Find("InGame_UI_sample").transform.Find("Skill_Upgrade").gameObject;
        }
    }

    private void Start()
    {
        liveParent = Player.instance.ui.turretHp; //Turret Hp go into this location.
    }

    void Update()
    {
        if (Time.timeScale != 0)
        {
            if (Input.GetKeyDown(KeyCode.B)) // On/Off structure mode.
            {
                if (skillWindow)
                {
                    if (skillWindow.activeSelf == false)
                    {
                        Player.instance.playerAttackSkill.enabled = true;

                        if (structureMode == false)
                        {
                            if (area && areaLayer != 0) area.SetActive(true); // Visible area.
                            structureMode = true;
                            Player.instance.playerAttackSkill.enabled = false;
                        }
                        else
                        {
                            if (area && areaLayer != 0) area.SetActive(false); // Unvisible area.
                            structureMode = false;

                        }
                    }
                }
            }
            // Change Object////////////////////////

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
        }
        //// Area is exist ///////////
        
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
        //////////////////////////////
        
        if (structureMode == true)
        {
            //Mouse fixed
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            // Check Install is possible.

            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            // Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); Other Way.

            RaycastHit hit;

            // Debug.DrawRay(ray.origin, ray.direction * distance,Color.red); If you want see...

            if (Physics.Raycast(ray, out hit, distance, installLayer))
            {
                Debug.DrawRay(hit.point, hit.normal, Color.blue);

                // install fixed position.
                GridStructure grid;
                if ((grid = hit.collider.gameObject.GetComponent<GridStructure>()) != null) 
                {
                    changePosValue = grid.posPreview;
                } 

                else changePosValue = new(0, 0, 0);

                if (target == null && selectedPrefab[selectNumber])
                {
                    target = Instantiate(selectedPrefab[selectNumber], hit.point, Quaternion.identity); // View Sample Object.
                    color = target.GetComponentInChildren<Renderer>().material.color;
                }
                else if (target != null && target.transform.position != hit.point)
                {
                    if (changePosValue != new Vector3(0, 0, 0))
                    {
                        target.transform.position = changePosValue;
                        target.transform.rotation = grid.transform.rotation;
                    }
                    else
                    {
                        target.transform.position = hit.point;
                        target.transform.up = hit.normal; // Set rotation according to ground slope.
                    }
                }
            }
            else if (target != null)
            {
                Destroy(target);
            }


            if (target != null && selectedPrefab[selectNumber])
            {
                Install_Compatibility Install = target.GetComponent<Install_Compatibility>();
                if (Install == null)
                    Install = target.GetComponentInChildren<Install_Compatibility>();

                if (Install && Install.compatibility == true)
                {
                    //////// Change Color ////////////
                    for (int i = 0; i < Install.renderers.Length; i++)
                    {
                        Install.renderers[i].material.color = color;
                    }

                    if (Input.GetKeyDown(KeyCode.Mouse2))
                    {

                        //////// Install Object ///////////////
                        GameObject install;
                        if (changePosValue != new Vector3(0, 0, 0))
                        {
                            install = Instantiate(prefab[selectNumber], changePosValue, target.transform.rotation); // Set rotation according to preview.
                        }
                        else
                        {
                            install = Instantiate(prefab[selectNumber], hit.point, Quaternion.identity);

                            if (install)
                                install.transform.up = hit.normal; // Set rotation according to ground slope.
                        }

                        Turret t = install.GetComponentInChildren<Turret>();
                        if (t != null) // If player spawn turret.
                        {
                            GameObject installHUI = Instantiate(prefabHUI[TurretNum], liveParent.transform);
                            t.hui = installHUI; // Need it to delete hp ui when turret is gone.
                            Slider turrets = install.GetComponentInChildren<Turret>().hpBar = installHUI.GetComponentInChildren<Slider>(); // Set hp view.
                            turrets.maxValue = t.hp;
                            turrets.value = t.hp;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < Install.renderers.Length; i++)
                    {
                        Install.renderers[i].material.color = Color.red; // Change color when collision with obstacle.
                    }
                }
            }
        }
        else // Not Structure Mode
        {

            if (target != null)
            {
                Destroy(target);
            }
        }
    }


    public void ChangeNext() //Change Next Object.
    {
        Destroy(target);
        if (selectNumber < (selectedPrefab.Length - 1))
            selectNumber++;

        else selectNumber = 0;
    }

    public void ChangePrevious() //Change Previous Object.
    {
        Destroy(target);
        if (selectNumber > 0)
            selectNumber--;

        else selectNumber = selectedPrefab.Length - 1;
    }
}
