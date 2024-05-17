using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHover : MonoBehaviour
{
    public GameObject button;
    public Texture2D selectedImage;
    public Texture2D deselectedImage;

    public void ButtonSelected()
    {
        button.GetComponent<RawImage>().texture = selectedImage;
    }
    public void ButtonDeselected()
    {
        button.GetComponent<RawImage>().texture = deselectedImage;
    }
}
