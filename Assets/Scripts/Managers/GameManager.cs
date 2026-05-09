using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour, IDataPersistence
{
    public static GameManager instance;
    public bool PlayerIsAlive { get; private set; } = true;
    public Vector3 PlayerCheckpoint { get; private set; } = Vector3.zero;
    public bool HasPlayerCheckpoint { get; private set; } = false;

    [Header("References")]
    [SerializeField] private GameObject player;
    // private PlayerController playerController;

    [Header("UI References")]
    [SerializeField] private GameObject blackoutScreen;

    public void SaveData(GameData data)
    {
        data.playerCheckpointPosition = PlayerCheckpoint;
        data.hasPlayerCheckpoint = HasPlayerCheckpoint;
    }

    public void LoadData(GameData data)
    {
        PlayerCheckpoint = data.playerCheckpointPosition;
        HasPlayerCheckpoint = data.hasPlayerCheckpoint;
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
