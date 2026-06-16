using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenePersistenceManager : MonoBehaviour, IDataPersistence
{
    // This manager is for persisting player positions across scenes, recording which exit they took to get to the next scene, and any other scene-specific data that needs to be saved and loaded when transitioning between scenes. 
    // It will work alongside the GameManager and GameSceneManager to ensure that all necessary data is properly saved and loaded during scene transitions.
    public static ScenePersistenceManager instance;
    // Obtain this every scene change. Load the player here by DEFAULT.
    [SerializeField] private Transform sceneDefaultPlayerSpawn;
    // Private reference to the spawn point that is to be used. Referenced for getting its current rotation and setting input maps.
    private Transform spawnPointToUse;
    // Check if player is loading from an existing save.
    private bool loadingFromSave;
    // Transform from save.
    private Transform spawnPointFromSave;
    // Player position loaded from save (avoid relying on a Transform that may not exist yet)
    private Vector3 savedPlayerPosition;
    // ID of the exit trigger last interacted with.
    public string LastExitTriggerID;
    public GameObject player;
    [SerializeField] private GameObject playerPrefab;

    public void SaveData(GameData data)
    {
        // Nothing here.
    }

    public void LoadData(GameData data)
    {
        Debug.Log("LoadData in ScenePersistence called.");
        // Use the player's position from data here to load the player in instead.
        loadingFromSave = data.hasLastSavedPlayerPosition;
        Debug.Log("Loading from save: " + loadingFromSave);

        if (loadingFromSave)
        {
            savedPlayerPosition = data.playerPosition;
            // Clear the flag so subsequent scene loads won't re-trigger loading-from-save
            data.hasLastSavedPlayerPosition = false;
        }
        // spawnPointFromSave = data.playerPosition;
    }

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one ScenePersistenceManager instance exists.");
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        sceneDefaultPlayerSpawn = GameObject.Find("SceneDefaultPlayerSpawn").transform;

        // End operations if sceneDefaultPlayerSpawn is NULL.
        if (sceneDefaultPlayerSpawn == null)
        {
            return;
        }

        playerPrefab = Resources.Load<GameObject>("Player/Player New");

        // if (player == null)
        // {
        //     player = Instantiate(playerPrefab, sceneDefaultPlayerSpawn.position, sceneDefaultPlayerSpawn.rotation);
        // }

        if (loadingFromSave)
        {
            // Debug.Log("Loading from save. Spawning player in last checkpointed position.");
            // Use the saved player position directly; use the default spawn's rotation for input mapping
            spawnPointToUse = sceneDefaultPlayerSpawn;
            player = Instantiate(playerPrefab, savedPlayerPosition, sceneDefaultPlayerSpawn.rotation);
            // Reset loading from save, this is only supposed to trigger once when loading from an existing game.
            loadingFromSave = false;
        }
        // else if (LastExitTriggerID != null)
        // {
        //     // If there is a last trigger, spawn them in the new matching scene's position.
        //     MoveToScene[] moveToSceneObjs = GameObject.FindObjectsOfType<MoveToScene>();
        //     bool foundMatchingExit = false;
        //     foreach (MoveToScene moveToSceneObj in moveToSceneObjs)
        //     {
        //         // If a movetosceneObj matches the ID of the triggered one, instantiate the player there.
        //         if (moveToSceneObj.ExitTriggerID == LastExitTriggerID)
        //         {
        //             if (player == null)
        //             {
        //                 spawnPointToUse = moveToSceneObj.playerSpawnPoint;
        //                 player = Instantiate(playerPrefab, spawnPointToUse.position, spawnPointToUse.rotation);
        //                 foundMatchingExit = true;
        //                 break;
        //             }
        //             else
        //             {
        //                 Debug.LogWarning("Player already exists.");
        //                 foundMatchingExit = true;
        //                 break;
        //             }
        //         }
        //     }
        //     if (!foundMatchingExit)
        //     {
        //         // Debug.LogWarning("No trigger with matching ID found. Spawning player in default spawn position.");
        //         if (player == null)
        //         {
        //             spawnPointToUse = sceneDefaultPlayerSpawn;
        //             player = Instantiate(playerPrefab, spawnPointToUse.position, spawnPointToUse.rotation);
        //         }
        //     }
        // }
        else
        {
            // If there was no last trigger, i.e. playing from a scene, spawn the player in the default area.
            if (player == null)
            {
                spawnPointToUse = sceneDefaultPlayerSpawn;
                player = Instantiate(playerPrefab, spawnPointToUse.position, spawnPointToUse.rotation);
                // player = Instantiate(playerPrefab, sceneDefaultPlayerSpawn.position, sceneDefaultPlayerSpawn.rotation);
            }
        }

        // `rotation` is a Quaternion; its components are NOT Euler degrees. Use `eulerAngles.y`
        // and compare using DeltaAngle to tolerate floating-point and wrap-around differences.
        float yAngle = spawnPointToUse.eulerAngles.y;
        if (Mathf.Abs(Mathf.DeltaAngle(yAngle, 90f)) < 1f)
        {
            InputManager.GetInstance().SwitchToPlayerMap("PlayerControlsY90");
        }
        else if (Mathf.Abs(Mathf.DeltaAngle(yAngle, -90f)) < 1f)
        {
            InputManager.GetInstance().SwitchToPlayerMap("PlayerControls");
        }
        else if (Mathf.Abs(Mathf.DeltaAngle(yAngle, 0f)) < 1f)
        {
            InputManager.GetInstance().SwitchToPlayerMap("PlayerControlsAxisConfig");
        }
        else
        {
            Debug.LogWarning("Error when reading current spawn point to use's Y rotation.");
            InputManager.GetInstance().SwitchToPlayerMap("PlayerControlsAxisConfig");
        }
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
