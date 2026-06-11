using UnityEditor.EditorTools;
using UnityEngine;

public class GameEndingManager : MonoBehaviour
{
    public static GameEndingManager instance;

    [Header("Ending Bools")]
    public bool HasHelpedHideo;
    public bool HasLetterFromTanaka;
    public bool ChoseMainRoute = false;
    public bool ChoseBackRoute = false;
    public bool TalkedToSoldiers = false;

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

        if (DialogueManager.GetInstance() != null)
        {
            HasHelpedHideo = GetInkBool("HasHelpedHideo");
            HasLetterFromTanaka = GetInkBool("HasLetterFromTanaka");
        }
    }

    void Update()
    {
        if (!TalkedToSoldiers && (GetInkBool("ShowedTheTag") || GetInkBool("ShowedTheLetter")))
        {
            Debug.Log("Player has talked to soldiers.");
            TalkedToSoldiers = true;
        }
    }

    private bool GetInkBool(string variableName)
    {
        if (DialogueManager.GetInstance() == null)
        {
            return false;
        }

        Ink.Runtime.Object variableState = DialogueManager.GetInstance().GetVariableState(variableName);
        if (variableState is Ink.Runtime.BoolValue boolValue)
        {
            return boolValue.value;
        }

        if (variableState == null)
        {
            Debug.LogWarning($"Ink variable '{variableName}' was not found.");
        }
        else
        {
            Debug.LogWarning($"Ink variable '{variableName}' is not a bool: {variableState.GetType().Name}");
        }

        return false;
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

    // Clear both route booleans without invoking the other setter logic.
    public void ClearRoutes()
    {
        Debug.Log("Clearing route choices.");
        ChoseMainRoute = false;
        ChoseBackRoute = false;

        if (DialogueManager.GetInstance() != null)
        {
            DialogueManager.GetInstance().SetVariableState("ChoseMainRoute", false);
            DialogueManager.GetInstance().SetVariableState("ChoseBackRoute", false);
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
        HasHelpedHideo = ((Ink.Runtime.BoolValue)DialogueManager.GetInstance().GetVariableState("HasHelpedHideo")).value;
        HasLetterFromTanaka = ((Ink.Runtime.BoolValue)DialogueManager.GetInstance().GetVariableState("HasLetterFromTanaka")).value;
    }
}
