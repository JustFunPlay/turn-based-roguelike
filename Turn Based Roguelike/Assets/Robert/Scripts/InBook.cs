using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InBook : MonoBehaviour
{
    public GameObject bestiaryTab;
    public GameObject menuButtons;
    public GameObject catalog;
    public GameObject leftPage1Button;
    public GameObject leftPage2Button;
    public GameObject rightPage2Button;
    public GameObject rightPage3Button;
    public Animator animator;
    public void OpenUI()
    {
        bestiaryTab.SetActive(true);
        Debug.Log("book opened");
        animator.SetInteger("Book", 0);
    }
    public void CloseBook()
    {
        menuButtons.SetActive(true);
        catalog.SetActive(false);
        animator.SetInteger("Book", 0);
        Debug.Log("book closed");
    }
    public void PageOne()
    {
        animator.SetInteger("Book", 1);
    }
    public void PageTwo()
    {
        animator.SetInteger("Book", 2);
    }
    public void PageThree()
    {
        animator.SetInteger("Book", 3);
    }
    public void CloseAllUI()
    {
        leftPage1Button.SetActive(false);
        leftPage2Button.SetActive(false);
        rightPage2Button.SetActive(false);
        rightPage3Button.SetActive(false);
        bestiaryTab.SetActive(false);
    }
    public void PageOneUI()
    {
        rightPage2Button.SetActive(true);
        rightPage3Button.SetActive(true);
        bestiaryTab.SetActive(true);
    }
    public void PageTwoUI()
    {
        leftPage1Button.SetActive(true);
        rightPage3Button.SetActive(true);
    }
    public void PageThreeUI()
    {
        leftPage1Button.SetActive(true);
        leftPage2Button.SetActive(true);
    }
}
