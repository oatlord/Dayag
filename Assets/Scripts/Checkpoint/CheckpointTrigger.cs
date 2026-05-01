using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CheckpointTrigger : MonoBehaviour
{
    [SerializeField] private GameObject visualCue;
    [SerializeField] private GameObject savingStatusText; 
    private bool playerInRange;

    private void Awake()
    {
        playerInRange = false;
    }

    void Update()
    {
        if (playerInRange && !DialogueManager.GetInstance().dialogueIsPlaying)
        {
            visualCue.SetActive(true);
            bool interactPressed = InputManager.GetInstance().GetInteractPressed();
            if (interactPressed)
            {
                savingStatusText.SetActive(true);
                DataPersistenceManager.instance.SaveGame();
            }
        }
        else
        {
            visualCue.SetActive(false);
            savingStatusText.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerInRange = true;
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
