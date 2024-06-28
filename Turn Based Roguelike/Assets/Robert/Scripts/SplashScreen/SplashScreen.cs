using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashScreen : MonoBehaviour
{
    public Animator animator;
    public GameObject text;
    public GameObject fadeInImage;
    public void StartTextZoom()
    {
        text.SetActive(true);
        animator.SetInteger("NextAnim", 1);
    }
    public void StartFadeIn()
    {
        fadeInImage.SetActive(true);
        animator.SetInteger("NextAnim", 2);
    }
}
