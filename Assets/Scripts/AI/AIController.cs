using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    [Header("Debug State Settings")]
    public Material patrolMaterial;
    public Material idleMaterial;
    public Material alertMaterial;

    [Header("Enemy Waypoint and Patrol Settings")]
    public List<Transform> waypoints = new List<Transform>();
    [Tooltip("Time in seconds the enemy waits at each waypoint or its last position before moving to the next one")]
    public float waitAtWaypointTime = 2;
    public Transform m_currentWaypoint;

    [Header("Enemy Vision Settings")]
    public Transform playerHead;
    public float viewRadius = 15;
    public float viewDistance = 20;
    public float turnSpeed = 3;
    [Tooltip("Time in seconds before the enemy starts chasing the player after seeing them")]
    public float timeToSeePlayer = 5;

    [Header("Enemy Movement Settings")]
    public float enemyChaseSpeed;
    public float enemyWalkSpeed;

    private Vector3 playerPosition;
    private bool enemySeesPlayer;
    private float m_timer = 0;

    private Animator animController;

    void Awake()
    {
        animController = GetComponent<Animator>();

        if (waypoints.Count == 0)
        {
            throw new Exception("No waypoints assigned to the AIController. Please assign waypoints in the inspector.");
        }
    }

    void Start()
    {
        gameObject.transform.position = waypoints[0].position;
        m_currentWaypoint = waypoints[0];
    }

    void Update()
    {
        if (Physics.SphereCast(playerHead.position, viewRadius, playerHead.forward, out RaycastHit hit, viewDistance))
        {
            if (hit.collider.CompareTag("Player"))
            {
                Debug.Log("Player detected");
                enemySeesPlayer = true;
                animController.SetBool("IsSeeingPlayer", true);
            }
        }
        else
        {
            enemySeesPlayer = false;
            animController.SetBool("IsSeeingPlayer", false);
        }

        if (enemySeesPlayer)
        {
            m_timer += Time.deltaTime;

            if (m_timer >= timeToSeePlayer)
            {
                m_timer = 0;
                animController.SetBool("IsChasing", true);
            }
        }
        else
        {
            m_timer = 0;
            animController.SetBool("IsChasing", false);
            animController.SetBool("IsSeeingPlayer", false);
        }

        if (animController.GetBool("IsChasing"))
        {
            gameObject.GetComponent<SphereCollider>().enabled = false;
        } else
        {
            gameObject.GetComponent<SphereCollider>().enabled = true;
        }
    }

    public void MoveWaypoint()
    {
        if (m_currentWaypoint == waypoints[waypoints.Count - 1])
        {
            m_currentWaypoint = waypoints[0];
        }
        else
        {
            m_currentWaypoint = waypoints[waypoints.IndexOf(m_currentWaypoint) + 1];
        }
    }

    public Vector3 ReturnPlayerPosition()
    {
        return playerPosition;
    }

    public void StopChase()
    {
        animController.SetBool("IsChasing", false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player Detected");

            animController.SetBool("IsAlert", true);

            playerPosition = other.transform.position;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            animController.SetBool("IsAlert", false);

            playerPosition = Vector3.zero;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(playerHead.position, viewRadius);
        Gizmos.DrawWireSphere(playerHead.position + playerHead.forward * viewDistance, viewRadius);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, gameObject.GetComponent<NavMeshAgent>().stoppingDistance);
    }
}
