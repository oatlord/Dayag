using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenePersistenceManager : MonoBehaviour
{
    // This manager is for persisting player positions across scenes, recording which exit they took to get to the next scene, and any other scene-specific data that needs to be saved and loaded when transitioning between scenes. 
    // It will work alongside the GameManager and GameSceneManager to ensure that all necessary data is properly saved and loaded during scene transitions.
    public static ScenePersistenceManager instance;
    // Obtain this every scene change. Load the player here by DEFAULT.
    [SerializeField] private Transform sceneDefaultPlayerSpawn;
    // Private reference to the spawn point that is to be used. Referenced for getting its current rotation and setting input maps.
    private Transform spawnPointToUse;
    // ID of the exit trigger last interacted with.
    public string LastExitTriggerID;
    public GameObject player;
    [SerializeField] private GameObject playerPrefab;

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
        playerPrefab = Resources.Load<GameObject>("Player/Player New");

        // if (player == null)
        // {
        //     player = Instantiate(playerPrefab, sceneDefaultPlayerSpawn.position, sceneDefaultPlayerSpawn.rotation);
        // }

        if (LastExitTriggerID != null)
        {
            // If there is a last trigger, spawn them in the new matching scene's position.
            MoveToScene[] moveToSceneObjs = GameObject.FindObjectsOfType<MoveToScene>();
            bool foundMatchingExit = false;
            foreach (MoveToScene moveToSceneObj in moveToSceneObjs)
            {
                // If a movetosceneObj matches the ID of the triggered one, instantiate the player there.
                if (moveToSceneObj.ExitTriggerID == LastExitTriggerID)
                {
                    if (player == null)
                    {
                        spawnPointToUse = moveToSceneObj.playerSpawnPoint;
                        player = Instantiate(playerPrefab, spawnPointToUse.position, spawnPointToUse.rotation);
                        foundMatchingExit = true;
                        break;
                    }
                    else
                    {
                        Debug.LogWarning("Player already exists.");
                        foundMatchingExit = true;
                        break;
                    }
                }
            }
            if (!foundMatchingExit)
            {
                Debug.LogWarning("No trigger with matching ID found. Spawning player in default spawn position.");
                if (player == null)
                {
                    spawnPointToUse = sceneDefaultPlayerSpawn;
                    player = Instantiate(playerPrefab, spawnPointToUse.position, spawnPointToUse.rotation);
                }
            }
        }
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
