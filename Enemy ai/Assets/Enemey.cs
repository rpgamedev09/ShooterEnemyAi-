using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemey : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;

    public LayerMask Groundlayer, PlayerLayer;

    //petrolling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    public bool playerInSightRange, PlayerInAttackRange;

    public bool LevelStart;
    public float sightRange, attackRange;
    public float timeBetweenBullets;
    public float health;
    public float shootForce;
    public float enemySpeed;

    bool alreadyAttacked;
    public GameObject projectile;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        enemySpeed = agent.GetComponent<NavMeshAgent>().speed;
        if (LevelStart)
        {
            playerInSightRange = Physics.CheckSphere(transform.position, sightRange, PlayerLayer);
            PlayerInAttackRange = Physics.CheckSphere(transform.position, attackRange, PlayerLayer);

            if (!playerInSightRange && !PlayerInAttackRange) Petrolling();
            if (playerInSightRange && !PlayerInAttackRange) ChasePlayer();
            if (PlayerInAttackRange && playerInSightRange) AttackPlayer();
        }
       
    }
    //Attack
    private void AttackPlayer()
    {
        agent.SetDestination(transform.position);
        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            ///AttackCode
            Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * shootForce, ForceMode.Impulse);
            ///

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenBullets) ;
        }

    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    //chasing
    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    //petrolling
    private void Petrolling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
        {
            agent.SetDestination(walkPoint);
        }
        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //walkpointReached
        if (distanceToWalkPoint.magnitude > 1f)
        {
            walkPointSet = false;

        }
    }

    private void SearchWalkPoint()
    {
        float randz = Random.Range(-walkPointRange, walkPointRange);
        float randx = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randx, transform.position.y, transform.position.z + randz);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, Groundlayer))
            walkPointSet = true;
    }

    //health and damage
    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
            Invoke(nameof(DestroyEnemy), 2f);
    }
    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }
    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
