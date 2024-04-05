using System.Collections;
using UnityEngine;

public class ShockerCollider : MonoBehaviour
{
    private PlayerHealth playerHealthScript;
    public float shockDamage = 20f;
    public float damageInterval = 1f; // Adjust this value to change the interval
    private bool canDamage = false; // Initially set to false

    public float soundMaxDistance = 30f;


    [SerializeField] private AudioClip electricitySounds;
    private bool playSound;

    // Start is called before the first frame update
    void Start()
    {
        playerHealthScript = GameObject.FindWithTag("Player").GetComponent<PlayerHealth>();
        playSound = true;
    }

    private void Update()
    {
        if(playSound)
        {
            playSound = false;

            // Calculate the distance between the player and the sound source
            float distanceToPlayer = Vector3.Distance(transform.position, GameObject.FindWithTag("Player").transform.position);

            // Calculate the volume based on the distance
            float adjustedVolume = Mathf.Clamp01(1f - (distanceToPlayer / soundMaxDistance)) * 1f;

            SoundFXManager.instance.PlaySoundFXClip(electricitySounds, transform, adjustedVolume);
            Invoke(nameof(ResetSound), electricitySounds.length);
        }
          
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

    private void ResetSound()
    {
        playSound = true;
    }
}
