using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shooting : MonoBehaviour
{

    [Header("References")]
    public Transform cam;
    public Transform attackPoint;
    public GameObject objectToShoot;

    [Header("Settings")]
    public int totalAmmo;
    public float shootCooldown;

    [Header("Shooting")]
    public KeyCode shootKey = KeyCode.Mouse0;
    public float shootForce;
    public float shootUpwardForce;

    private bool readyToShoot;

    public Text ammoCount;

    public bool isAiming;
    public CinemachineCameraOffset cinemachineCameraOffset;

    [SerializeField] private AudioClip shootSound;

    // Start is called before the first frame update
    void Start()
    {
        readyToShoot = true;
        cam = GameObject.FindWithTag("MainCamera").transform;
        attackPoint = GameObject.Find("Attack Point").transform;
        ammoCount = GameObject.Find("Ammo Count").GetComponent<Text>();
        cinemachineCameraOffset = GameObject.Find("FreeLook Camera").GetComponent<CinemachineCameraOffset>();
        isAiming = false;
    }

    private void Shoot()
    {
        readyToShoot = false;

        GameObject projectile = Instantiate(objectToShoot, attackPoint.position, cam.rotation);

        Rigidbody projectileRB = projectile.GetComponent<Rigidbody>();

        Vector3 forceDirection = cam.transform.forward;

        
        RaycastHit hit;
        if(Physics.Raycast(cam.position, cam.forward, out hit, 500f))
        {
            forceDirection = (hit.point - attackPoint.position).normalized;
        }
        

        Vector3 forceToAdd = cam.transform.forward * shootForce + transform.up * shootUpwardForce;

        projectileRB.AddForce(forceToAdd, ForceMode.Impulse);

        SoundFXManager.instance.PlaySoundFXClip(shootSound, transform, 1f);

        totalAmmo--;

        Invoke(nameof(ResetShoot), shootCooldown);

        Destroy(projectile, 20f);
    }

    // Update is called once per frame
    void Update()
    {
        ammoCount.text = objectToShoot.name + ": " + totalAmmo;

        if(Input.GetMouseButton(1))
        {
            isAiming = true;

            cinemachineCameraOffset.m_Offset.z = 1;

            if (Input.GetKey(shootKey) && readyToShoot && totalAmmo > 0)
            {
                Shoot();
            }
        }

        else
        {
            isAiming = false;

            cinemachineCameraOffset.m_Offset.z = -1;
        }

        
    }

    private void ResetShoot()
    {
        readyToShoot = true;
    }
}
