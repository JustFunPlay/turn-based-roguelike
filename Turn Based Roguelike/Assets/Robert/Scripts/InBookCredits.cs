using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InBookCredits : MonoBehaviour
{
    public GameObject credits;
    public GameObject mainButtons;
    public GameObject creditsUI;
    public Animator animator;
    public void CreditsUI()
    {
        if (gameObject.activeInHierarchy)
        {
            creditsUI.SetActive(true);
        }
    }
    public void CloseCreditsUI()
    {
        if (gameObject.activeInHierarchy)
        {
            credits.SetActive(false);
            mainButtons.SetActive(true);
            animator.SetInteger("Book", 0);
        }
    }
}
