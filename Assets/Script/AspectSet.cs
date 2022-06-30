using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AspectSet : MonoBehaviour
{
    public Dropdown resolutionsDropdown;
    List<Resolution> resolutions = new List<Resolution>();
    public int resolutionNum;
    public Toggle fullscreenBtn;
    int optionNum = 0;
    FullScreenMode screenMode;
    void Start()
    {
        initUI();
    }

    
    void Update()
    {
        exitSetting();
    }
    void initUI () {
        for (int i = 0; i < Screen.resolutions.Length; i++) {
            if (Screen.resolutions[i].refreshRate == 60)
                resolutions.Add(Screen.resolutions[i]);
        }
        resolutionsDropdown.options.Clear();
        foreach (Resolution item in resolutions) {
            Dropdown.OptionData option = new Dropdown.OptionData();
            option.text = item.width + "x" + item.height;
            resolutionsDropdown.options.Add(option);
            if(item.width == Screen.width && item.height == Screen.height) {
                resolutionsDropdown.value = optionNum;
            }
            optionNum++;
        }
        resolutionsDropdown.RefreshShownValue();
        fullscreenBtn.isOn = Screen.fullScreenMode.Equals(FullScreenMode.FullScreenWindow) ? true : false;
    }
    public void DropdownOptionChange (int x) {
        resolutionNum = x;
    }
    public void ApplyButton () {
        Screen.SetResolution(resolutions[resolutionNum].width, 
            resolutions[resolutionNum].height,
            screenMode);
    }
    public void FullScreenBtn (bool isFull) {
        screenMode = isFull ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
    }
    void exitSetting () {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            //Instantiate();//return to menu prefab
        }
    }
}
