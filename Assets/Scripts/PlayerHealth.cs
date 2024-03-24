using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;

    public Transform player;

    public Vector3 respawnPoint;
    public Quaternion respawnRotation;

    public float raycastDistanceFall = 5f;

    public Slider healthSlider;

    public float easeLerpSpeed = 0.01f;
    public float respawnYoffset = 3f;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        
        if (healthSlider.value != currentHealth)
        {
            healthSlider.value = Mathf.Lerp(healthSlider.value, currentHealth, easeLerpSpeed);
        } 

        // Perform a raycast downwards from the player's position
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, raycastDistanceFall))
        {
            if (hit.collider.CompareTag("Checkpoint"))
            {
                // Get the center position of the collider
                Vector3 centerPosition = hit.collider.bounds.center;

                // Calculate the respawn position above the center of the collider
                Vector3 respawnPosition = centerPosition + Vector3.up * respawnYoffset;

                // Get the rotation of the collider
                Quaternion respawnRotation = hit.collider.transform.rotation;

                // Call the updateRespawn function with the respawn position
                updateRespawn(respawnPosition, respawnRotation);
            }

            // Check if the object hit by the raycast has the tag "Fall"
            if (hit.collider.CompareTag("Fall"))
            {
                // Handle the collision with the "Fall" object
                Debug.Log("Player hit a fall object!");
                // You can call any necessary methods here or perform other actions

                TakeDamage(maxHealth);
            }
        }
    }



    public void TakeDamage(float damage)
    {
        if (damage > currentHealth) currentHealth = 0;
        else currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Debug.Log("Dead");

            StartCoroutine(RespawnAfterDelay(2f));
        }
    }

    IEnumerator RespawnAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        Debug.Log("Respawning...");

        // Respawn the player after the delay
        player.position = respawnPoint;
        player.rotation = respawnRotation;
        currentHealth = maxHealth;

        Debug.Log("Player respawned.");
    }

    public void updateRespawn(Vector3 newRespawn, Quaternion respawnRotation)
    {
        this.respawnPoint = newRespawn;
        this.respawnRotation = respawnRotation;
        Debug.Log("Update Respawn");
    }
}
