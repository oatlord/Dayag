using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class VirtualCamSetTarget : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private Transform target;

    void Awake()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        
        if (virtualCamera.Follow != null || virtualCamera.Follow != target)
        {
            // Debug.Log("Setting virtual camera follow target to " + target.name);
            virtualCamera.Follow = target;
        }

        if (virtualCamera.LookAt != null || virtualCamera.LookAt != target)
        {
            // Debug.Log("Setting virtual camera look at target to " + target.name);
            virtualCamera.LookAt = target;
        }
    }
}
