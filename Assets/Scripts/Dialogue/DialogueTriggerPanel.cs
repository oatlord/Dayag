using System.Collections;
using UnityEngine;

public class DialogueTriggerWithPanel : MonoBehaviour
{
    [Header("NPC Settings")]
    [SerializeField] private string NPCName;
    [SerializeField] private GameObject visualCue;

    [Header("Dialogue")]
    [SerializeField] private TextAsset inkJSON;
    [SerializeField] private bool isOneTimeDialogue = true;

    [Header("Transition Panel")]
    [SerializeField] private GameObject transitionPanel;
    [SerializeField] private CanvasGroup panelCanvasGroup;
    [SerializeField] private float fadeDuration = 0.8f;
    [SerializeField] private float delayBeforeDialogue = 0.3f;

    [Header("UI To Disable After Dialogue")]
    [SerializeField] private GameObject chatUI;

    private bool playerInRange;
    private bool isTransitioning = false;
    private bool dialogueUsed = false;

    private void Awake()
    {
        // Panel starts fully visible, no animation
        if (transitionPanel != null)
            transitionPanel.SetActive(true);

        if (panelCanvasGroup != null)
            panelCanvasGroup.alpha = 1f;
    }

    private void Update()
    {
        if (isOneTimeDialogue && dialogueUsed) return;

        if (playerInRange && !DialogueManager.GetInstance().dialogueIsPlaying && !isTransitioning)
        {
            visualCue.SetActive(true);

            if (InputManager.GetInstance().GetInteractPressed())
            {
                StartCoroutine(FadeOutPanelThenStartDialogue());
            }
        }
        else
        {
            visualCue.SetActive(false);
        }
    }

    private IEnumerator FadeOutPanelThenStartDialogue()
    {
        isTransitioning = true;
        visualCue.SetActive(false);

        // Fade the dark panel OUT
        yield return StartCoroutine(FadePanel(1f, 0f, fadeDuration));

        // Small delay then start dialogue
        yield return new WaitForSeconds(delayBeforeDialogue);
        DialogueManager.GetInstance().EnterDialogueMode(inkJSON);

        yield return StartCoroutine(WaitForDialogueToEnd());

        isTransitioning = false;
    }

    private IEnumerator WaitForDialogueToEnd()
    {
        // Wait a frame so dialogueIsPlaying has time to flip true
        yield return null;

        while (DialogueManager.GetInstance().dialogueIsPlaying)
        {
            yield return null;
        }

        OnDialogueFinished();
    }

    private void OnDialogueFinished()
    {
        dialogueUsed = true;

        if (transitionPanel != null)
            transitionPanel.SetActive(false);

        if (panelCanvasGroup != null)
            panelCanvasGroup.alpha = 0f;

        if (chatUI != null)
            chatUI.SetActive(false);

        if (visualCue != null)
            visualCue.SetActive(false);

        if (isOneTimeDialogue)
            this.enabled = false;
    }

    private IEnumerator FadePanel(float fromAlpha, float toAlpha, float duration)
    {
        if (panelCanvasGroup == null) yield break;

        float elapsed = 0f;
        panelCanvasGroup.alpha = fromAlpha;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            panelCanvasGroup.alpha = Mathf.Lerp(fromAlpha, toAlpha, elapsed / duration);
            yield return null;
        }

        panelCanvasGroup.alpha = toAlpha;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
            playerInRange = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
            playerInRange = false;
    }
}