using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DroneAI : MonoBehaviour
{
    public NavMeshAgent agent;

    private Transform player;

    private PlayerHealth playerHealthScript;

    public LayerMask groundMask, playerMask;

    //Patroling
    public Vector3 patrolPoint;
    private bool patrolPointSet;
    public float XandZrange;
    public float Yrange;

    //Attacking
    public float timeBetweenAttacks;
    private bool alreadyAttacked;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    public float attackDamage = 10f;

    private Animator animator;

    private Target target;

    public GameObject projectilePrefab; // Reference to the projectile prefab
    public Transform firePoint; // Point from where the projectile is instantiated
    public float projectileSpeed = 10f; // Speed of the projectile


    private void Awake()
    {
        player = GameObject.FindWithTag("Player").transform;
        playerHealthScript = player.GetComponent<PlayerHealth>();
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        target = GetComponent<Target>();
    }

    // Update is called once per frame
    void Update()
    {
        //Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, playerMask);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerMask);

        if (target.IsDead())
        {
            // Stop the NavMeshAgent from moving
            agent.isStopped = true;
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

        if (patrolPointSet)
        {
            agent.SetDestination(patrolPoint);
        }

        Vector3 distanceToWalkPoint = transform.position - patrolPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
        {
            patrolPointSet = false;
        }

    }

    private void SearchWalkPoint()
    {
        // Calculate random point in range
        float randomX = Random.Range(-XandZrange, XandZrange);
        float randomY = Random.Range(-Yrange, Yrange);
        float randomZ = Random.Range(-XandZrange, XandZrange);

        patrolPoint = new Vector3(transform.position.x + randomX, transform.position.y + randomY, transform.position.z + randomZ);

        patrolPointSet = true;
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        //if (!animator.GetCurrentAnimatorStateInfo(0).IsName("PA_WarriorAttack_Clip"))
        //{
        //    animator.SetTrigger("Attack");

            //Make sure the enmy doens't move
            agent.SetDestination(transform.position);

            transform.LookAt(player);

            if (!alreadyAttacked)
            {

                // Instantiate the projectile at the firePoint
                GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

                // Calculate the direction towards the player
                Vector3 direction = (player.position - firePoint.position).normalized;

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
    }
}
