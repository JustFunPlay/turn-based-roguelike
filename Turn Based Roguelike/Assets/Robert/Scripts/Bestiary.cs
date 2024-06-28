using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Bestiary : MonoBehaviour
{
    public GameObject mainParent;
    public GameObject[] beasts;
    public string[] objectName;
    public string[] descriptionText;
    public TMP_Text objectText;
    public TMP_Text description;

    public int currentSelected;

    public void Start()
    {
        currentSelected = 0;
    }
    public void NextBeast()
    {
        beasts[currentSelected].gameObject.SetActive(false);
        currentSelected = (currentSelected +1) % beasts.Length;
        objectText.text = objectName[currentSelected];
        description.text = descriptionText[currentSelected];
        beasts[currentSelected].gameObject.SetActive(true);
        beasts[currentSelected].GetComponent<ObjectDetails>().NewInfo();
        Debug.Log(currentSelected);
    }
    public void PrevBeast()
    {
        beasts[currentSelected].gameObject.SetActive(false);
        currentSelected--;
        objectText.text = objectName[currentSelected];
        description.text = descriptionText[currentSelected];
        if (currentSelected < 0)
        {
            currentSelected += beasts.Length;
        }
        beasts[currentSelected].gameObject.SetActive(true);
        beasts[currentSelected].GetComponent<ObjectDetails>().NewInfo();
        Debug.Log(currentSelected);
    }
}
