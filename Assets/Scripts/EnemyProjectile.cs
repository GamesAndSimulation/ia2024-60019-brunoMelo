using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{

    public float projectileDamage;
    private PlayerHealth playerHealthScript;

    public GameObject hitEffect;

    private void Start()
    {
        playerHealthScript = GameObject.FindWithTag("Player").GetComponent<PlayerHealth>();
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject impactGO = Instantiate(hitEffect);


        Destroy(impactGO, 2f);

        if (other.CompareTag("Player"))
        {
            playerHealthScript.TakeDamage(projectileDamage);

            Destroy(gameObject);
        }

        Destroy(gameObject);


    }
}
