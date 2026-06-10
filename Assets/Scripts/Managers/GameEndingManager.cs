using UnityEngine;

public class GameEndingManager : MonoBehaviour
{
    public static GameEndingManager instance;
    public bool HasHelpedHideo {get; private set;}
    public bool HasLetterFromTanaka {get; private set;}
    public bool ChoseMainRoute {get; private set;} = false;
    public bool ChoseBackRoute {get; private set;} = false;

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
    }

    public void SetMainRouteBool(bool choice)
    {
        ChoseMainRoute = choice;
    }

    public void SetBackRouteBool(bool choice)
    {
        ChoseMainRoute = choice;
    }

    void CalculateEnding()
    {
        HasHelpedHideo = ((Ink.Runtime.BoolValue) DialogueManager.GetInstance().GetVariableState("HasHelpedHideo")).value;
        HasLetterFromTanaka = ((Ink.Runtime.BoolValue) DialogueManager.GetInstance().GetVariableState("HasLetterFromTanaka")).value;
    }
}
