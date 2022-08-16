using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ROC;
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

    public int turretNum = 0;
    public int ItemAmount = 0;

    public int[] turretInstallPrice;

    // Auto Setting
    Color color;
    GameObject target;
    GameObject skillWindow;
    PlayerSaveData playerSaveData;

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
        if (target)
        {
            Player.instance.ui.monsterUI.SetActive(true);
        }
        else
        {
            Player.instance.ui.monsterUI.SetActive(false);
        }

        bool changeNotWork = false;
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
                            Player.instance.playerAttackSkill.isAttack = false;
                            structureMode = true;
                            ResetItemChange();
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

            for(int i= 0; i < 4; i++)
            {
                if (PlayerSaveData.itemList.Count > 0 && PlayerSaveData.itemList[i] != "1")
                {
                    changeNotWork = false;
                    break; 
                }
                    changeNotWork = true;
            }
            if (!changeNotWork)
            {
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
            Player.instance.playerAttackSkill.enabled = false;
            try
            {
                playerSaveData = SaveManager.Load<PlayerSaveData>("PlayerData");
            }
            catch
            {
                playerSaveData = new PlayerSaveData();
            }


            //Mouse fixed
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            int earlyItemAmount = ItemAmount;
            ItemAmount = 0;
            for (int i = 0; i < 4; i++)
            {
                if (PlayerSaveData.itemList[i] != "1")
                {
                    ItemAmount ++;
                }
            }
            if (ItemAmount > earlyItemAmount)
            {
                ResetItemChange();
            }

            if (PlayerSaveData.turretAmount == PlayerSaveData.turretAmountMax) return;
            if (changeNotWork) return;
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

                if (target == null)
                {
                    if (selectedPrefab[selectNumber])
                    {
                        target = Instantiate(selectedPrefab[selectNumber], hit.point, Quaternion.identity); // View Sample Object.
                        color = target.GetComponentInChildren<Renderer>().material.color;
                    }
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

                    if (Input.GetMouseButtonDown(0))
                    {
                        if (PlayerSaveData.gold >= turretInstallPrice[selectNumber])
                        {
                            //////// Install Object ///////////////
                            PlayerSaveData.gold -= turretInstallPrice[selectNumber];
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
                                GameObject installHUI = Instantiate(prefabHUI[turretNum], liveParent.transform);
                                t.hui = installHUI; // Need it to delete hp ui when turret is gone.
                                Slider turrets = install.GetComponentInChildren<Turret>().hpBar = installHUI.GetComponentInChildren<Slider>(); // Set hp view.
                                turrets.maxValue = t.hp;
                                turrets.value = t.hp;
                            }
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
        int selectNumberSave = selectNumber;
        if ((selectNumber + 1) < (selectedPrefab.Length) && PlayerSaveData.itemList.Count > (selectNumber + 1))
        {
            selectNumber++;
            while (PlayerSaveData.itemList[selectNumber] == "1")
            {
                if ((selectNumber + 1) < (selectedPrefab.Length) && PlayerSaveData.itemList.Count > (selectNumber + 1))
                {
                    selectNumber++;
                }
                else
                {
                    ResetItemChange();
                }
            }
        }
        else
        {
            ResetItemChange();
        }
        if (selectNumberSave != selectNumber)
            Destroy(target);
    }

    public void ChangePrevious() //Change Previous Object.
    {
        int selectNumberSave = selectNumber;
        if ((selectNumber - 1) >= 0)
        {
            selectNumber--;
            while (PlayerSaveData.itemList[selectNumber] == "1")
            {
                if ((selectNumber - 1) >= 0)
                {
                    selectNumber--;
                }
                else
                {
                    ResetItemChangeReverse();
                    break;
                }
            }
        }
        else
        {
            ResetItemChangeReverse();
        }
        if (selectNumberSave != selectNumber)
            Destroy(target);
    }

    public void ResetItemChange()
    {
        if (PlayerSaveData.itemList.Count > 0)
        {
            if (PlayerSaveData.itemList[0] == "Turret Lv1") selectNumber = 0;
            else if (PlayerSaveData.itemList[1] == "Turret Lv2") selectNumber = 1;
            else if (PlayerSaveData.itemList[2] == "Turret Lv3") selectNumber = 2;
            else if (PlayerSaveData.itemList[3] == "Turret Lv4") selectNumber = 3;
        }
    }
    public void ResetItemChangeReverse()
    {
        if (PlayerSaveData.itemList.Count > 0)
        {
            if (PlayerSaveData.itemList[3] == "Turret Lv4") selectNumber = 3;
            else if (PlayerSaveData.itemList[2] == "Turret Lv3") selectNumber = 2;
            else if (PlayerSaveData.itemList[1] == "Turret Lv2") selectNumber = 1;
            else if (PlayerSaveData.itemList[0] == "Turret Lv1") selectNumber = 0;
        }
    }
}
