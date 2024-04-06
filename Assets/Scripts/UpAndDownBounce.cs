using UnityEngine;

public class UpAndDownBounce : MonoBehaviour
{
    public float amplitude = 1f; // Amplitude of the bounce (peak height)
    public float speed = 1f; // Speed of the bounce

    private Vector3 startPosition; // Initial position of the object

    void Start()
    {
        startPosition = transform.position; // Store the initial position
    }

    void Update()
    {
        // Calculate the new Y position based on sine wave
        float newY = startPosition.y + Mathf.Sin(Time.time * speed) * amplitude;

        // Update the object's position
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);
    }
}
