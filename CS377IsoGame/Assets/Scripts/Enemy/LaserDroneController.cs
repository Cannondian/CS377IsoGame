using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserDroneController : EnemyAI
{
    [SerializeField] GameObject LaserConnectionContainer;

    public float PauseInterval; // How long the enemy will wait after reaching a walkpoint by the player
    public int MaxConnections; // How many other laser drones this drone can "connect" to
    public float DamagePerSecond; // Damage *per* laser connection per second

    private bool SearchForNewWalkPoint = true;
    private GameObject[] OtherLaserDrones;
    private List<GameObject> LaserConnections = new List<GameObject>();
    private int RandomSeed;

    private LayerMask PlayerLayerMask;
    private LayerMask WalkableLayerMask;

    protected override void Awake()
    {
        base.Awake();

        // Create a random seed for this drone which we'll use on each frame
        RandomSeed = Random.Range(0, 1000);

        // Instantiate MaxConnections laser connection containers and store them
        for (int i = 0; i < MaxConnections; i++)
        {
            GameObject Container = (GameObject)Instantiate(LaserConnectionContainer, 
                                                           transform.position,
                                                           Quaternion.identity, 
                                                           transform); // keep this drone as parent for this connection 
            SoundManager.PlaySoundLoop(SoundManager.Sound.Generic_Laser, Container.transform, 1f/MaxConnections);
            LaserConnections.Add(Container);
        }

        PlayerLayerMask = LayerMask.GetMask("Player");
        WalkableLayerMask = LayerMask.GetMask("Walkable");

        AttackDamage = DamagePerSecond * Time.fixedDeltaTime; // Scale attack damage to fit desired damage per second
    }

    void Start()
    {
        // This needs to be in start otherwise the SoundManager may not be instantiated yet...
        SoundManager.PlaySoundLoop(SoundManager.Sound.Generic_Hover, transform, 1f/MaxConnections);
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
            yield return new WaitForSeconds(PauseInterval + Random.Range(-PauseInterval/2f, PauseInterval/2f));
            walkPointSet = false;
            SearchForNewWalkPoint = true;
        }
    }

    void SearchForOtherLaserDronesInSightRange()
    {
        // Find all other laser drones in the scene
        OtherLaserDrones = GameObject.FindGameObjectsWithTag("LaserDrone");

        // Shuffle the order of the drones in the array
        ReshuffleDrones(); // This is breaking things at the moment

        // Iterate through all drones and remove those out of the range
        int i = 0; // current array index
        int j = 0; // number of valid drones in range
        foreach (var drone in OtherLaserDrones)
        {
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
        // Move laser connections to drone position
        for (int j = 0; j < MaxConnections; j++)
        {
            LaserConnections[j].transform.position = transform.position;
        }

        int i = 0; // Current laser connection
        foreach (var OtherDrone in OtherLaserDrones) 
        {
            if (OtherDrone != null 
                && !CheckForEnvironmentIntersection(transform.position, OtherDrone.transform.position))
            {
                LaserConnections[i].SetActive(true);
                var LaserRenderer = LaserConnections[i].GetComponent<LineRenderer>();
                LaserRenderer.SetPosition(0, transform.position);
                LaserRenderer.SetPosition(1, OtherDrone.transform.position);
                CheckForPlayerIntersection(transform.position, OtherDrone.transform.position);
                i += 1;
            }
        }

        // Disable remaining laser connections (i.e., all connections after i)
        DisableLaserConnections(i);
    }

    void CheckForPlayerIntersection(Vector3 Start, Vector3 End)
    {
        // Checks if player is between the start and end points of the laser connection and calls the damage event
        if (Physics.Raycast(Start, End - Start, (End - Start).magnitude, PlayerLayerMask))
        {
            // Call the player's take damage event, deal damage per physics tick if it hits player
            EventBus.TriggerEvent(EventTypes.Events.ON_PLAYER_DAMAGE_TAKEN, AttackDamage);
        }
    }

    bool CheckForEnvironmentIntersection(Vector3 Start, Vector3 End)
    {
        // Return whether the laser connection is interrupted by some piece of the environment
        // Note: environment is currently labeled as any objects in the walkable layer mask
        return Physics.Raycast(Start, End - Start, (End - Start).magnitude, WalkableLayerMask);
    }

    void DisableLaserConnections(int i)
    {
        while (i < MaxConnections)
        {
            LaserConnections[i].SetActive(false);
            i += 1;
        }
    }

    void ReshuffleDrones()
    {
        // Set random seed to make sure we shuffle the drones the same way each time
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
