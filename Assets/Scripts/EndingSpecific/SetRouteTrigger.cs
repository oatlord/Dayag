using UnityEngine;

public class SetRouteTrigger : MonoBehaviour
{
    [Header("Route Choice Bools")]
    [Tooltip("Check only if this trigger is for if the player chose the main route.")]
    [SerializeField] private bool choseMainRoute;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            switch (choseMainRoute)
            {
                case true:
                    GameEndingManager.instance.SetMainRouteBool(true);
                    GameEndingManager.instance.SetBackRouteBool(false);
                    break;
                case false:
                    GameEndingManager.instance.SetMainRouteBool(false);
                    GameEndingManager.instance.SetBackRouteBool(true);
                    break;
            }
        }
    } 
    
}