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
    private int waypointIndex = 0;
    private float waitTimer = 0f;
    private bool isWaiting = false;

    [Header("Enemy Vision and Detection Settings")]
    public Transform playerHead;
    public float viewRadius = 15;
    public float viewDistance = 20;
    [Tooltip("Field of view angle in degrees for the AI's vision cone")]
    public float viewAngle = 90;
    public float turnSpeed = 3;
    public float alertRadius = 3;
    [Tooltip("Layers considered by the AI's vision spherecast (what can block sight)")]
    [SerializeField] private LayerMask sightLayerMask = ~0;
    [Tooltip("(Legacy – no longer used for detection filling. Detection speed is now distance-based via Detection Meter Settings.)")]
    public float timeToSeePlayer = 5;
    [Tooltip("Range of soldier's hit, i.e. the range that the soldier needs to kill the player.")]
    public float hitRange = 2;

    [Header("Enemy Multiplier Settings")]
    [Tooltip("AI's alert radius rises to this amount when the player is running.")]
    [SerializeField] private float alertRadiusHeightenedAmount;
    [Tooltip("AI's alert radius lowers to this amount when the player is crouching.")]
    [SerializeField] private float alertRadiusLessenedAmount;

    [Header("Alert to Other State Settings")]
    [SerializeField] private float m_alertToSwitchTimer;
    [Tooltip("Time till alert state can become other states.")]
    [SerializeField] private float alertToSwitchTimerMax = 2f;

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
    [SerializeField] private float detectionFillRateFar = 0.15f;
    [Tooltip("Multiplier applied to the fill rate while the player is only sensed by proximity " +
             "(inside alertRadius, not yet confirmed by line-of-sight) and still within the " +
             "alertToSwitchTimerMax verification window. Keeps the meter visibly creeping during " +
             "that window instead of staying pinned at zero. 1 = same speed as confirmed sight, " +
             "0 = old behaviour (frozen until verified).")]
    [SerializeField] private float proximityNoticeFillMultiplier = 0.35f;

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
        if (player == null)
        player = GameObject.FindGameObjectWithTag("Player");

        if (waypoints.Count > 0)
        {
            waypointIndex = 0;
            m_currentWaypoint = waypoints[0];
            navMeshAgent.SetDestination(m_currentWaypoint.position);
        }
    }

    void Update()
    {
        if (GameEndingManager.instance != null && GameEndingManager.instance.IsPlayingEnding) return;
        if (DialogueManager.GetInstance().dialogueIsPlaying) return;

        Debug.Log("Enemy reached player: " + enemyReachedPlayer);

        if (enemyReachedPlayer)
        {
            if (!hasHitAnimPlayed)
            {
                animController.SetTrigger("Hit");
                hasHitAnimPlayed = true;
                enemyHasHitPlayer = true;
                enemyHitStateActive = true;
            }
            animController.SetBool("IsSeeingPlayer", false);
            animController.SetBool("IsChasing", false);
            animController.SetBool("IsPatrolling", false);
            animController.SetBool("IsAlert", false);

            m_ResetStateTimer += Time.deltaTime;
            if (m_ResetStateTimer >= ResetStateTimer)
            {
                ResetAfterHit();
            }

            return;
        }

        enemySeesPlayer = CanSeePlayer();

        bool playerNoticed = enemySeesPlayer || playerInAlertRadius;
        bool wasAlertBefore = animController.GetBool("IsAlert");

        if (playerNoticed && !wasAlertBefore)
        {
            PlayAlertAudio();
            animController.SetBool("IsAlert", true);
        }

        // --- ALERT TIMER HANDLING ---
        if (animController.GetBool("IsAlert"))
        {
            m_alertToSwitchTimer += Time.deltaTime;
            
            // If the player is completely out of range/sight, start counting down to drop alert state
            if (!playerNoticed && m_alertToSwitchTimer >= alertToSwitchTimerMax)
            {
                animController.SetBool("IsAlert", false);
                m_alertToSwitchTimer = 0f;
            }
        }
        else
        {
            m_alertToSwitchTimer = 0f;
        }

        // --- HEAD TRACKING FIX ---
        // Only turn to look at the player if we actually actively notice them this frame!
        if (playerNoticed && !animController.GetBool("IsChasing"))
        {
            Vector3 dir = player.transform.position - transform.position;
            dir.y = 0;
            if (dir.sqrMagnitude > 0.001f)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, 
                    Quaternion.LookRotation(dir), turnSpeed * Time.deltaTime);
            }
        }

        // --- DETECTION METER PROGRESSION (CHASE BREAKER FIX) ---
        if (playerNoticed)
        {
            animController.SetBool("IsSeeingPlayer", true);

            float dist = Vector3.Distance(transform.position, player.transform.position);
            float proximity = Mathf.InverseLerp(viewDistance, alertRadius, dist);
            float fillRate = Mathf.Lerp(detectionFillRateFar, detectionFillRateNear, proximity);

            if (!enemySeesPlayer && m_alertToSwitchTimer < alertToSwitchTimerMax)
                fillRate *= proximityNoticeFillMultiplier;

            detectionProgress = Mathf.Clamp01(detectionProgress + fillRate * Time.deltaTime);

            if (detectionProgress >= 1f)
            {
                animController.SetBool("IsChasing", true);
                navMeshAgent.speed = enemyChaseSpeed;
            }
        }
        else
        {
            animController.SetBool("IsSeeingPlayer", false);
            
            // Slowly drain the meter while out of sight
            detectionProgress = Mathf.Clamp01(detectionProgress - detectionDrainRate * Time.deltaTime);

            // Only break the chase when the meter completely hits 0!
            if (detectionProgress <= 0f)
            {
                animController.SetBool("IsChasing", false);
            }
        }

        // === STATE MANAGEMENT FIX ===
        bool isChasing = animController.GetBool("IsChasing");
        bool isAlertStateActive = animController.GetBool("IsAlert");

        if (isChasing)
        {
            ChasePlayer();
            animController.SetBool("IsPatrolling", false);
            animController.SetBool("IsIdle", false);
        }
        else if (isAlertStateActive) // If player lost them but the alert/drain cooldown is still ticking
        {
            // Wipe their navigation path completely so they stop running to your ghost position!
            if (navMeshAgent.hasPath)
            {
                navMeshAgent.ResetPath(); 
            }
            animController.SetBool("IsPatrolling", false);
            animController.SetBool("IsIdle", true); // Stands still or plays alert idle
        }
        else // Completely calm, back to normal patrol
        {
            animController.SetBool("IsPatrolling", true);
            animController.SetBool("IsIdle", false);
            PatrolBehaviour();
        }

        // --- SPRINT / CROUCH SPHERECOLLIDER ADJUSTMENTS ---
        if (InputManager.GetInstance().IsSprinting)
            sphereCollider.radius = alertRadiusHeightenedAmount;
        else if (InputManager.GetInstance().IsCrouching)
            sphereCollider.radius = alertRadiusLessenedAmount;
        else
            sphereCollider.radius = alertRadius;

        sphereCollider.enabled = !isChasing;
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

        if (distanceToPlayer > viewDistance)
        {
            return false;
        }

        if (angleToPlayer <= viewAngle * 0.5f)
        {
            // Cast a sphere along the direction to the player and inspect all hits
            // in distance order. If any `Barrier` appears before the `Player`,
            // the player is considered hidden.
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, viewRadius, normalizedDirection, viewDistance, sightLayerMask.value);
            if (hits == null || hits.Length == 0)
            {
                return false;
            }

            System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));

            foreach (var h in hits)
            {
                var col = h.collider;
                if (col == null) continue;

                // Ignore our own colliders
                if (col.transform.IsChildOf(transform)) continue;

                // Ignore trigger colliders
                if (col.isTrigger) continue;

                if (col.gameObject.CompareTag("Barrier"))
                {
                    // A barrier blocks sight even if the player is behind it.
                    return false;
                }

                if (col.gameObject.CompareTag("Player"))
                {
                    // Player is the first non-ignored hit -> visible
                    return true;
                }

                // Otherwise keep checking next hit
            }

            // No player hit among non-ignored colliders
            return false;
        }

        return false;
    }

    private void ChasePlayer()
    {
        if (player == null) return;
        
        navMeshAgent.speed = enemyChaseSpeed;
        navMeshAgent.SetDestination(player.transform.position);
    }

    private void PatrolBehaviour()
    {
        if (waypoints.Count == 0) return;

        navMeshAgent.speed = enemyWalkSpeed;

        if (isWaiting)
        {
            waitTimer += Time.deltaTime;
            if (waitTimer >= waitAtWaypointTime)
            {
                isWaiting = false;
                waitTimer = 0f;
                MoveWaypoint();
            }
            return;
        }

        // If we reached the current waypoint
        if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance + 0.5f)
        {
            isWaiting = true;
            waitTimer = 0f;
            animController.SetBool("IsPatrolling", false); // optional brief idle at waypoint
            // You can set IsIdle = true here if you want a proper idle animation at each waypoint
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

    // Reset flags used by the hit sequence so the AI can resume normal behavior.
    public void ResetAfterHit()
    {
        hasHitAnimPlayed = false;
        enemyReachedPlayer = false;
        enemyHasHitPlayer = false;
        enemyHitStateActive = false;
        m_ResetStateTimer = 0f;
        detectionProgress = 0f;
        playerInAlertRadius = false;
        m_alertToSwitchTimer = 0f;
        isWaiting = false;
        waitTimer = 0f;

        animController.SetBool("IsChasing", false);
        animController.SetBool("IsAlert", false);
        animController.SetBool("IsSeeingPlayer", false);
        animController.SetBool("IsPatrolling", true);
        animController.SetBool("IsIdle", false);

        if (waypoints.Count > 0)
        navMeshAgent.SetDestination(waypoints[waypointIndex].position);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerPosition = other.transform.position;
            playerInAlertRadius = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerPosition = other.transform.position;
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