using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private GameObject visualCue;

    [SerializeField] private TextAsset inkJSON;

    private bool playerInRange;
    // private PlayerController playerController;

    private void Awake()
    {
        // visualCue.SetActive(false);
        playerInRange = false;
    }

    private void Update()
    {
        // Debug.Log($"DialogueTrigger Update - playerInRange: {playerInRange}, dialoguePlaying: {DialogueManager.GetInstance().dialogueIsPlaying}");
        
        if (playerInRange && !DialogueManager.GetInstance().dialogueIsPlaying)
        {
            visualCue.SetActive(true);
            bool interactPressed = InputManager.GetInstance().GetInteractPressed();
            // Debug.Log($"Checking for interact - result: {interactPressed}");
            if (interactPressed)
            {
                // Debug.Log("Interact button pressed! Entering dialogue mode.");
                // Debug.Log(inkJSON.text);
                DialogueManager.GetInstance().EnterDialogueMode(inkJSON);
            }
        } else
        {
            visualCue.SetActive(false);
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
