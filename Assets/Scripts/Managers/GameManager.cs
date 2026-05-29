using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;

public class GameManager : MonoBehaviour, IDataPersistence
{
    public static GameManager instance;

    // Player State
    public bool PlayerIsAlive { get; private set; } = true;

    // Player Checkpoint System
    public Vector3 PlayerCheckpoint { get; private set; } = Vector3.zero;
    public bool HasPlayerCheckpoint { get; private set; } = false;

    // Player Variables
    public bool HasHelped;
    public bool HasLetter;
    public string NameOfChoice;

    [Header("References")]
    [SerializeField] private GameObject player;
    // private PlayerController playerController;

    [Header("UI References")]
    [SerializeField] private GameObject blackoutScreen;

    public void SaveData(GameData data)
    {
        data.playerCheckpointPosition = PlayerCheckpoint;
        data.hasPlayerCheckpoint = HasPlayerCheckpoint;

        // data.HasHelped = ReturnHasPlayerHelped();
        // data.HasLetter = ReturnDoesPlayerHaveLetter();
        // data.NameOfChoice = ReturnNameOfChoice();
    }

    public void LoadData(GameData data)
    {
        PlayerCheckpoint = data.playerCheckpointPosition;
        HasPlayerCheckpoint = data.hasPlayerCheckpoint;

        // HasHelped = data.HasHelped;
        // HasLetter = data.HasLetter;
        // NameOfChoice = data.NameOfChoice;
    }

    void Update()
    {
        Debug.Log("Player Checkpoint: " + PlayerCheckpoint);
    }

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one instance found. Destroying this instance.");
            Destroy(this.gameObject);
        } else
        {
            instance = this;
        }

        // playerController = player.GetComponent<PlayerController>();
    }

    // private bool ReturnHasPlayerHelped()
    // {
    //     return ((Ink.Runtime.BoolValue) DialogueManager.GetInstance().GetVariableState("HasHelped")).value;
    // }

    // private bool ReturnDoesPlayerHaveLetter()
    // {
    //     return ((Ink.Runtime.BoolValue) DialogueManager.GetInstance().GetVariableState("HasLetter")).value;
    // }

    // private string ReturnNameOfChoice()
    // {
    //     return ((Ink.Runtime.StringValue) DialogueManager.GetInstance().GetVariableState("NameOfChoice")).value;
    // }

    public void KillPlayer()
    {
        PlayerIsAlive = false;
        blackoutScreen.SetActive(true);
    }

    public void SetPlayerCheckpoint(Vector3 checkpointPosition)
    {
        PlayerCheckpoint = checkpointPosition;
        HasPlayerCheckpoint = true;
    }

    public void RevivePlayer()
    {
        if (!HasPlayerCheckpoint)
        {
            Debug.LogWarning("No player checkpoint set. Cannot revive to checkpoint.");
            return;
        }

        Debug.Log("Revived Player at: " + PlayerCheckpoint);
        player.transform.position = new Vector3(PlayerCheckpoint.x, 0, PlayerCheckpoint.z);
        PlayerIsAlive = true;
        // blackoutScreen.SetActive(false);
    }
}
