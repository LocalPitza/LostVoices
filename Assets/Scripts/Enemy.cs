using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
//using Unity.AI.Navigation;
using UnityEngine.SceneManagement;



public class Enemy : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    [SerializeField] private Transform player; // Reference to the player's transform
    public Transform centerPoint; // Center point for random patrols
    public float hearRadius;
    public float hearRadiusRun;
    public float hearRadiusWalk;
    public float range;
    public float chaseDuration = 10f; // Duration for which the enemy will chase the player
    private float hearDistance;
    private bool isChasing = false;
    private Coroutine chaseTimerCoroutine;

    private void Start()
    {
        navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        hearDistance = Vector3.Distance(transform.position, player.position);

        // Patrol when not chasing
        if (!isChasing && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            PatrolRandomly();
        }

        // Detect player noise and start or continue chasing if within range
        if (PlayerMakingNoise())
        {
            StartChase();
        }
        else if (isChasing && !PlayerMakingNoise())
        {
            // Player is no longer heard, proceed to last known location and stop chasing after timer
            if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                isChasing = false; // Stop chasing if player is no longer heard and reached last known position
                if (chaseTimerCoroutine != null)
                {
                    StopCoroutine(chaseTimerCoroutine);
                    chaseTimerCoroutine = null;
                }
            }
        }
    }

    private bool PlayerMakingNoise()
    {
        var playerMovement = player.GetComponent<FirstPersonController>().CurrentInput;
        
        bool isWalking = Mathf.Abs(playerMovement.x) > 0.1f || Mathf.Abs(playerMovement.y) > 0.1f;
        bool isRunning = Mathf.Abs(playerMovement.x) > 3.5f || Mathf.Abs(playerMovement.y) > 3.5f;

        if (isRunning && hearDistance < hearRadiusRun)
        {
            Debug.Log("I CAN HEAR YOU RUNNING");
            return true;
        }
        if (isWalking && hearDistance < hearRadiusWalk)
        {
            Debug.Log("I CAN HEAR YOU WALKING");
            return true;
        }
        if (hearDistance < hearRadius)
        {
            Debug.Log("I CAN HEAR BREATHING");
            return true;
        }

        return false;
    }

    private void StartChase()
    {
        if (!isChasing)
        {
            isChasing = true;
            if (chaseTimerCoroutine != null)
            {
                StopCoroutine(chaseTimerCoroutine);
            }
            chaseTimerCoroutine = StartCoroutine(StopChaseAfterTime());
        }

        navMeshAgent.SetDestination(player.position); // Continuously update to player’s position
    }

    private IEnumerator StopChaseAfterTime()
    {
        yield return new WaitForSeconds(chaseDuration);
        isChasing = false;
    }

    private void PatrolRandomly()
    {
        Vector3 point;
        if (RandomPoint(centerPoint.position, range, out point))
        {
            navMeshAgent.SetDestination(point);
        }
    }

    private bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        Vector3 randomPoint = center + Random.insideUnitSphere * range;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
        {
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && hearDistance < hearRadius)
        {
            TriggerPlayerDeath();
        }
    }

    private void TriggerPlayerDeath()
    {
        Debug.Log("You have been killed!");
        // Display UI message instead of reloading scene
    }
}
