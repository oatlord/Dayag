using UnityEngine;

public class GameEndingManager : MonoBehaviour
{
    public static GameEndingManager instance;

    [Header("Ending Bools")]
    public bool HasHelpedHideo;
    public bool HasLetterFromTanaka;
    public bool ChoseMainRoute = false;
    public bool ChoseBackRoute = false;

    // Manager to be placed in Zone 5 to distinguish the next ending. Will work on this as Zone 5 is finished.

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one instance found. Destroying this instance.");
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
        // Read these two variables too just in case
        HasHelpedHideo = ((Ink.Runtime.BoolValue)DialogueManager.GetInstance().GetVariableState("HasHelpedHideo")).value;
        HasLetterFromTanaka = ((Ink.Runtime.BoolValue)DialogueManager.GetInstance().GetVariableState("HasLetterFromTanaka")).value;
    }

    public void SetMainRouteBool(bool choice)
    {
        Debug.Log("Set main route bool: " + choice);
        ChoseMainRoute = choice;
    }

    public void SetBackRouteBool(bool choice)
    {
        Debug.Log("Set back route bool: " + choice);
        ChoseMainRoute = choice;
    }

    void CalculateEnding()
    {
        HasHelpedHideo = ((Ink.Runtime.BoolValue) DialogueManager.GetInstance().GetVariableState("HasHelpedHideo")).value;
        HasLetterFromTanaka = ((Ink.Runtime.BoolValue) DialogueManager.GetInstance().GetVariableState("HasLetterFromTanaka")).value;
    }
}
