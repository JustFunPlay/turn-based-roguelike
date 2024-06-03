using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSwitch : MonoBehaviour
{
    public GameObject[] characters;
    public float fadeDelay = 0.5f;
    public float animDelay = 10.5f;
    public Animator fadeInOut;
    void Start()
    {
        StartCoroutine(CharacterSwitcherTimer());
    }
    public IEnumerator CharacterSwitcherTimer()
    {
        int i = 0;
            Debug.Log("running charrotate");
        while (true)
        {
            yield return new WaitForSeconds(animDelay);
            fadeInOut.SetInteger("DoAnim", 1);
            Debug.Log("fading");
            yield return new WaitForSeconds(fadeDelay);
            Debug.Log("swithc char" + i);
            characters[i].SetActive(false);

            i = (i+1) % characters.Length;
            characters[i].SetActive(true);

            fadeInOut.SetInteger("DoAnim", 2);
        }
    }
    //public void CharacterSwitcher()
    //{
    //    for (int i=0; i < character.Length; i++)
    //    {
    //        Debug.Log(character[i].ToString());
    //    }
    //}
}
