using UnityEngine;

// Script for SPECIFICALLY deactivating the main route blockade after talking to the soldiers
// and showing either the tag or the note.
public class DeactivateMainRouteBlockade : MonoBehaviour
{
    void Update()
    {
        if (GameEndingManager.instance.TalkedToSoldiers)
        {
            gameObject.SetActive(false);
        }
    }
}
