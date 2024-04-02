using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPickUp : MonoBehaviour
{
    private DeployShield deployShield;

    // Start is called before the first frame update
    void Start()
    {
        deployShield = GameObject.FindWithTag("Player").GetComponent<DeployShield>();

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            deployShield.UnlockShield();
            Destroy(gameObject);
        }
    }
}
