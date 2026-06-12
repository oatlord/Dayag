using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script for SPECIFICALLY deactivating the PassThroughDialogueTrigger by the main road on runtime.

public class DeactivateDialogueTrigger : MonoBehaviour
{
    // [SerializeField] private string deactivateAtBool;
    [SerializeField] private PassthroughDialogueTrigger passthroughDialogueTrigger;
    void Start()
    {
        // if (gameObject.TryGetComponent<PassthroughDialogueTrigger>(out passthroughDialogueTrigger))
        // {
        //     Debug.Log("PassThrough Dialogue Trigger found in object.");
        // }
        // passthroughDialogueTrigger = GetComponent<PassthroughDialogueTrigger>();
    }

    void Update()
    {
        if (GameEndingManager.instance.TalkedToSoldiers)
        {
            passthroughDialogueTrigger.enabled = false;
        }
    }
}
