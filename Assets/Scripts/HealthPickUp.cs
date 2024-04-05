using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickUp : MonoBehaviour
{
    private PlayerHealth playerHealthScript;

    public float healAmount = 100f;

    [SerializeField] private AudioClip healhPickUpSound;

    // Start is called before the first frame update
    void Start()
    {
        playerHealthScript = GameObject.FindWithTag("Player").transform.GetComponent<PlayerHealth>();

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            SoundFXManager.instance.PlaySoundFXClip(healhPickUpSound, transform, 1f);
            playerHealthScript.Heal(healAmount);
            Destroy(gameObject);
        }
    }
}
