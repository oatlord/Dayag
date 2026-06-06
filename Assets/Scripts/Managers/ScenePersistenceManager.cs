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

        if (player == null)
        {
            player = Instantiate(playerPrefab, sceneDefaultPlayerSpawn.position, sceneDefaultPlayerSpawn.rotation);
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
