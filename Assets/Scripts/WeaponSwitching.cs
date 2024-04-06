using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitching : MonoBehaviour
{
    public int selectedWeapon = 0;
    public bool sniperUnlocked = false;
    public bool rifleUnlocked = false;
    public bool shotgunUnlocked = false;

    // Start is called before the first frame update
    void Start()
    {
        selectedWeapon = 0;
        SelectWeapon();
    }

    // Update is called once per frame
    void Update()
    {
        int previousSelectedWeapon = selectedWeapon;

        if(Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            if(selectedWeapon >= transform.childCount - 1 )
            {
                selectedWeapon = 0;
            }
            else
            {
                selectedWeapon++;
            }
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if (selectedWeapon <= 0)
            {
                selectedWeapon = transform.childCount - 1;
            }
            else
            {
                selectedWeapon--;
            }
        }

        
        if(Input.GetKeyDown(KeyCode.Z)) 
        {
            selectedWeapon = 0;
        }
        if (Input.GetKeyDown(KeyCode.X) && transform.childCount >= 2 && sniperUnlocked)
        {
            selectedWeapon = 1;
        }
        if (Input.GetKeyDown(KeyCode.C) && transform.childCount >= 3 && rifleUnlocked)
        {
            selectedWeapon = 2;
        }
        if (Input.GetKeyDown(KeyCode.V) && transform.childCount >= 4 && shotgunUnlocked)
        {
            selectedWeapon = 3;
        }
        

        if (previousSelectedWeapon != selectedWeapon)
        {
            if(selectedWeapon == 1 && !sniperUnlocked)
            {
                selectedWeapon = previousSelectedWeapon;
            }

            if (selectedWeapon == 2 && !rifleUnlocked)
            {
                selectedWeapon = previousSelectedWeapon;
            }

            if (selectedWeapon == 3 && !shotgunUnlocked)
            {
                selectedWeapon = previousSelectedWeapon;
            }

            SelectWeapon();
        }
    }

    void SelectWeapon()
    {
        int i = 0;
        foreach(Transform weapon in transform)
        {
            if(i == selectedWeapon)
            {
                weapon.gameObject.SetActive(true);
            }
            else
            {
                weapon.gameObject.SetActive(false);
            }
            i++;
        }
    }

    public void UnlockSniper()
    {
        sniperUnlocked = true;
    }

    public void UnlockRiffle()
    {
        rifleUnlocked = true;
    }

    public void UnlockShotgun()
    {
        shotgunUnlocked = true;
    }
}
