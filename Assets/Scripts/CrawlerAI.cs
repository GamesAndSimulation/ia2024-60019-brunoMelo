using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CrawlerAI : MonoBehaviour
{
    public NavMeshAgent agent;

    private Transform player;

    private PlayerHealth playerHealthScript;

    public LayerMask groundMask, playerMask;

    //Patroling
    public Vector3 walkPoint;
    private bool walkPointSet;
    public float walkPointRange;

    //Attacking
    public float timeBetweenAttacks;
    private bool alreadyAttacked;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    public float attackDamage = 10f;

    private Animator animator;

    private Target target;

    [SerializeField] private AudioClip runningSound;
    [SerializeField] private AudioClip attackSound;


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

        if(target.IsDead())
        {
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

        //SoundFXManager.instance.PlaySoundFXClip(runningSound, transform, 0.2f);
    }

    private void Patroling()
    {
        if (!walkPointSet)
        {
            SearchWalkPoint();
        }

        if(walkPointSet)
        {
            agent.SetDestination(walkPoint);
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
        }
            
    }

    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if(Physics.Raycast(walkPoint, -transform.up, 2f, groundMask))
        {
            walkPointSet = true;
        }
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        if(!animator.GetCurrentAnimatorStateInfo(0).IsName("PA_WarriorAttack_Clip"))
        {
            animator.SetTrigger("Attack");

            //Make sure the enmy doens't move
            agent.SetDestination(transform.position);

            transform.LookAt(player);

            if (!alreadyAttacked)
            {

                SoundFXManager.instance.PlaySoundFXClip(attackSound, transform, 1f);

                playerHealthScript.TakeDamage(attackDamage);

                alreadyAttacked = true;

                Invoke(nameof(ResetAttack), timeBetweenAttacks);
            }
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void RunningSound()
    {
        //SoundFXManager.instance.PlaySoundFXClip(runningSound, transform, 0.2f);
    }
}
