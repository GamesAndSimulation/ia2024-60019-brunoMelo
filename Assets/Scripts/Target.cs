using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Target : MonoBehaviour
{
    public float maxHealth = 50f;
    private float currentHealth;

    public Slider healthSlider;
    public float easeLerpSpeed = 0.05f;

    private Animator animator;
    private Canvas healthCanvas; // Reference to the Canvas component

    private Rigidbody rb;
    private NavMeshAgent agent;

    [SerializeField] private AudioClip[] damageSoundClips;
    [SerializeField] private AudioClip deathSoundClip;


    private void Awake()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        healthCanvas = GetComponentInChildren<Canvas>();
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (healthSlider.value != currentHealth)
        {
            healthSlider.value = Mathf.Lerp(healthSlider.value, currentHealth, easeLerpSpeed);
        }

        if (IsDead())
        {
            healthCanvas.gameObject.SetActive(false);
            return;
        }
    }


    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

       

        if(IsDead())
        {
            SoundFXManager.instance.PlaySoundFXClip(deathSoundClip, transform, 1f * 3);
            Die();
        }
        else
        {
            SoundFXManager.instance.PlayRandomSoundFXClip(damageSoundClips, transform, 1f);
        }
    }

    public bool IsDead()
    {
        return currentHealth <= 0f;
    }

    private void Die()
    {
        animator.SetBool("IsDead", true);
        rb.useGravity = true;
        if(agent != null)
        {
            agent.enabled = false;
        }
    }
}
