using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

// Some code adapted from https://www.youtube.com/watch?v=yeaELkoxD9w&t=215s
public class ChangeResolution : MonoBehaviour
{
    TMP_Dropdown dropdown;
    public string resolutionText;
    int dropdownValue;
    public Toggle fullscreen;
    public Toggle vsync;
    private Resolution[] resolutions;

    void Start()
    {
        dropdown = GetComponent<TMP_Dropdown>();

        fullscreen.isOn = Screen.fullScreen;

        _ = (QualitySettings.vSyncCount == 1) ? vsync.isOn == true : vsync.isOn == false;

        // Get resolutions from computer and add them to the dropdown list
        resolutions = Screen.resolutions.Select(resolution => new Resolution { width = resolution.width, height = resolution.height }).Distinct().ToArray();
        for (int i = resolutions.Length - 1; i >= 0; i--)
        {
            List<string> newRes = new List<string> { $"{resolutions[i].width}x{resolutions[i].height}" };
            dropdown.AddOptions(newRes);
        }

        dropdown.SetValueWithoutNotify(0); // Set dropdown to first in list (highest native resolution)
        Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, fullscreen.isOn); // Set resolution
    }

    // Update is called once per frame
    void Update()
    {
        dropdownValue = dropdown.value; //Keep the current index of the Dropdown in a variable
        resolutionText = dropdown.options[dropdownValue].text;

        int width = int.Parse(resolutionText.Split('x')[0]), height = int.Parse(resolutionText.Split('x')[1]);
        Screen.SetResolution(width, height, fullscreen.isOn);
    }
}
