using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour, IDataPersistence
{
    public static GameSceneManager instance;
    [SerializeField] private GameObject loadingScreen;
    public bool SceneIsLoading { get; private set; } = false;
    // private string lastTrigger;

    public void SaveData(GameData data)
    {
        string activeSceneName = SceneManager.GetActiveScene().name;
        data.currentSceneName = activeSceneName;

        switch (activeSceneName) 
        {
            case "Zone 1":
                data.currentZone = "Wrecked Hometown";
                break;
            case "Zone 2":
                data.currentZone = "Abaca Fields";
                break;
            case "Zone 3":
                data.currentZone = "Kempei Tai Checkpoint";
                break;
            case "Zone 4":
                data.currentZone = "Ruined Plantation";
                break;
            case "Zone 5":
                data.currentZone = "Mintal Base";
                break;
            default:
                Debug.LogWarning("Unreadable scene name.");
                data.currentZone = "Your Home";
                break;
        }
        Debug.Log("Saving current scene name: " + data.currentSceneName);
    }

    public void LoadData(GameData data)
    {
        // Do nothing since we want to load the current scene, not move to a different scene.
    }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one of this instance exists. Destroying clone instance.");
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    private void Start()
    {
        if (loadingScreen == null)

            loadingScreen = GameObject.Find("LoadingScreen");
            // Debug.LogError("Loading screen reference is not set in GameSceneManager. Please set it in the inspector.");              // Debug.LogError("Loading screen reference is not set in GameSceneManager. Please set it in the inspector.");         
        }
        
    

    // void OnEnable()
    // {
    //     SceneManager.sceneLoaded += OnSceneLoaded;
    // }

    // public void OnEnteredExitTrigger(string triggerName, string sceneToMoveTo)
    // {
    //     lastTrigger = triggerName;
    //     Debug.Log("Entered trigger: " + triggerName + ", moving to scene: " + sceneToMoveTo);
    //     MoveToScene(sceneToMoveTo);
    // }

    // void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    // {
    //     GameObject player = GameObject.FindGameObjectWithTag("Player");

    //     // Safety check: player should exist in the newly loaded scene
    //     if (player == null)
    //     {
    //         Debug.LogWarning("Player not found in newly loaded scene. Waiting for player initialization.");
    //         return;
    //     }

    //     MoveToScene[] moveToSceneObjects = FindObjectsOfType<MoveToScene>();
    //     foreach (MoveToScene moveToScene in moveToSceneObjects)
    //     {
    //         if (moveToScene.triggerName == lastTrigger)
    //         {
    //             CharacterController cc = player.GetComponent<CharacterController>();
    //             if (cc != null)
    //             {
    //                 cc.enabled = false;
    //             }
    //             player.transform.position = moveToScene.spawnPoint.position;
    //             if (cc != null)
    //             {
    //                 cc.enabled = true;
    //             }
    //             break;
    //         }
    //     }
    // }

//     void OnSceneLoaded(Scene scene, LoadSceneMode mode)
// {
//     StartCoroutine(PositionPlayerAfterLoad());
// }

// IEnumerator PositionPlayerAfterLoad()
// {
//     // Wait a frame for the new scene's objects to fully initialize
//     yield return null;
    
//     GameObject player = GameObject.FindGameObjec
//     loadingScreen = GameObject.Find("LoadingScreen");tWithTag("Player");
//     if (player == null)
//     {
//         Debug.LogWarning("Player not found in newly loaded scene.");
//         yield break;
//     }

//     MoveToScene[] moveToSceneObjects = FindObjectsOfType<MoveToScene>();
//     foreach (MoveToScene moveToScene in moveToSceneObjects)
//     {
//         if (moveToScene.triggerName == lastTrigger)
//         {
//             CharacterController cc = player.GetComponent<CharacterController>();
//             if (cc != null)
//             {
//                 cc.enabled = false;
//             }
//             player.transform.position = moveToScene.spawnPoint.position;
//             if (cc != null)
//             {
//                 cc.enabled = true;
//             }
//             break;
//         }
//     }
// }

    // Manager for handling moving between scenes in-game.
    public void MoveToScene(string sceneToMoveTo)
    {
        // Open loading screen first, then load the next scene when loading is done.
        loadingScreen.SetActive(true);
        StartCoroutine(LoadSceneAsync(sceneToMoveTo));
    }

    IEnumerator LoadSceneAsync(string sceneToMoveTo)
    {
        SceneIsLoading = true;
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneToMoveTo);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        loadingScreen.SetActive(false);
        SceneIsLoading = false;
    }
}
