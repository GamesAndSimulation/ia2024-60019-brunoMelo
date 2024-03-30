using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAddon : MonoBehaviour
{
    private Rigidbody rb;

    private bool targetHit;

    public float projectileDamage;
    public GameObject impactEffect;
    public float impactForce = 30f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (targetHit)
            return;
        else
            targetHit = true;

        rb.isKinematic = true;

        transform.SetParent(collision.transform);

        Target target = collision.transform.GetComponent<Target>();
        if (target != null)
        {
            target.TakeDamage(projectileDamage);
        }

        if (collision.rigidbody != null)
        {
            collision.rigidbody.AddForce(-collision.contacts[0].normal * impactForce);
        }

        GameObject impactGO = Instantiate(impactEffect, collision.contacts[0].point, Quaternion.FromToRotation(Vector3.up, collision.contacts[0].normal));


        Destroy(impactGO, 2f);

        Destroy(gameObject, 0.1f);
    }
}
