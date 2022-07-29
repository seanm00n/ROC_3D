using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class ResolutionSetting : MonoBehaviour, ISetting
{
    [Space]
    [Header("UI")]
    public Dropdown resolutionsDropdown;
    public Toggle fullscreenBtn;

    // Below statement is Resolution Setting //
    private FullScreenMode screenMode;
    readonly List<Resolution> resolutions = new();

    private void Start()
    {
        InitUI();
    }

    private void InitUI()
    {
        resolutions.AddRange(Screen.resolutions);

        resolutionsDropdown.options.Clear();

        // Add all available resolutions
        resolutionsDropdown.AddOptions(resolutions.Select(res => res.width + "x" + res.height).ToList());

        // Update the dropdown appearance
        resolutionsDropdown.RefreshShownValue();
        fullscreenBtn.isOn = Screen.fullScreenMode.Equals(FullScreenMode.FullScreenWindow);
    }

    // Used in inspector
    public void ApplyButton(float num) // Resolution apply button
    {
        var selectedResolution = resolutions[resolutionsDropdown.value];

        Screen.SetResolution(selectedResolution.width, selectedResolution.height, screenMode);
    }

    // Used in inspector
    public void FullScreenBtn(bool isFull)
    {
        screenMode = isFull ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
    }

    // Used in inspector
    public void ExitAspectSet()
    {
        gameObject.SetActive(false);
    }
}
