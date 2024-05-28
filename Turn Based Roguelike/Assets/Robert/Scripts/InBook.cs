using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InBook : MonoBehaviour
{
    public bool inBook;
    public GameObject bestiaryTab;
    public GameObject menuButtons;
    public GameObject catalog;
    public Animator animator;
    public void OpenUI()
    {
        if(!inBook)
        {
            bestiaryTab.SetActive(true);
            Debug.Log("book opened");
            animator.SetInteger("BookPar", 0);
            inBook = true;
        }
        else
        {
            menuButtons.SetActive(true);
            catalog.SetActive(false);
            animator.SetInteger("BookPar", 0);
            Debug.Log("book closed");
            inBook = false;
        }
    }
}
