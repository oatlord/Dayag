using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : Menu
{
    [Header("Menu Navigation")]
    [SerializeField] private SaveSlotsMenu saveSlotsMenu;

    [Header("Menu Buttons")]
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button loadGameButton;
    [SerializeField] private Button quitButton;

    [Header("Menu Graphics")]
    [SerializeField] private GameObject dayagTitle;
    [SerializeField] private GameObject dayagBaybayin;

    [Header("Menu Panels")]
    [SerializeField] private CanvasGroup mainMenuCanvasGroup;
    [SerializeField] private CanvasGroup optionsCanvasGroup;
    [SerializeField] private CanvasGroup creditsCanvasGroup;

    private void Start() {
        if (!DataPersistenceManager.instance.HasGameData()) {
            loadGameButton.interactable = false;
        } else
        {
            loadGameButton.interactable = true;
        }

        InitializeMenuState();
    }

    private void InitializeMenuState()
    {
        SetCanvasGroupVisibility(mainMenuCanvasGroup, true);
        SetCanvasGroupVisibility(optionsCanvasGroup, false);
        SetCanvasGroupVisibility(creditsCanvasGroup, false);
    }

    private void SetCanvasGroupVisibility(CanvasGroup group, bool visible)
    {
        if (group == null) return;
        group.alpha = visible ? 1f : 0f;
        group.interactable = visible;
        group.blocksRaycasts = visible;
    }

    // Start is called before the first frame update
    public void OnNewGameClicked() {
        saveSlotsMenu.ActivateMenu(false);
        this.DeactivateMenu();
        DisableMenuGraphics();
        // Debug.Log("New Game Clicked");
        // DataPersistenceManager.instance.NewGame();
        // SceneManager.LoadSceneAsync("SampleScene");
    }

    public void OnLoadGameClicked() {
        saveSlotsMenu.ActivateMenu(true);
        this.DeactivateMenu();
        DisableMenuGraphics();
    }

    public void OnContinueGameClicked() {
        DisableMenuButtons();
        DisableMenuGraphics();
        Debug.Log("Continue Game Clicked");
        SceneManager.LoadSceneAsync("SampleScene"); 
    }

    public void OnQuitClicked()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void DisableMenuButtons() {
        newGameButton.interactable = false;
        loadGameButton.interactable = false;
        // continueGameButton.interactable = false;
    }

    private void DisableMenuGraphics() {
        if (dayagTitle != null) {
            dayagTitle.SetActive(false);
        }

        if (dayagBaybayin != null) {
            dayagBaybayin.SetActive(false);
        }
    }

    public void EnableMenuGraphics() {
        if (dayagTitle != null) {
            dayagTitle.SetActive(true);
        }

        if (dayagBaybayin != null) {
            dayagBaybayin.SetActive(true);
        }
    }

    public void ActivateMenu() {
        SetCanvasGroupVisibility(mainMenuCanvasGroup, true);
    }

    public void DeactivateMenu() {
        SetCanvasGroupVisibility(mainMenuCanvasGroup, false);
    }

    public void OnOptionsClicked() {
        SetCanvasGroupVisibility(optionsCanvasGroup, true);
        SetCanvasGroupVisibility(creditsCanvasGroup, false);
        DeactivateMenu();
        DisableMenuGraphics();
    }

    public void OnCreditsClicked() {
        SetCanvasGroupVisibility(creditsCanvasGroup, true);
        SetCanvasGroupVisibility(optionsCanvasGroup, false);
        DeactivateMenu();
    }

    public void CloseSubMenuPanels() {
        SetCanvasGroupVisibility(optionsCanvasGroup, false);
        SetCanvasGroupVisibility(creditsCanvasGroup, false);
    }
}
