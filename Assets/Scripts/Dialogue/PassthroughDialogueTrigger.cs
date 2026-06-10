using Ink.Parsed;
using Ink.Runtime;
using UnityEngine;

public class PassthroughDialogueTrigger : MonoBehaviour
{
    [SerializeField] private TextAsset inkJSON;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DialogueManager.GetInstance().EnterDialogueMode(inkJSON);
        }
    }
}
