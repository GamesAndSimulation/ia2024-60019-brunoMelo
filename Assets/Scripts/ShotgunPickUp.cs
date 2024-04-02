using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunPickUp : MonoBehaviour
{
    private WeaponSwitching weaponSwitchingScript;

    // Start is called before the first frame update
    void Start()
    {
        weaponSwitchingScript = GameObject.FindWithTag("Player").GetComponentInChildren<WeaponSwitching>();

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            weaponSwitchingScript.UnlockShotgun();
            Destroy(gameObject);
        }
    }
}