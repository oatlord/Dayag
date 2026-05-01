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
        // DisableMenuButtons();
        // Debug.Log("New Game Clicked");
        // DataPersistenceManager.instance.NewGame();
        // SceneManager.LoadSceneAsync("SampleScene");
    }

    public void OnLoadGameClicked() {
        saveSlotsMenu.ActivateMenu(true);
        this.DeactivateMenu();
    }

    public void OnContinueGameClicked() {
        DisableMenuButtons();
        Debug.Log("Continue Game Clicked");
        SceneManager.LoadSceneAsync("SampleScene"); 
    }

    private void DisableMenuButtons() {
        newGameButton.interactable = false;
        loadGameButton.interactable = false;
        // continueGameButton.interactable = false;
    }

    public void ActivateMenu() {
        this.gameObject.SetActive(true);
    }

    public void DeactivateMenu() {
        this.gameObject.SetActive(false);
    }
}
