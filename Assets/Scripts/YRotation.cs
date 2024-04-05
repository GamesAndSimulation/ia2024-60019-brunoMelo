using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YRotation : MonoBehaviour
{
    public float rotationSpeed = 60f; // Rotation speed in degrees per second

    private float angle = 0f; // Current angle of rotation

    void Update()
    {
        // Update the angle based on the rotation speed and time
        angle += rotationSpeed * Time.deltaTime;

        // Calculate the rotation using Quaternion.Euler
        Quaternion targetRotation = Quaternion.Euler(0f, angle, 0f);

        // Smoothly rotate the object towards the target rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Enemy"))
        {
            other.transform.SetParent(transform);
            Debug.Log("Parent");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        other.transform.SetParent(null);
        Debug.Log("Unparent");
    }
}
