using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InBookSettings : MonoBehaviour
{
    public GameObject settings;
    public GameObject mainButtons;
    public GameObject settingsUI;
    public Animator animator;
    public void SettingsUI()
    {
        if (gameObject.activeInHierarchy)
        {
            settingsUI.SetActive(true);
        }
    }
    public void CloseSettingsUI()
    {
        if (gameObject.activeInHierarchy)
        {
            settings.SetActive(false);
            mainButtons.SetActive(true);
            animator.SetInteger("Book", 0);
        }
    }
}
