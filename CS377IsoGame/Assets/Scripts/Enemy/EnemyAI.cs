using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public abstract class EnemyAI : MonoBehaviour
{
    
    public float Health { get; protected set; }
    [SerializeField] public Health myHealth;
    public float AttackDamage { get; protected set; }
    protected NavMeshAgent agent;
    public Transform player;
    public LayerMask Walkable, whatIsPlayer;
    public LayerMask Player;

    // Patrolling
    public Vector3 walkPoint;
    public bool walkPointSet;
    public float walkPointRange;

    // States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;
    public bool stunned;
    
    // UI elements
    public Slider healthBar;

    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        Debug.Log(agent);
        player = GameObject.FindWithTag("Player").transform;
        stunned = false;
 
    }
    

    protected virtual void Die()
    {
        // Simple destroy for now
        Destroy(gameObject);
    }

    protected virtual void FixedUpdate()
    {
        
        // Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        //Debug.Log("Player in Sight range" + playerInSightRange);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        //Debug.Log("Player in attack range" + playerInAttackRange);

        if (!stunned)
        {
            
            agent.isStopped = false;
            if (!playerInSightRange && !playerInAttackRange) Patroling();
            if (playerInSightRange && !playerInAttackRange) ChasePlayer();
            if (playerInSightRange && playerInAttackRange) AttackPlayer();
        }
        else
        {
            agent.isStopped = true;
        }

        Health = myHealth.currentHealth;
        UpdateCanvas();
        if (Health < 0)
        {
            Die();
        }
    }


    protected virtual void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        // Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }

    protected virtual void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, Walkable))
        {
            walkPointSet = true;
        }
    }

    protected virtual void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    protected abstract void AttackPlayer();

    protected abstract void UpdateCanvas();

    protected void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
