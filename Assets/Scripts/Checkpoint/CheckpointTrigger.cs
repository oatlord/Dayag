using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CheckpointTrigger : MonoBehaviour
{
    [SerializeField] private GameObject visualCue;
    [SerializeField] private TextMeshProUGUI savingStatusText; 
    [SerializeField] private Transform playerCheckpoint;
    
    private bool playerInRange;
    private Coroutine savingCoroutine;

    private void Awake()
    {
        playerInRange = false;
        if (savingStatusText != null)
            savingStatusText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (playerInRange && !DialogueManager.GetInstance().dialogueIsPlaying)
        {
            visualCue.SetActive(true);

            if (InputManager.GetInstance().GetInteractPressed())
            {
                // Debug.Log("Player checkpoint set.");
                
                // Start the saving animation
                if (savingCoroutine != null)
                    StopCoroutine(savingCoroutine);
                
                savingCoroutine = StartCoroutine(SavingAnimation());
                
                // Save the game
                SetPlayerCheckpoint();
                DataPersistenceManager.instance.SaveGame();
            }
        }
        else
        {
            visualCue.SetActive(false);
        }
    }

    private void SetPlayerCheckpoint()
    {
        GameManager.instance.SetPlayerCheckpoint(playerCheckpoint.position);
    }

    private IEnumerator SavingAnimation()
    {
        savingStatusText.gameObject.SetActive(true);
        string baseText = "Saving";
        float totalDuration = 3f;
        float dotSpeed = 0.25f; // time between dots
        float timer = 0f;

        while (timer < totalDuration)
        {
            for (int dots = 0; dots <= 3; dots++)
            {
                string currentText = baseText + new string('.', dots);
                savingStatusText.text = currentText;

                float waitTime = Mathf.Min(dotSpeed, totalDuration - timer);
                yield return new WaitForSeconds(waitTime);
                timer += waitTime;

                if (timer >= totalDuration) break;
            }
        }

        // Hide after animation finishes
        savingStatusText.gameObject.SetActive(false);
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