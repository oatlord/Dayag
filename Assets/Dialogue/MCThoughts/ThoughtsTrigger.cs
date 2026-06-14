using System.Collections;
using UnityEngine;
using TMPro;
using Ink.Runtime;

public class ThoughtsTrigger : MonoBehaviour
{
    [Header("Ink")]
    [SerializeField] private TextAsset inkJSON;

    [Header("UI")]
    [SerializeField] private Canvas dialogueCanvas;
    [SerializeField] private TextMeshProUGUI dialogueText;

    [Header("Settings")]
    [SerializeField] private float typingSpeed = 0.04f;
    [SerializeField] private float retriggerDelay = 0.5f;

    private Story _story;
    private Coroutine _typingCoroutine;
    private bool _canTrigger = true;

    private void Awake()
    {
        dialogueCanvas.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") || !_canTrigger) return;
        StartDialogue();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        StopDialogue();
        StartCoroutine(RetriggerCooldown());
    }

    private void StartDialogue()
    {
        _story = new Story(inkJSON.text);
        dialogueCanvas.enabled = true;

        if (_typingCoroutine != null)
            StopCoroutine(_typingCoroutine);

        if (_story.canContinue)
            _typingCoroutine = StartCoroutine(TypeLine(_story.Continue()));
    }

    private void StopDialogue()
    {
        if (_typingCoroutine != null)
        {
            StopCoroutine(_typingCoroutine);
            _typingCoroutine = null;
        }

        dialogueCanvas.enabled = false;
        dialogueText.text = "";
        _canTrigger = false;
    }

    private IEnumerator TypeLine(string line)
    {
        dialogueText.text = "";
        foreach (char c in line)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    private IEnumerator RetriggerCooldown()
    {
        yield return new WaitForSeconds(retriggerDelay);
        _canTrigger = true;
    }
}