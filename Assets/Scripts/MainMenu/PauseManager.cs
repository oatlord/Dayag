using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject pausePanel;
    public CanvasGroup pauseCanvasGroup;
    public GameObject optionsPanel;
    public OptionManager optionManager;

    private bool isPaused = false;
    private string mainMenuSceneName = "MainMenu";

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (pausePanel != null)
            pausePanel.SetActive(true);

        if (pauseCanvasGroup == null && pausePanel != null)
            pauseCanvasGroup = pausePanel.GetComponent<CanvasGroup>();

        SetCanvasGroupVisibility(pauseCanvasGroup, false);

        if (optionsPanel != null)
            optionsPanel.SetActive(true);

        if (optionManager == null && optionsPanel != null)
            optionManager = optionsPanel.GetComponent<OptionManager>();
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
        if (isPaused && optionManager != null && optionManager.IsOpenedFromPause())
        {
            optionManager.CloseOptionsPanel();
            return;
        }

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
        SetCanvasGroupVisibility(pauseCanvasGroup, true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        SetCanvasGroupVisibility(pauseCanvasGroup, false);
        isPaused = false;
        
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneName);
        pausePanel.SetActive(false);
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
        if (optionManager != null)
        {
            HidePausePanel();
            optionManager.OpenFromPause();
            return;
        }

        Debug.Log("Options opened");
    }
    
    public void OnSaveButton()
    {
        Debug.Log("Save menu opened");
    }
    
    public void OnReturnToMenuButton() => ReturnToMainMenu();

    public void ShowPausePanel()
    {
        if (pauseCanvasGroup != null)
        {
            SetCanvasGroupVisibility(pauseCanvasGroup, true);
            return;
        }

        if (pausePanel != null)
            pausePanel.SetActive(true);
    }

    public void HidePausePanel()
    {
        if (pauseCanvasGroup != null)
        {
            SetCanvasGroupVisibility(pauseCanvasGroup, false);
            return;
        }

        if (pausePanel != null)
            pausePanel.SetActive(false);
    }

    private void SetCanvasGroupVisibility(CanvasGroup group, bool visible)
    {
        if (group == null) return;
        group.alpha = visible ? 1f : 0f;
        group.interactable = visible;
        group.blocksRaycasts = visible;
    }
}
