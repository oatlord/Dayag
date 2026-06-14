using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, IDataPersistence
{
    public static GameManager instance;

    // Player State
    public bool PlayerIsAlive { get; private set; } = true;
    public bool PlayerIsBeingChased { get; private set; } = false;

    // Player Revival System
    public Vector3 PlayerRevivePoint { get; private set; } = Vector3.zero;
    public bool HasRevivePoint { get; private set; } = false;

    // Player Checkpoint System
    public Vector3 PlayerCheckpoint { get; private set; } = Vector3.zero;
    public bool HasCheckpoint { get; private set; } = false;
    // Player Variables
    // public bool HasHelped;
    // public bool HasLetter;
    // public string NameOfChoice;

    [Header("References")]
    [SerializeField] private GameObject player;
    // private PlayerController playerController;

    [Header("UI References")]
    [SerializeField] private GameObject blackoutScreen;

    public void SaveData(GameData data)
    {
        // data.playerRevivePoint = PlayerRevivePoint;
        // data.hasPlayerRevivePoint = HasRevivePoint;

        data.playerPosition = PlayerCheckpoint;
        data.hasLastSavedPlayerPosition = HasCheckpoint;
        Debug.Log("SaveData in GameManager called.");

        // data.HasHelped = ReturnHasPlayerHelped();
        // data.HasLetter = ReturnDoesPlayerHaveLetter();
        // data.NameOfChoice = ReturnNameOfChoice();
    }

    public void LoadData(GameData data)
    {
        // PlayerRevivePoint = data.playerRevivePoint;
        // HasRevivePoint = data.hasPlayerRevivePoint;

        Debug.Log("LoadData in GameManager called.");

        // HasHelped = data.HasHelped;
        // HasLetter = data.HasLetter;
        // NameOfChoice = data.NameOfChoice;
    }

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

        // playerController = player.GetComponent<PlayerController>();
    }

    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
    }

    void Update()
    {
        Debug.Log("Player Revival Point: " + HasRevivePoint);
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

        // If scene is Zone 5, and the game ending manager isn't null, perform the following.
        if (SceneManager.GetActiveScene().name == "Zone 5" && GameEndingManager.instance != null)
        {
            if (GameEndingManager.instance.HasHelpedHideo)
            {
                StartCoroutine(StartEnding3LoadSequence());
            }
            else if (GameEndingManager.instance.ChoseBackRoute &&
                !GameEndingManager.instance.HasHelpedHideo &&
                !GameEndingManager.instance.HasLetterFromTanaka)
            {
                StartCoroutine(StartEnding1LoadSequence());
            }
        }
    }

    IEnumerator StartEnding1LoadSequence()
    {
        yield return new WaitForSeconds(5f);
        GameEndingManager.instance.GetEnding1();
    }

    IEnumerator StartEnding3LoadSequence()
    {
        yield return new WaitForSeconds(5f);
        GameEndingManager.instance.GetEnding3();
    }

    public void SetChasePlayerState(bool isBeingChased)
    {
        PlayerIsBeingChased = isBeingChased;
    }

    public void SetPlayerRevivePoint(Vector3 revivePosition)
    {
        PlayerRevivePoint = revivePosition;
        HasRevivePoint = true;
    }

    public void SetPlayerCheckpoint(Vector3 checkpointPosition)
    {
        PlayerCheckpoint = checkpointPosition;
        HasCheckpoint = true;
    }

    public void RevivePlayer()
    {
        if (!HasRevivePoint)
        {
            // Debug.LogWarning("No player revive point set. Cannot revive to checkpoint.");
            // return;
            Transform sceneDefaultPlayerSpawn = GameObject.Find("SceneDefaultPlayerSpawn").transform;
            Debug.Log("Revived Player at default spawn point");
            player.transform.position = sceneDefaultPlayerSpawn.position;
            PlayerIsAlive = true;
        }
        else
        {
            Debug.Log("Revived Player at: " + PlayerRevivePoint);
            player.transform.position = new Vector3(PlayerRevivePoint.x, 0, PlayerRevivePoint.z);
            PlayerIsAlive = true;
        }

        // blackoutScreen.SetActive(false);
    }
}
