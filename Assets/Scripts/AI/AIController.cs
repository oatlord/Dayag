using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(NavMeshAgent))]
public class AIController : MonoBehaviour
{
    [Header("Debug State Settings")]
    public bool allowMaterialDebug;
    public Material patrolMaterial = null;
    public Material idleMaterial = null;
    public Material alertMaterial = null;

    [Header("Player Reference")]
    public GameObject player;

    [Header("Enemy Waypoint and Patrol Settings")]
    public List<Transform> waypoints = new List<Transform>();
    [Tooltip("Time in seconds the enemy waits at each waypoint or its last position before moving to the next one")]
    public float waitAtWaypointTime = 2;
    public Transform m_currentWaypoint;

    [Header("Enemy Vision and Detection Settings")]
    public Transform playerHead;
    public float viewRadius = 15;
    public float viewDistance = 20;
    [Tooltip("Field of view angle in degrees for the AI's vision cone")]
    public float viewAngle = 90;
    public float turnSpeed = 3;
    public float alertRadius = 3;
    [Tooltip("Time in seconds before the enemy starts chasing the player after seeing them")]
    public float timeToSeePlayer = 5;

    [Header("Enemy Multipler Settings")]
    [Tooltip("AI's alert radius rises to this amount when the player is running.")]
    [SerializeField] private float alertRadiusHeightenedAmount;
    [Tooltip("AI's alert radius lowers to this amount when the player is crouching.")]
    [SerializeField] private float alertRadiusLessenedAmount;

    [Header("Enemy Movement Settings")]
    public float enemyChaseSpeed;
    public float enemyWalkSpeed;

    private Vector3 playerPosition;
    private bool enemySeesPlayer;
    private float m_timer = 0;

    // ENEMY COMPONENT REFERENCES
    private Animator animController;
    private SphereCollider sphereCollider;

    void Awake()
    {
        animController = GetComponent<Animator>();
        sphereCollider = GetComponent<SphereCollider>();
        sphereCollider.radius = alertRadius;

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
        enemySeesPlayer = CanSeePlayer();
        // if (Physics.SphereCast(playerHead.position, viewRadius, playerHead.forward, out RaycastHit hit, viewDistance))
        // {
        //     if (hit.collider.CompareTag("Player"))
        //     {
        //         Debug.Log("Player detected");
        //         enemySeesPlayer = true;
        //         animController.SetBool("IsSeeingPlayer", true);
        //     }
        // }
        // else
        // {
        //     enemySeesPlayer = false;
        //     animController.SetBool("IsSeeingPlayer", false);
        // }

        if (enemySeesPlayer)
        {
            animController.SetBool("IsSeeingPlayer", true);
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
        }
        else
        {
            gameObject.GetComponent<SphereCollider>().enabled = true;
        }

        if (InputManager.GetInstance().IsSprinting)
        {
            sphereCollider.radius = alertRadiusHeightenedAmount;
        }
        else if (InputManager.GetInstance().IsCrouching)
        {
            sphereCollider.radius = alertRadiusLessenedAmount;
        }
        else
        {
            sphereCollider.radius = alertRadius;
        }
    }

    public void ForceChase() 
    {
        Debug.Log("Chase forced.");

        animController.SetBool("IsSeeingPlayer", true);
        animController.SetBool("IsChasing", true);
    }

    private bool CanSeePlayer()
    {
        if (player == null)
        {
            return false;
        }

        RaycastHit hit;
        Vector3 rayDirection = player.transform.position - transform.position;
        float distanceToPlayer = rayDirection.magnitude;
        float angleToPlayer = Vector3.Angle(rayDirection, transform.forward);
        Vector3 normalizedDirection = rayDirection.normalized;

        if (distanceToPlayer <= alertRadius)
        {
            Debug.Log("close");
            return true;
        }

        if (distanceToPlayer > viewDistance)
        {
            return false;
        }

        if (angleToPlayer <= viewAngle * 0.5f)
        {
            Debug.Log("within field of view");

            if (Physics.Raycast(transform.position, normalizedDirection, out hit, viewDistance))
            {
                if (hit.collider.gameObject == player)
                {
                    Debug.Log("Can see player");
                    return true;
                }

                Debug.Log("Can not see player");
                return false;
            }
        }

        return false;
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
        if (player == null)
        {
            return;
        }

        Vector3 rayDirection = player.transform.position - transform.position;
        float distanceToPlayer = rayDirection.magnitude;
        float angleToPlayer = Vector3.Angle(rayDirection, transform.forward);
        Vector3 normalizedDirection = rayDirection.normalized;

        // Draw the alert radius sphere
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, alertRadius);

        // Draw the maximum view distance circle
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, viewDistance);

        // Draw the view cone edges
        Gizmos.color = Color.yellow;
        float halfAngle = viewAngle * 0.5f;
        Vector3 leftDir = Quaternion.Euler(0, -halfAngle, 0) * transform.forward;
        Vector3 rightDir = Quaternion.Euler(0, halfAngle, 0) * transform.forward;
        Gizmos.DrawRay(transform.position, leftDir * viewDistance);
        Gizmos.DrawRay(transform.position, rightDir * viewDistance);

        // Draw ray to player in the same color as detection logic
        bool canSee = distanceToPlayer <= alertRadius ||
            (distanceToPlayer <= viewDistance && angleToPlayer <= halfAngle);
        Gizmos.color = canSee ? Color.green : Color.red;
        Gizmos.DrawRay(transform.position, normalizedDirection * Mathf.Min(distanceToPlayer, viewDistance));

        // Draw a helper line to show the angle difference
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(transform.position, transform.position + leftDir * viewDistance * 0.2f);
        Gizmos.DrawLine(transform.position, transform.position + rightDir * viewDistance * 0.2f);
    }
}
