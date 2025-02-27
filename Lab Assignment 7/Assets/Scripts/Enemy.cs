using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using System.Collections.Generic;

public class Enemy : MonoBehaviour
{
    public Transform[] waypoints;
    public Transform player;
    public Transform goal;
    public float patrolSpeed = 3f;
    public float chaseSpeed = 5f;
    public float detectionRange = 5f;
    public float chaseDuration = 3f;
    public float stoppingDistance = 1f;
    private int currentWaypointIndex = 0;
    private bool isChasing = false;
    private float chaseTimer = 0f;
    
    private void Start()
    {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        agent.destination = goal.position;
    }
    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer < detectionRange && !isChasing)
        {
            StartCoroutine(ChasePlayer());
        }

        if (!isChasing)
        {
            Patrol();
        }
    }
    void Patrol()
    {
        Transform targetWaypoint = waypoints[currentWaypointIndex];
        MoveTowards(targetWaypoint.position, patrolSpeed);

        if (Vector3.Distance(transform.position, targetWaypoint.position) < stoppingDistance)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }
    }
    IEnumerator ChasePlayer()
    {
        isChasing = true;
        chaseTimer = 0f;

        while (chaseTimer < chaseDuration)
        {
            chaseTimer += Time.deltaTime;
            MoveTowards(player.position, chaseSpeed);
            yield return null;
        }

        isChasing = false;
    }
    void MoveTowards(Vector3 targetPosition, float speed)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;
        transform.LookAt(targetPosition);
    }
}