using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class TurnWallToTrigger : MonoBehaviour
{
    // Specifically turns an existing blockade into a trigger to pass through.
    private BoxCollider boxCollider;
    void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameEndingManager.instance.ChoseMainRoute)
        {
            boxCollider.isTrigger = true;
        } else
        {
            boxCollider.isTrigger = false;
        }
    }
}
