using UnityEngine;
using UnityEngine.AI;

public class EnemyAIController : MonoBehaviour
{
    private Animator animator;
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask Terrain, Player;
    public float health;
    public GameObject sword;

    // Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    // Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;

    // States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange, playerAttackIsHit;

    private void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component not found on the Enemy GameObject.");
        }
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, Player);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, Player);

        if (!playerAttackIsHit)
        {
            if (!playerInSightRange && !playerInAttackRange)
                Patroling();

            if (playerInSightRange && !playerInAttackRange) 
                ChasePlayer();

            if (playerInAttackRange && playerInSightRange)
                AttackPlayer();

            if (Input.GetMouseButton(1))
            {
                sword.GetComponent<MeshCollider>().enabled = false;
            }
        }

        else
        {
            playerInAttackRange = false;
            playerInSightRange = false;
            HitByPlayerAttack();
        }
    }

    private void Patroling()
    {
        animator.SetBool("isWalking", true);
        animator.SetBool("isAttacking", false);
        sword.GetComponent<MeshCollider>().enabled = false;

        if (!walkPointSet)
            SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, Terrain))
            walkPointSet = true;
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
        animator.SetBool("isWalking", true);
        animator.SetBool("isAttacking", false);
        sword.GetComponent<MeshCollider>().enabled = false;
    }

    private void AttackPlayer()
    {
        agent.SetDestination(transform.position);
        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isAttacking", true);         
            sword.GetComponent<MeshCollider>().enabled = true;
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);            
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    private void HitByPlayerAttack()
    {
        Debug.Log("Enemy hit by attack");
        sword.SetActive(false);
        animator.SetBool("isWalking", false);
        animator.SetBool("isAttacking", false);
        animator.SetBool("isDead", true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Attack"))
        {
            playerAttackIsHit = true;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}