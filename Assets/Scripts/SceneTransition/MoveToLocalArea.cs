using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToLocalArea : MonoBehaviour
{
    [SerializeField] private Transform localAreaToMoveToArea;
    [SerializeField] private GameObject player;

    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
    }

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
    }
}
