using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyNavMesh : MonoBehaviour
{
    public Transform patrolRoute;
    public Transform player;

    private NavMeshAgent agent;
    private Transform[] locations;
    private int currentLocation = 0;
    private bool chasingPlayer = false;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Renderer>().material.color = new Color(128, 128, 128);
        agent = GetComponent<NavMeshAgent>();
        InitializePatrolRoute();
        MoveToNextPatrolLocation();
    }

    // Update is called once per frame
    void Update()
    {
        if (!chasingPlayer && !agent.pathPending && agent.remainingDistance < 0.2f)
        {
            MoveToNextPatrolLocation();
        }
    }

    void MoveToNextPatrolLocation()
    {
        if (locations.Length == 0) return;
        agent.SetDestination(locations[currentLocation].position);
        currentLocation = (currentLocation + 1) % locations.Length;
    }

    void InitializePatrolRoute()
    {
        locations = new Transform[patrolRoute.childCount];
        for (int i = 0; i < patrolRoute.childCount; i++)
        {
            locations[i] = patrolRoute.GetChild(i);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            Debug.Log("Player detected - start chasing!");
            chasingPlayer = true;
            GetComponent<Renderer>().material.color = new Color(100, 0, 0);
            agent.SetDestination(player.position);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Player")
        {
            Debug.Log("Player out of range - resume patrol.");
            chasingPlayer = false;
            GetComponent<Renderer>().material.color = new Color(128, 128, 128);
            MoveToNextPatrolLocation();
        }
    }
}
