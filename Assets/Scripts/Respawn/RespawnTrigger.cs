using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor.Animations;

public class RespawnTrigger : MonoBehaviour
{
    [Header("UI Options")]
    [SerializeField] private GameObject RespawnPointSetText;
    [SerializeField] private GameObject visualCue;
    [SerializeField] private Transform RespawnPositionChild;

    private bool HasBeenInteractedWithOnce = false;
    private bool playerInRange = false;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Has been interacted with: " + HasBeenInteractedWithOnce);
        if (playerInRange && !DialogueManager.GetInstance().dialogueIsPlaying && HasBeenInteractedWithOnce)
        {
            visualCue.SetActive(true);
            bool interactPressed = InputManager.GetInstance().GetInteractPressed();
            if (interactPressed)
            {
                SetPlayerRespawnPoint();
            }
        }
        else
        {
            visualCue.SetActive(false);
            // RespawnPointSetText.SetActive(false);
        }
    }

    void SetPlayerRespawnPoint()
    {
        GameManager.instance.SetPlayerRevivePoint(RespawnPositionChild.position);
        Debug.Log("Player respawn point set to: " + RespawnPositionChild.name);
        RespawnPointSetText.SetActive(true);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerInRange = true;
            if (!HasBeenInteractedWithOnce)
            {
                SetPlayerRespawnPoint();
                // GameManager.instance.SetPlayerCheckpoint(RespawnPositionChild.position);
                HasBeenInteractedWithOnce = true;
                // RespawnPointSetText.SetActive(true);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerInRange = false;
            RespawnPointSetText.SetActive(false);
        }
    }
}
