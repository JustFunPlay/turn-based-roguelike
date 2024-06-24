using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookCredits : MonoBehaviour
{
    public GameObject mainButtons;
    public GameObject credits;
    public GameObject creditsUI;
    public Animator animator;
    public void CreditsButton()
    {
        mainButtons.SetActive(false);
        credits.SetActive(true);
        animator.SetInteger("Book", 1);
    }
    public void ExitCredits()
    {
        creditsUI.SetActive(false);
        animator.SetInteger("Book", 4);
    }
}
