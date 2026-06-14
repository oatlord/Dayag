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

    [Header("Menu Graphics")]
    [SerializeField] private GameObject dayagTitle;
    [SerializeField] private GameObject dayagBaybayin;

    [Header("Menu Panels")]
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private GameObject creditsPanel;

    private void Start() {
        if (!DataPersistenceManager.instance.HasGameData()) {
            loadGameButton.interactable = false;
            // continueGameButton.interactable = false;
        } else
        {
            loadGameButton.interactable = true;
        }
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
        this.gameObject.SetActive(true);
    }

    public void DeactivateMenu() {
        this.gameObject.SetActive(false);
    }

    public void OnOptionsClicked() {
        if (optionsPanel != null) {
            optionsPanel.SetActive(true);
        }

        if (creditsPanel != null) {
            creditsPanel.SetActive(false);
        }
    }

    public void OnCreditsClicked() {
        if (creditsPanel != null) {
            creditsPanel.SetActive(true);
        }

        if (optionsPanel != null) {
            optionsPanel.SetActive(false);
        }
    }

    public void CloseSubMenuPanels() {
        if (optionsPanel != null) {
            optionsPanel.SetActive(false);
        }

        if (creditsPanel != null) {
            creditsPanel.SetActive(false);
        }
    }
}
