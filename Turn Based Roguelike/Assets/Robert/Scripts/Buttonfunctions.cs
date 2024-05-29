using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buttonfunctions : MonoBehaviour
{
    public GameObject bestScreen;
    public GameObject buttons;
    public void BestiaryScreen()
    {
        bestScreen.SetActive(true);
        buttons.SetActive(false);

    }
    public void GoBackToButtonsBest()
    {
        bestScreen.SetActive(false);
        buttons.SetActive(true);
    }
}
