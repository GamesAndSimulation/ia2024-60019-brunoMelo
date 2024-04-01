using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DroneAI : MonoBehaviour
{
    private Transform player;
    private Transform playerCenter;

    private PlayerHealth playerHealthScript;

    public LayerMask playerMask;

    //Patroling
    public Vector3 patrolPoint;
    private bool patrolPointSet;
    public float XandZrange;
    public float Yrange;
    private Vector3 originalPosition;

    //Attacking
    public float timeBetweenAttacks;
    private bool alreadyAttacked;

    private bool isMoving; // Flag to control movement

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    public float attackDamage = 10f;

    private Animator animator;

    private Target target;

    public GameObject projectilePrefab; // Reference to the projectile prefab
    public Transform firePoint; // Point from where the projectile is instantiated
    public float projectileSpeed = 10f; // Speed of the projectile
    public float movementSpeed = 5f;
    public float rotationSpeed = 10f;


    private void Awake()
    {
        player = GameObject.FindWithTag("Player").transform;
        playerCenter = player.Find("Player Center").transform;
        playerHealthScript = player.GetComponent<PlayerHealth>();
        animator = GetComponent<Animator>();
        target = GetComponent<Target>();

        originalPosition = transform.position; // Store the original position
        isMoving = true;

        animator.SetBool("IsMoving", true);
    }

    // Update is called once per frame
    void Update()
    {
        //Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, playerMask);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerMask);

        if (target.IsDead())
        {
            animator.SetBool("IsDead", true);
            return; // Exit the Update() method to stop further processing
        }

        if (!playerInSightRange && !playerInAttackRange)
        {
            Patroling();
        };

        if (playerInSightRange && !playerInAttackRange)
        {
            ChasePlayer();
        }

        if (playerInSightRange && playerInAttackRange)
        {
            AttackPlayer();
        }
    }

    private void Patroling()
    {
        if (!patrolPointSet)
        {
            SearchWalkPoint();
        }

        if (patrolPointSet && isMoving)
        {
            // Move towards the patrol point
            Vector3 direction = (patrolPoint - transform.position).normalized;
            transform.position += direction * Time.deltaTime * movementSpeed; // Adjust speed as needed

            // Smoothly rotate towards the patrol point
            Quaternion targetRotation = Quaternion.LookRotation(patrolPoint - transform.position);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed); // Adjust rotation speed as needed
        }

        Vector3 distanceToPatrolPoint = transform.position - patrolPoint;

        // Check if the drone reached the patrol point
        if (distanceToPatrolPoint.magnitude < 1f)
        {
            patrolPointSet = false;
        }

    }

    private void SearchWalkPoint()
    {
        float randomX = Random.Range(originalPosition.x - XandZrange, originalPosition.x + XandZrange); 
        float randomZ = Random.Range(originalPosition.z - XandZrange, originalPosition.z + XandZrange);
        float randomY = Random.Range(originalPosition.y - Yrange, originalPosition.y + Yrange);

        patrolPoint = new Vector3(randomX, randomY, randomZ);
        patrolPointSet = true;

        /*
        // Calculate random point in range
        float randomX = Random.Range(-XandZrange, XandZrange);
        float randomY = Random.Range(-Yrange, Yrange);
        float randomZ = Random.Range(-XandZrange, XandZrange);

        patrolPoint = transform.position + new Vector3(randomX, randomY, randomZ);
        patrolPointSet = true;
        */
    }

    private void ChasePlayer()
    {
        if(isMoving)
        {
            // Move towards the player
            Vector3 direction = (playerCenter.position - transform.position).normalized;
            transform.position += direction * Time.deltaTime * 5f; // Adjust speed as needed

            // Smoothly rotate towards the patrol point
            Quaternion targetRotation = Quaternion.LookRotation(playerCenter.position - transform.position);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed); // Adjust rotation speed as needed
        }
    }

    private void AttackPlayer()
    {
        //if (!animator.GetCurrentAnimatorStateInfo(0).IsName("PA_WarriorAttack_Clip"))
        //{
        //    animator.SetTrigger("Attack");

        //Make sure the enmy doens't move
            

            if (!alreadyAttacked)
            {

            
                isMoving = false;
                animator.SetBool("IsMoving", false);

                transform.LookAt(playerCenter);

                // Instantiate the projectile at the firePoint
                GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

                // Calculate the direction towards the player
                Vector3 direction = (playerCenter.position - firePoint.position).normalized;

                // Set the velocity of the projectile
                projectile.GetComponent<Rigidbody>().velocity = direction * projectileSpeed;

                alreadyAttacked = true;

                Invoke(nameof(ResetAttack), timeBetweenAttacks);

                Destroy(projectile, 20f);
            }
        //}
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
        isMoving = true;
        animator.SetBool("IsMoving", true);
    }
}
