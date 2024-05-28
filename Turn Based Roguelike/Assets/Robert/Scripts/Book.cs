using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Book : MonoBehaviour
{
    public GameObject bestiaryTab;
    public GameObject mainButtons;
    public GameObject catalog;
    public Animator animator;
    public void StartAnim()
    {
        mainButtons.SetActive(false);
        catalog.SetActive(true);
        animator.SetInteger("BookPar", 1);
    }
    public void ExitBook()
    {
        bestiaryTab.SetActive(false);
        animator.SetInteger("BookPar", 2);
    }
}
