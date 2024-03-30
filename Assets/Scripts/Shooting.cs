using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // Start is called before the first frame update
    void Start()
    {
        readyToShoot = true;
    }

    private void Shoot()
    {
        readyToShoot = false;

        GameObject projectile = Instantiate(objectToShoot, attackPoint.position, cam.rotation);

        Rigidbody projectileRB = projectile.GetComponent<Rigidbody>();

        Vector3 forceDirection = cam.transform.forward;

        /*
        RaycastHit hit;
        if(Physics.Raycast(cam.position, cam.forward, out hit, 500f))
        {
            forceDirection = (hit.point - attackPoint.position).normalized;
        }
        */

        Vector3 forceToAdd = cam.transform.forward * shootForce + transform.up * shootUpwardForce;

        projectileRB.AddForce(forceToAdd, ForceMode.Impulse);

        totalAmmo--;

        Invoke(nameof(ResetShoot), shootCooldown);

        Destroy(projectile, 20f);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(shootKey) && readyToShoot && totalAmmo > 0) 
        {
            Shoot();
        }
    }

    private void ResetShoot()
    {
        readyToShoot = true;
    }
}
