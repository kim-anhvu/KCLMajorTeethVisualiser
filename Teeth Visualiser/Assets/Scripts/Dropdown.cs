using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/**
 * Handles the multiple selection of toggles in each filter
 */
public class Dropdown : MonoBehaviour
{
    public List<Toggle> toggles = new List<Toggle>(); // list of toggles in filter
    public GameObject dropdown; // filter panel
    List<string> selected = new List<string>(); // list of active toggles

    // Start is called before the first frame update
    void Start()
    {
        dropdown.SetActive(false);
        selected.Add("All");
    }
    // Update is called in each frame update
    void Update()
    {
        HideIfClickedOutside();
    }

    /*
     * Set the dropdown to active or inactive
     */
    public void ShowDropdown()
    {
        if (!dropdown.activeInHierarchy)
        {
            dropdown.SetActive(true);
        }
        else
        {
            dropdown.SetActive(false);
        }
    }

    /*
     * If all toggle is clicked, set all other toggles to active
     */
    public void SelectAll()
    {
        if (toggles[0].isOn)
        {
            for (int i = 1; i < toggles.Count; i++)
            {
                toggles[i].isOn = true;
            }
        }

    }

    /*
     * Stops user from unselecting all toggles
     * Forces last toggle to be unselected to remain selected
     * @Param Toggle toggle
     *   toggle that is clicked
     */
    public void HandleNoneSelected(Toggle toggle)
    {
        if (!toggle.isOn)
        {
            bool unselected = true;
            for(int i = 0; i < toggles.Count; i++)
            {
                if (toggles[i].isOn)
                {
                    unselected = false;
                }
            }
            if (unselected)
            {
                toggle.isOn = true;
            }
        }
    }

    /*
     * If toggle is unselected and all toggle is selected, unselect all toggle
     * @param Toggle toggle
     *   toggle that is clicked
     */
    public void UnselectAll(Toggle toggle)
    {
        if (!toggle.isOn && toggles[0].isOn)
        {
            toggles[0].isOn = false;
        }
    }

    /*
     * Hide dropdown panel if mouse clicked outside panel
     */
    private void HideIfClickedOutside()
    {
        if (Input.GetMouseButton(0) && dropdown.activeSelf &&
            !RectTransformUtility.RectangleContainsScreenPoint(
                dropdown.GetComponent<RectTransform>(),
                Input.mousePosition))
        {
            dropdown.SetActive(false);
        }
    }

    /*
     * @return List<string> selected
     *   list of selected toggles
     */
    public List<string> GetSelected()
    {
        selected = new List<string>();
        if (toggles[0].isOn)
        {
            selected.Add("All");
        }
        else
        {
            for (int i = 1; i < toggles.Count; i++)
            {
                if (toggles[i].isOn)
                {
                    selected.Add("" + toggles[i].GetComponentInChildren<TextMeshProUGUI>().text);
                }
            }
        }
        return selected;
    }

    /*
     * Reset all toggles to be active
     */
    public void Reset()
    {
        for (int i = 0; i < toggles.Count; i++)
        {
            toggles[i].isOn = true;
        }
    }
}