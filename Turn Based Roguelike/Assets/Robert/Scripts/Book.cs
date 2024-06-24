using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Book : MonoBehaviour
{
    public GameObject mainButtons;
    public GameObject catalog;
    public Animator animator;
    public void StartAnim()
    {
        mainButtons.SetActive(false);
        catalog.SetActive(true);
        animator.SetInteger("Book", 1);
    }
    public void ExitBook()
    {
        animator.SetInteger("Book", 4);
    }
}
