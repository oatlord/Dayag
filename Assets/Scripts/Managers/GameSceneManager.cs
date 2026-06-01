using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    public static GameSceneManager instance;
    [SerializeField] private GameObject loadingScreen;
    public bool SceneIsLoading { get; private set; } = false;

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
