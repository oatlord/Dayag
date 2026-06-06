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
            foreach (MoveToScene moveToSceneObj in moveToSceneObjs)
            {
                // If a movetosceneObj matches the ID of the triggered one, instantiate the player there.
                if (moveToSceneObj.ExitTriggerID == LastExitTriggerID)
                {
                    if (player == null)
                    {
                        player = Instantiate(playerPrefab, moveToSceneObj.playerSpawnPoint.position, moveToSceneObj.playerSpawnPoint.rotation);
                        break;
                    }
                    else
                    {
                        Debug.LogWarning("Player already exists.");
                        break;
                    }
                }
            }
            Debug.LogWarning("No trigger with matching ID found. Spawning player in default spawn position.");
            if (player == null)
            {
                player = Instantiate(playerPrefab, sceneDefaultPlayerSpawn.position, sceneDefaultPlayerSpawn.rotation);
            }
        }
        else
        {
            // If there was no last trigger, i.e. playing from a scene, spawn the player in the default area.
            if (player == null)
            {
                player = Instantiate(playerPrefab, sceneDefaultPlayerSpawn.position, sceneDefaultPlayerSpawn.rotation);
            }
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
