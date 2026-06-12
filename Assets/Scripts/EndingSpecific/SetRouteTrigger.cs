using UnityEngine;

public class SetRouteTrigger : MonoBehaviour
{
    [Header("Route Choice Bools")]
    [Tooltip("Check only if this trigger is for if the player chose the main route.")]
    [SerializeField] private bool choseMainRoute;
    // private bool triggerEnabled;

    // void OnEnable()
    // {
    //     triggerEnabled = true;
    // }

    // void OnDisable()
    // {
    //     triggerEnabled = false;
    // }

    void OnTriggerEnter(Collider other)
    {
        // Do NOTHING if trigger itself is not enabled.
        // if (triggerEnabled == true)
        // {
        if (other.CompareTag("Player"))
        {
            // switch (choseMainRoute)
            // {
            //     case true:
            //         GameEndingManager.instance.SetMainRouteBool(true);
            //         GameEndingManager.instance.SetBackRouteBool(false);
            //         break;
            //     case false:
            //         GameEndingManager.instance.SetMainRouteBool(false);
            //         GameEndingManager.instance.SetBackRouteBool(true);
            //         break;
            // }
            if (choseMainRoute)
            {
                // If it isn't true, set it to true. Else, turn it false.
                if (!GameEndingManager.instance.ChoseMainRoute)
                {
                    GameEndingManager.instance.SetMainRouteBool(true);
                    GameEndingManager.instance.SetBackRouteBool(false);
                } else
                {
                    GameEndingManager.instance.ClearRoutes();
                }
            } else
            {
                if (!GameEndingManager.instance.ChoseBackRoute)
                {
                    GameEndingManager.instance.SetMainRouteBool(false);
                    GameEndingManager.instance.SetBackRouteBool(true);
                } else
                {
                    GameEndingManager.instance.ClearRoutes();
                }
            }

        }
        // } else
        // {
        //     return;
        // }
    }

}