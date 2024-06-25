using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class TestHighlight : MonoBehaviour
{
    public Camera Camera;
    public void Start()
    {
        Camera.cullingMask = 5;
    }
    public void HighlightOnTopChar()
    {
        Camera.cullingMask = 7;
    }
}
