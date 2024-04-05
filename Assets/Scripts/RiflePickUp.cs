using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiflePickUp : MonoBehaviour
{
    private WeaponSwitching weaponSwitchingScript;

    [SerializeField] private AudioClip weaponUnlockSound;

    // Start is called before the first frame update
    void Start()
    {
        weaponSwitchingScript = GameObject.FindWithTag("Player").GetComponentInChildren<WeaponSwitching>();

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SoundFXManager.instance.PlaySoundFXClip(weaponUnlockSound, transform, 1f);
            weaponSwitchingScript.UnlockRiffle();
            Destroy(gameObject);
        }
    }
}
