using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveToScene : MonoBehaviour
{
    [SerializeField] private string sceneName;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (SceneUtility.GetBuildIndexByScenePath("Assets/Scenes/" + sceneName + ".unity") == -1)
            {
                Debug.LogError("Scene " + sceneName + " does not exist. Please check the scene name and try again.");
                return;
            }
            GameSceneManager.instance.MoveToScene(sceneName);
        }
    }
}
