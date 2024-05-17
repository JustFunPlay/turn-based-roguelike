using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSwitch : MonoBehaviour
{
    public GameObject[] characters;
    public float delay = 10;
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
            yield return new WaitForSeconds(delay);
            Debug.Log("swithc char" + i);
            characters[i].SetActive(false);

            i = (i+1) % characters.Length;
            characters[i].SetActive(true);
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
