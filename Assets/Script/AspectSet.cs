using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class AspectSet : MonoBehaviour
{
    public Dropdown resolutionsDropdown;
    public Toggle fullscreenBtn;

    readonly List<Resolution> _resolutions = new();
    FullScreenMode _screenMode;
    
    void Start()
    {
        InitUI();
    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            //Instantiate();//return to menu prefab
            Destroy(gameObject);
        }
    }
    
    void InitUI () {
        foreach (var resolution in Screen.resolutions)
        {
            if (resolution.refreshRate == 60) // 항상 60hz만 서포트?
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
    public void ApplyButton () {
        Screen.SetResolution(_resolutions[resolutionsDropdown.value].width, 
            _resolutions[resolutionsDropdown.value].height,
            _screenMode);
    }
    
    // Used in inspector
    public void FullScreenBtn (bool isFull) {
        _screenMode = isFull ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
    }
}
