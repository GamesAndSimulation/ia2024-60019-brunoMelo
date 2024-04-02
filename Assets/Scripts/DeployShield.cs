using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeployShield : MonoBehaviour
{
    public GameObject shieldPrefab;
    public bool shieldUnlocked = false;
    public Transform attackPoint;
    public Transform cam;
    private bool readyToDeploy;
    public float deployCooldown = 10f;
    public float shieldDuration = 3f;

    void Start()
    {
        attackPoint = GameObject.Find("Shield Point").transform;
        cam = GameObject.FindWithTag("MainCamera").transform;
        readyToDeploy = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(shieldUnlocked)
        {
            if(Input.GetKeyDown(KeyCode.Q))
            {
                Deploy();
            }
        }
    }

    public void UnlockShield()
    {
        shieldUnlocked = true;
    }

    private void Deploy()
    {
        if(readyToDeploy)
        {
            readyToDeploy = false;

            GameObject shieldDeployed = Instantiate(shieldPrefab, attackPoint.position, cam.rotation);

            Invoke(nameof(ResetCooldown), deployCooldown);

            Destroy(shieldDeployed, shieldDuration);
        }
    }

    private void ResetCooldown()
    {
        readyToDeploy = true;
    }
}
