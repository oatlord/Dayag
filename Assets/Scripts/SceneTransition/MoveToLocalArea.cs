using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToLocalArea : MonoBehaviour
{
    [SerializeField] private Transform localAreaToMoveToArea;
    [SerializeField] private GameObject player;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Player entered local area trigger. Moving player to local area.");
        CharacterController cc = player.GetComponent<CharacterController>();
        if (cc != null)
        {
            cc.enabled = false;
        }
        player.transform.position = localAreaToMoveToArea.position;
        if (cc != null)
        {
            cc.enabled = true;
        }
        // if (other.CompareTag("Player"))
        // {
        //     Debug.Log("Player entered local area trigger. Moving player to local area.");

        //     // GameObject go = other.gameObject;
        //     // Debug.Log(go.name);
        //     // go.transform.position = localAreaToMoveToArea.position;

        //     // Transform targetTransform = other.transform;
        //     // if (other.attachedRigidbody != null)
        //     // {
        //     //     targetTransform = other.attachedRigidbody.transform;
        //     // }
        //     // else if (other.transform.parent != null)
        //     // {
        //     //     targetTransform = other.transform.root;
        //     // } else {
        //     //     Debug.LogWarning("Player collider does not have attached Rigidbody or parent. Moving the collider's transform.");
        //     // }

        //     // targetTransform.position = localAreaToMoveToArea.position;
        // }
    }
}
