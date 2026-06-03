using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour, IDataPersistence
{
    public static GameSceneManager instance;
    [SerializeField] private GameObject loadingScreen;
    public bool SceneIsLoading { get; private set; } = false;

    public void SaveData(GameData data)
    {
        data.currentSceneName = SceneManager.GetActiveScene().name;
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
        }
    }

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
