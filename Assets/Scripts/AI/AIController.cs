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
    [Tooltip("(Legacy – no longer used for detection filling. Detection speed is now distance-based via Detection Meter Settings.)")]
    public float timeToSeePlayer = 5;
    [Tooltip("Range of soldier's hit, i.e. the range that the soldier needs to kill the player.")]
    public float hitRange = 2;

    [Header("Enemy Multiplier Settings")]
    [Tooltip("AI's alert radius rises to this amount when the player is running.")]
    [SerializeField] private float alertRadiusHeightenedAmount;
    [Tooltip("AI's alert radius lowers to this amount when the player is crouching.")]
    [SerializeField] private float alertRadiusLessenedAmount;

    [Header("Enemy Movement Settings")]
    public float enemyChaseSpeed;
    public float enemyWalkSpeed;

    [Header("Enemy Audio Settings")]
    [SerializeField] private AudioSource alertAudioSource = null;

    // ─── Detection Meter ──────────────────────────────────────────────────────
    [Header("Detection Meter Settings")]
    [Tooltip("How quickly the detection meter drains per second when the player is out of sight.")]
    [SerializeField] private float detectionDrainRate = 1f;
    [Tooltip("Fill rate (progress/sec) when the player is AT the alertRadius (closest).")]
    [SerializeField] private float detectionFillRateNear = 1.0f;
    [Tooltip("Fill rate (progress/sec) when the player is AT the viewDistance (farthest visible).")]
    [SerializeField] private float detectionFillRateFar  = 0.15f;

    /// <summary>
    /// Normalised detection progress (0 = undetected, 1 = fully alerted / chasing).
    /// Read by DetectionMeterUI to drive the on-screen slider.
    /// </summary>
    [HideInInspector] public float detectionProgress = 0f;

    /// <summary>True while the player is inside the alert-radius trigger.</summary>
    [HideInInspector] public bool playerInAlertRadius = false;
    // ─────────────────────────────────────────────────────────────────────────

    private Vector3 playerPosition;
    private bool enemySeesPlayer;
    public bool enemyReachedPlayer = false;

    // RESET AI STATE POST-PLAYER DEATH
    [Header("Post-Chase AI Reset Settings")]
    public bool enemyHasHitPlayer = false;
    public bool enemyHitStateActive = false;
    private float m_ResetStateTimer = 0;
    [Tooltip("Time till AI state reset. Must match player script's death sequence length.")]
    [SerializeField] private float ResetStateTimer = 6f;

    // ENEMY COMPONENT REFERENCES
    private Animator animController;
    private SphereCollider sphereCollider;
    private NavMeshAgent navMeshAgent;

    // ENEMY ANIMATION SETTINGS
    private bool hasHitAnimPlayed = false;

    void Awake()
    {
        animController = GetComponent<Animator>();
        sphereCollider = GetComponent<SphereCollider>();
        navMeshAgent = GetComponent<NavMeshAgent>();
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

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
    }

    void Update()
    {
        if (GameEndingManager.instance != null && GameEndingManager.instance.IsPlayingEnding)
        {
            return;
        }

        if (DialogueManager.GetInstance().dialogueIsPlaying)
        {
            return;
        }
        
        if (enemyReachedPlayer)
        {
            if (!hasHitAnimPlayed)
            {
                animController.SetTrigger("Hit");
                hasHitAnimPlayed = true;
                enemyHasHitPlayer = true;
            }
            animController.SetBool("IsSeeingPlayer", false);
            animController.SetBool("IsChasing", false);
            animController.SetBool("IsPatrolling", false);
            animController.SetBool("IsAlert", false);
            return;
        }

        enemySeesPlayer = CanSeePlayer();

        // ── Alert-radius head-tracking ─────────────────────────────────────────
        // Rotate on Y to face the player while they are inside the proximity
        // sphere, without moving the AI's position.
        if (playerInAlertRadius && player != null && !animController.GetBool("IsChasing"))
        {
            Vector3 dirToPlayer = player.transform.position - transform.position;
            dirToPlayer.y = 0f;
            if (dirToPlayer.sqrMagnitude > 0.001f)
            {
                Quaternion targetRot = Quaternion.LookRotation(dirToPlayer);
                transform.rotation  = Quaternion.Slerp(transform.rotation, targetRot, turnSpeed * Time.deltaTime);
            }
        }

        if (enemySeesPlayer)
        {
            animController.SetBool("IsSeeingPlayer", true);

            // ── Distance-based fill rate ──────────────────────────────────────
            // proximity = 1 when player is at alertRadius (closest),
            // proximity = 0 when player is at viewDistance (farthest visible).
            float distToPlayer = Vector3.Distance(transform.position, player.transform.position);
            float proximity    = Mathf.InverseLerp(viewDistance, alertRadius, distToPlayer);
            float fillRate     = Mathf.Lerp(detectionFillRateFar, detectionFillRateNear, proximity);

            detectionProgress = Mathf.Clamp01(detectionProgress + fillRate * Time.deltaTime);

            if (detectionProgress >= 1f)
            {
                detectionProgress = 1f;
                animController.SetBool("IsChasing", true);
            }
        }
        else
        {
            animController.SetBool("IsChasing", false);
            animController.SetBool("IsSeeingPlayer", false);

            // ── Drain the meter gradually when the player is out of sight ──
            detectionProgress = Mathf.Clamp01(detectionProgress - detectionDrainRate * Time.deltaTime);
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

    public void PlayAlertAudio()
    {
        if (alertAudioSource != null && !alertAudioSource.isPlaying)
        {
            alertAudioSource.Play();
        }
    }

    private bool CanSeePlayer()
    {
        if (player == null || enemyReachedPlayer)
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
            return true;
        }

        if (distanceToPlayer > viewDistance)
        {
            return false;
        }

        if (angleToPlayer <= viewAngle * 0.5f)
        {
            if (Physics.SphereCast(transform.position, viewRadius, normalizedDirection, out hit, viewDistance))
            {
                if (hit.collider.gameObject.CompareTag("Player"))
                {
                    return true;
                }

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
            animController.SetBool("IsAlert", true);
            playerPosition    = other.transform.position;
            playerInAlertRadius = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            animController.SetBool("IsAlert", false);
            playerPosition    = Vector3.zero;
            playerInAlertRadius = false;
        }
    }

    private void OnDrawGizmos()
    {
        if (player == null) return;

        Vector3 rayDirection = player.transform.position - transform.position;
        float distanceToPlayer = rayDirection.magnitude;
        float angleToPlayer = Vector3.Angle(rayDirection, transform.forward);
        Vector3 normalizedDirection = rayDirection.normalized;
        float halfAngle = viewAngle * 0.5f;

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, alertRadius);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, viewDistance);

        Gizmos.color = Color.yellow;
        Vector3 leftDir = Quaternion.Euler(0, -halfAngle, 0) * transform.forward;
        Vector3 rightDir = Quaternion.Euler(0, halfAngle, 0) * transform.forward;
        Gizmos.DrawRay(transform.position, leftDir * viewDistance);
        Gizmos.DrawRay(transform.position, rightDir * viewDistance);

        Vector3 spherecastEnd = transform.position + normalizedDirection * viewDistance;
        Gizmos.color = Color.white;
        Gizmos.DrawLine(transform.position, spherecastEnd);
        Gizmos.DrawWireSphere(transform.position, viewRadius);
        Gizmos.DrawWireSphere(spherecastEnd, viewRadius);

        int sphereSteps = 4;
        for (int i = 1; i < sphereSteps; i++)
        {
            Vector3 stepPos = transform.position + normalizedDirection * (viewDistance * i / (float)(sphereSteps - 1));
            Gizmos.DrawWireSphere(stepPos, viewRadius);
        }

        bool canSee = distanceToPlayer <= alertRadius ||
            (distanceToPlayer <= viewDistance && angleToPlayer <= halfAngle);
        Gizmos.color = canSee ? Color.green : Color.red;
        Gizmos.DrawRay(transform.position, normalizedDirection * Mathf.Min(distanceToPlayer, viewDistance));

        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(transform.position, transform.position + leftDir * viewDistance * 0.2f);
        Gizmos.DrawLine(transform.position, transform.position + rightDir * viewDistance * 0.2f);
    }
}