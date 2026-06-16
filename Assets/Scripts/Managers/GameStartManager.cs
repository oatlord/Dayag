using UnityEngine;

public class GameStartManager : MonoBehaviour
{
    public static GameStartManager instance;

    [Header("Ink Variables to Watch")]
    public bool LeaveHouse = false;

    [Header("Objects Controlled by LeaveHouse")]
    public GameObject[] objectsToDeactivateOnLeaveHouse;

    [Header("Debug Controls")]
    [Tooltip("Toggle this to force LeaveHouse = true and sync with Ink")]
    public bool debugLeaveHouse;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one InkObjectController found. Destroying this one.");
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }

        SyncAllFromInk();
    }

    void Start()
    {
        SyncAllFromInk();
    }

    void Update()
    {
        bool currentLeaveHouse = GetInkBool("LeaveHouse");

        if (currentLeaveHouse != LeaveHouse)
        {
            LeaveHouse = currentLeaveHouse;
            ApplyObjectStates();
        }
    }

    private void SyncAllFromInk()
    {
        if (DialogueManager.GetInstance() == null) return;

        LeaveHouse = GetInkBool("LeaveHouse");

        ApplyObjectStates();
    }

    private bool GetInkBool(string variableName)
    {
        if (DialogueManager.GetInstance() == null)
            return false;

        Ink.Runtime.Object variableState = DialogueManager.GetInstance().GetVariableState(variableName);

        if (variableState is Ink.Runtime.BoolValue boolValue)
        {
            return boolValue.value;
        }

        if (variableState == null)
        {
            // Debug.LogWarning($"Ink variable '{variableName}' was not found.");
        }
        else
        {
            // Debug.LogWarning($"Ink variable '{variableName}' is not a bool.");
        }

        return false;
    }

    public void SetLeaveHouse(bool value)
    {
        LeaveHouse = value;

        if (DialogueManager.GetInstance() != null)
        {
            DialogueManager.GetInstance().SetVariableState("LeaveHouse", value);
        }

        ApplyObjectStates();
    }

    private void ApplyObjectStates()
    {
        // LeaveHouse logic
        foreach (GameObject obj in objectsToDeactivateOnLeaveHouse)
        {
            if (obj != null)
            {
                obj.SetActive(!LeaveHouse);
            }
        }

    }

    private void OnValidate()
    {
        if (debugLeaveHouse != LeaveHouse)
        {
            debugLeaveHouse = LeaveHouse;
            SetLeaveHouse(debugLeaveHouse);
        }
    }
}