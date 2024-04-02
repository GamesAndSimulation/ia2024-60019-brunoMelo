using System.Collections;
using UnityEngine;

public class ShockerCollider : MonoBehaviour
{
    private PlayerHealth playerHealthScript;
    public float shockDamage = 20f;
    public float damageInterval = 1f; // Adjust this value to change the interval
    private bool canDamage = false; // Initially set to false

    // Start is called before the first frame update
    void Start()
    {
        playerHealthScript = GameObject.FindWithTag("Player").GetComponent<PlayerHealth>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canDamage = true; // Allow continuous damage
            StartCoroutine(ApplyDamageRepeatedly()); // Start the coroutine for continuous damage
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canDamage = false; // Stop continuous damage when exiting
        }
    }

    private void ApplyDamage()
    {
        if (canDamage)
        {
            playerHealthScript.TakeDamage(shockDamage); // Apply damage at intervals
        }
    }

    private IEnumerator ApplyDamageRepeatedly()
    {
        while (canDamage)
        {
            ApplyDamage(); // Apply damage at intervals
            yield return new WaitForSeconds(damageInterval); // Wait for the specified interval
        }
    }
}
