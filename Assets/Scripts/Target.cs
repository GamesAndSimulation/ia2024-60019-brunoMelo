using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Target : MonoBehaviour
{
    public float maxHealth = 50f;
    private float currentHealth;

    public Slider healthSlider;
    public float easeLerpSpeed = 0.05f;

    private Animator animator;
    private Canvas healthCanvas; // Reference to the Canvas component


    private void Awake()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        healthCanvas = GetComponentInChildren<Canvas>();
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
        if(currentHealth <= 0f)
        {
            animator.SetTrigger("Death");
            //Die();
        }
    }

    public bool IsDead()
    {
        return currentHealth <= 0f;
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
