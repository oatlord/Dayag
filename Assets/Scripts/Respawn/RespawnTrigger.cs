using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RespawnTrigger : MonoBehaviour
{
    [Header("UI Options")]
    [SerializeField] private GameObject RespawnPointSetText;
    [SerializeField] private GameObject visualCue;
    [SerializeField] private Transform RespawnPositionChild;

    private bool HasBeenInteractedWithOnce = false;
    private bool playerInRange = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // if (playerInRange)
        // {
        //     visualCue.SetActive(true);
        // } else
        // {
        //     visualCue.SetActive(false);
        // }
        Debug.Log("Has been interacted with: " + HasBeenInteractedWithOnce);
        if (playerInRange && !DialogueManager.GetInstance().dialogueIsPlaying && HasBeenInteractedWithOnce)
        {
            visualCue.SetActive(true);
            bool interactPressed = InputManager.GetInstance().GetInteractPressed();
            if (interactPressed)
            {
                // Debug.Log("Player respawn point set to: " + RespawnPositionChild.name);
                SetPlayerRespawnPoint();
                RespawnPointSetText.SetActive(true);
                // visualCue.SetActive(true);
                // GameManager.instance.SetPlayerCheckpoint(RespawnPositionChild.position);
                // DataPersistenceManager.instance.SaveGame();
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
        GameManager.instance.SetPlayerCheckpoint(RespawnPositionChild.position);
        Debug.Log("Player respawn point set to: " + RespawnPositionChild.name);
        // RespawnPointSetText.SetActive(true);
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
        }
    }
}
