using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bestiary : MonoBehaviour
{
    public GameObject mainParent;
    public GameObject[] beasts;
    public int currentSelected;

    public void Start()
    {
        currentSelected = 0;
    }
    public void NextBeast()
    {
        beasts[currentSelected].gameObject.SetActive(false);
        currentSelected = (currentSelected +1) % beasts.Length;
        beasts[currentSelected].gameObject.SetActive(true);
        beasts[currentSelected].GetComponent<ObjectDetails>().NewInfo();
        Debug.Log(currentSelected);
    }
    public void PrevBeast()
    {
        beasts[currentSelected].gameObject.SetActive(false);
        currentSelected--;
        if (currentSelected < 0)
        {
            currentSelected += beasts.Length;
        }
        beasts[currentSelected].gameObject.SetActive(true);
        beasts[currentSelected].GetComponent<ObjectDetails>().NewInfo();
        Debug.Log(currentSelected);
    }
}
