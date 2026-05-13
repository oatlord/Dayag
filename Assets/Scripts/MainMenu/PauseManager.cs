using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject pausePanel;
    public GameObject optionsPanel;

    private bool isPaused = false;
    private string mainMenuSceneName = "MainMenu";

    private void Awake()
    {
        if (pausePanel != null)
            pausePanel.SetActive(false);
        
        if (optionsPanel != null)
            optionsPanel.SetActive(false);
    }

    private void Update()
    {
        if (IsGameplayScene() && Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            PauseGame();
        }
        else
        {
            ResumeGame();
        }
    }

    private void PauseGame()
    {
        Time.timeScale = 0f;
        pausePanel.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
        isPaused = false;
        
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneName);
    }

    private bool IsGameplayScene()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        return currentScene != mainMenuSceneName && 
               !currentScene.Contains("Menu") && 
               !currentScene.Contains("Loading");
    }

    public void OnResumeButton() => ResumeGame();
    public void OnOptionsButton() 
    {
        Debug.Log("Options opened");
    }
    
    public void OnSaveButton()
    {
        Debug.Log("Save menu opened");
    }
    
    public void OnReturnToMenuButton() => ReturnToMainMenu();
}