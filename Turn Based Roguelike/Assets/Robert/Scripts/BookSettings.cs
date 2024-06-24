using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookSettings : MonoBehaviour
{
    public GameObject mainButtons;
    public GameObject settings;
    public GameObject settingsUI;
    public Animator animator;
    public void SettingsButton()
    {
        mainButtons.SetActive(false);
        settings.SetActive(true);
        animator.SetInteger("Book", 1);
    }
    public void ExitSettings()
    {
        settingsUI.SetActive(false);
        animator.SetInteger("Book", 4);
    }
}
