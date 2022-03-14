using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChangeQuality : MonoBehaviour
{
    private TMP_Dropdown dropdown;
    private int dropdownValue;

    void Start()
    {
        dropdown = GetComponent<TMP_Dropdown>();
        dropdown.SetValueWithoutNotify(0); // Set value to first in list (Ultra)
        QualitySettings.SetQualityLevel(dropdown.options.Count);
    }

    // Update is called once per frame
    void Update()
    {
        dropdownValue = dropdown.value; //Keep the current index of the Dropdown in a variable
        QualitySettings.SetQualityLevel(dropdown.options.Count - dropdownValue); // Set quality to top level (Ultra) by default
    }
}
