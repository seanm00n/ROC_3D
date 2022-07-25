using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class AspectSet : MonoBehaviour
{
    public bool menu = false;
    GameObject skillWindow;
    public Dropdown resolutionsDropdown;
    public Toggle fullscreenBtn;

    readonly List<Resolution> _resolutions = new();
    FullScreenMode _screenMode;

    public PlayerMovement pm;
    public PlayerAttack pa;
    public Camera_manager cm;

    void Start()
    {
        InitUI();

        if (GameObject.Find("InGame_UI_sample") != null && GameObject.Find("InGame_UI_sample").transform.Find("Skill_Upgrade") != null)
        {
            skillWindow = GameObject.Find("InGame_UI_sample").transform.Find("Skill_Upgrade").gameObject;
        }

        pm = FindObjectOfType<PlayerMovement>();
        pa = FindObjectOfType<PlayerAttack>();
        cm = FindObjectOfType<Camera_manager>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //Instantiate();//return to menu prefab
            gameObject.SetActive(false);
        }
    }

        void InitUI()
        {
            foreach (var resolution in Screen.resolutions)
            {
                //if (resolution.refreshRate == 60) // 항상 60hz만 서포트?
                _resolutions.Add(resolution);
            }

            resolutionsDropdown.options.Clear();

            // Add all available resolutions
            resolutionsDropdown.AddOptions(_resolutions.Select(res => res.width + "x" + res.height).ToList());

            // Update the dropdown appearance
            resolutionsDropdown.RefreshShownValue();
            fullscreenBtn.isOn = Screen.fullScreenMode.Equals(FullScreenMode.FullScreenWindow);
        }

        // Used in inspector
        public void ApplyButton()
        {
            Screen.SetResolution(_resolutions[resolutionsDropdown.value].width,
            _resolutions[resolutionsDropdown.value].height,
            _screenMode);
        }

        // Used in inspector
        public void FullScreenBtn(bool isFull)
        {
            _screenMode = isFull ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
        }

        //Used in inspector
        public void ExitAspectSet()
        {
            gameObject.SetActive(false);
        }


        private void OnEnable()
        {

            Time.timeScale = 0f;
        if (!menu)
        {
            FindObjectOfType<PlayerMovement>().enabled = false;
            FindObjectOfType<PlayerAttack>().enabled = false;
            FindObjectOfType<Camera_manager>().enabled = false;
        }
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }


        private void OnDisable()
        {
            if (skillWindow)
            {
                if (skillWindow.activeSelf == false)
                {
                if(pm)
                    pm.enabled = true;
                if (pa && StructureSpawn_Test.StructureMode == false)
                    pa.enabled = true;
                if (cm)
                    cm.enabled = true;

                    Time.timeScale = 1f;
                }
            }
            if (menu)
            {
                Time.timeScale = 1f;
            }
        }
    }