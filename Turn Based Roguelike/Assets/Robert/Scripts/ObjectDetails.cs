using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ObjectDetails : MonoBehaviour
{
    public string objectName;
    public string objectDescription;
    public TMP_Text objectNameText;

    public void NewInfo()
    {
        objectNameText.text = objectName;
    }
}
