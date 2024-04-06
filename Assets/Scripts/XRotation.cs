using UnityEngine;

public class XRotation : MonoBehaviour
{
    public float rotationSpeed = 60f; // Rotation speed in degrees per second

    private float angle = 0f; // Current angle of rotation

    void Update()
    {
        // Update the angle based on the rotation speed and time
        angle += rotationSpeed * Time.deltaTime;

        // Calculate the rotation using Quaternion.Euler
        Quaternion targetRotation = Quaternion.Euler(angle, 0f, 0f);

        // Smoothly rotate the object towards the target rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime);
    }
}
