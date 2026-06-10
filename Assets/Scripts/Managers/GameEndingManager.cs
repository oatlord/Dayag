using UnityEngine;

public class GameEndingManager : MonoBehaviour
{
    public static GameEndingManager instance;

    [Header("Ending Bools")]
    public bool HasHelpedHideo;
    public bool HasLetterFromTanaka;
    public bool ChoseMainRoute = false;
    public bool ChoseBackRoute = false;

    [Header("Debug Inspector")]
    [Tooltip("Tick to force ChoseMainRoute = true and sync with Ink state.")]
    public bool debugSetMainRoute;
    [Tooltip("Tick to force ChoseBackRoute = true and sync with Ink state.")]
    public bool debugSetBackRoute;
    [Tooltip("Toggle this to set HasHelpedHideo and sync with Ink state.")]
    public bool debugHasHelpedHideo;
    [Tooltip("Toggle this to set HasLetterFromTanaka and sync with Ink state.")]
    public bool debugHasLetterFromTanaka;

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
        ChoseBackRoute = !choice;

        if (DialogueManager.GetInstance() != null)
        {
            DialogueManager.GetInstance().SetVariableState("ChoseMainRoute", choice);
            DialogueManager.GetInstance().SetVariableState("ChoseBackRoute", !choice);
        }
    }

  public void SetBackRouteBool(bool choice)
    {
        Debug.Log("Set back route bool: " + choice);
        ChoseMainRoute = choice;
        ChoseBackRoute = choice;
        ChoseMainRoute = !choice;

        if (DialogueManager.GetInstance() != null)
        {
            DialogueManager.GetInstance().SetVariableState("ChoseBackRoute", choice);
            DialogueManager.GetInstance().SetVariableState("ChoseMainRoute", !choice);
        }
    }

    public void SetHasHelpedHideo(bool value)
    {
        Debug.Log("Set HasHelpedHideo: " + value);
        HasHelpedHideo = value;

        if (DialogueManager.GetInstance() != null)
        {
            DialogueManager.GetInstance().SetVariableState("HasHelpedHideo", value);
        }
    }

    public void SetHasLetterFromTanaka(bool value)
    {
        Debug.Log("Set HasLetterFromTanaka: " + value);
        HasLetterFromTanaka = value;

        if (DialogueManager.GetInstance() != null)
        {
            DialogueManager.GetInstance().SetVariableState("HasLetterFromTanaka", value);
        }
    }

 private void OnValidate()
    {
        if (debugSetMainRoute)
        {
            debugSetMainRoute = false;
            SetMainRouteBool(true);
        }

        if (debugSetBackRoute)
        {
            debugSetBackRoute = false;
            SetBackRouteBool(true);
        }

        if (debugHasHelpedHideo != HasHelpedHideo)
        {
            SetHasHelpedHideo(debugHasHelpedHideo);
        }

        if (debugHasLetterFromTanaka != HasLetterFromTanaka)
        {
            SetHasLetterFromTanaka(debugHasLetterFromTanaka);
        }
    }

    void CalculateEnding()
    {
        HasHelpedHideo = ((Ink.Runtime.BoolValue) DialogueManager.GetInstance().GetVariableState("HasHelpedHideo")).value;
        HasLetterFromTanaka = ((Ink.Runtime.BoolValue) DialogueManager.GetInstance().GetVariableState("HasLetterFromTanaka")).value;
    }
}
