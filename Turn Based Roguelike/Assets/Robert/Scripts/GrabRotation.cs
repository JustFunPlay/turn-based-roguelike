using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabRotation : MonoBehaviour
{
    float rotationSpeed = 5f;

    private void OnMouseDrag()
    {
        float xRotation = Input.GetAxis("Mouse X") * rotationSpeed;
        float yRotation = Input.GetAxis("Mouse Y") * rotationSpeed;

        transform.Rotate(Vector3.down, xRotation);
        transform.Rotate(Vector3.up, yRotation);
    }
}
