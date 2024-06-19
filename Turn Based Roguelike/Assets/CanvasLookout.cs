using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasLookout : MonoBehaviour
{
    private Camera mainCamera;
    public void Start()
    {
        mainCamera = Camera.main;
    }
    void Update()
    {
        transform.LookAt(mainCamera.transform.position);
    }
}
