using Ink.Parsed;
using Ink.Runtime;
using UnityEngine;

public class PassthroughDialogueTrigger : MonoBehaviour
{
    [SerializeField] private TextAsset inkJSON;
    private bool triggerEnabled;

    void OnEnable()
    {
        triggerEnabled = true;
    }

    void OnDisable()
    {
        triggerEnabled = false;
    }
    void OnTriggerEnter(Collider other)
    {
        if (!triggerEnabled) return;
        
        if (other.CompareTag("Player"))
        {
            DialogueManager.GetInstance().EnterDialogueMode(inkJSON);
        }
    }
}