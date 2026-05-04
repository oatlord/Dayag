using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    public static GameSceneManager instance;

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
        SceneManager.LoadScene(sceneToMoveTo);
    }
}
