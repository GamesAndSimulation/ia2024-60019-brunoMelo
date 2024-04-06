using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleXRotation : MonoBehaviour
{
    public float rotationSpeed = 45f; // Rotation speed in degrees per second

    void Update()
    {
        // Rotate the object around the X-axis
        transform.Rotate(Vector3.right * rotationSpeed * Time.deltaTime);
    }
}
