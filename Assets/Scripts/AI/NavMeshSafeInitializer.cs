using UnityEngine;
using UnityEngine.AI;

public class NavMeshSafeInitializer : MonoBehaviour
{
    private NavMeshAgent agent;
    
    // Drag your AI script component here in the Inspector
    [SerializeField] private MonoBehaviour myAIScript; 
    [SerializeField] private MonoBehaviour detectionScript;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        // Wait 0.1 seconds into the game for the build asset to finish loading
        Invoke(nameof(EnableEverything), 0.1f);
    }

    void EnableEverything()
    {
        // Ensure the agent is perfectly snapped to the static NavMesh
        if (NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 5.0f, NavMesh.AllAreas))
        {
            transform.position = hit.position;
            
            // 1. Turn on the NavMesh first
            agent.enabled = true;
            
            // 2. Turn on your AI script. This instantly triggers its Start() method!
            if (myAIScript != null)
            {
                myAIScript.enabled = true; 
                detectionScript.enabled = true;
                Debug.Log($"{gameObject.name} AI script successfully activated via component toggle.");
            }
        }
        else
        {
            Debug.LogError($"{gameObject.name} failed to find the static NavMesh asset in this build.");
        }
    }
}
