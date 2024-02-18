using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserDroneController : EnemyAI
{
    [SerializeField] GameObject LaserConnectionContainer;

    public float PauseInterval; // How long the enemy will wait after reaching a walkpoint by the player
    public int MaxConnections; // How many other laser drones this drone can "connect" to

    private bool SearchForNewWalkPoint = true;
    private GameObject[] OtherLaserDrones;
    private List<GameObject> LaserConnections = new List<GameObject>();
    private int RandomSeed;

    protected override void Awake()
    {
        base.Awake();

        // Create a random seed for this drone which we'll use on each frame
        RandomSeed = Random.Range(0, 1000);

        // Instantiate MaxConnections laser connection containers and store them
        for (int i = 0; i < MaxConnections; i++)
        {
            LaserConnections.Add((GameObject)Instantiate(LaserConnectionContainer, Vector3.zero, Quaternion.identity, transform));
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        SearchForOtherLaserDronesInSightRange();
        DrawLaserToNearbyDrones(); 
    }

    protected override void Patroling()
    {
        base.Patroling();
    }

    protected override void ChasePlayer()
    {
        StartCoroutine(SearchAndMoveToWalkPointByPlayer());
    }

    protected override void AttackPlayer()
    {
        StartCoroutine(SearchAndMoveToWalkPointByPlayer());
    }

    protected override void UpdateCanvas()
    {
        healthBar.value = Health;
    }

    IEnumerator SearchAndMoveToWalkPointByPlayer()
    {
        if (!walkPointSet)
        {
            // Search for a walk point within attack range around the current player position
            walkPoint = new Vector3(player.position.x + Random.Range(-attackRange, attackRange), 
                                    transform.position.y,
                                    player.position.z + Random.Range(-attackRange, attackRange));

            // If that walkPoint is viable, move towards it
            if (Physics.Raycast(walkPoint, -transform.up, 2f, Walkable))
            {
                walkPointSet = true;
            }
            agent.SetDestination(walkPoint);
        }

        // If walk point is reached, wait before the enemy is allowed to search for a new walkpoint.
        // This should give time for the player to reach and attack the enemy.
        // Note: to prevent multiple coroutines from setting walkPointSet to false, we bar entry using the
        // 'SearchForNewWalkPoint' boolean which is set by the first call to the coroutine after a walk point is set.  
        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        if (distanceToWalkPoint.magnitude < 0.2f && SearchForNewWalkPoint)
        {
            SearchForNewWalkPoint = false;
            yield return new WaitForSeconds(PauseInterval);
            walkPointSet = false;
            SearchForNewWalkPoint = true;
        }
    }

    void SearchForOtherLaserDronesInSightRange()
    {
        // Find all other laser drones in the scene
        OtherLaserDrones = GameObject.FindGameObjectsWithTag("LaserDrone");

        // Shuffle the order of the drones in the array
        ReshuffleDrones();

        // Iterate through all drones and remove those out of the range
        int i = 0; // current array index
        int j = 0; // number of valid drones in range
        foreach (var drone in OtherLaserDrones)
        {
            // No self connections
            if (drone == OtherLaserDrones[i])
            {
                OtherLaserDrones[i] = null;
                i += 1;
                continue;
            }

            if ((drone.transform.position - transform.position).magnitude > sightRange)
            {
                // Remove out of range
                OtherLaserDrones[i] = null;
            }
            else
            {
                // In range, so this drone is valid -- iterate valid drones
                j += 1;
            }

            // Limit the number of drones in the final list to MaxConnections
            if (j >= MaxConnections)
            {
                OtherLaserDrones[i] = null;
            }

            i += 1;
        }
    }

    void DrawLaserToNearbyDrones()
    {
        int i = 0; // Current laser connection
        foreach (var drone in OtherLaserDrones)
        {
            if (drone != null)
            {
                var ThisLaser = LaserConnections[i].GetComponent<LineRenderer>();
                ThisLaser.enabled = true;
                ThisLaser.SetPosition(0, transform.position);
                ThisLaser.SetPosition(1, drone.transform.position);
                i += 1;
            }
        }

        // Disable remaining laser connections (i.e., all connections after i)
        DisableLaserConnections(i);
    }

    void DisableLaserConnections(int i)
    {
        while (i < MaxConnections)
        {
            var ThisLaser = LaserConnections[i].GetComponent<LineRenderer>();
            ThisLaser.enabled = false;
            i += 1;
        }
    }

    void ReshuffleDrones()
    {
        // Set random seed
        Random.seed = RandomSeed;

        // Knuth shuffle algorithm
        for (int i = 0; i < OtherLaserDrones.Length; i++)
        {
            GameObject tmp = OtherLaserDrones[i];
            int r = Random.Range(i, OtherLaserDrones.Length);
            OtherLaserDrones[i] = OtherLaserDrones[r];
            OtherLaserDrones[r] = tmp;
        }
    }
}
