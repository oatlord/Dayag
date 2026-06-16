using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SaveSlot : MonoBehaviour
{
    [Header("Profile")]
    [SerializeField] private string profileId = "";

    [Header("Content")]
    [SerializeField] private GameObject noDataContent;
    [SerializeField] private GameObject hasDataContent;
    [SerializeField] private TextMeshProUGUI zoneText;

    [Header("Display Options")]
    [SerializeField] private Sprite yourHomeImage;
    [SerializeField] private Sprite zone1Image;
    [SerializeField] private Sprite zone2Image;
    [SerializeField] private Sprite zone3Image;
    [SerializeField] private Sprite zone4Image;
    [SerializeField] private Sprite zone5Image;
 
    private Button saveSlotButton;
    private Image saveSlotImage;
    private string toLoadSceneName;

    private void Awake() {
        saveSlotButton = this.GetComponent<Button>();
        saveSlotImage = this.GetComponent<Image>();
    }

    public void SetData(GameData data) {
        if (data == null) {
            noDataContent.SetActive(true);
            hasDataContent.SetActive(false);
        } else {
            noDataContent.SetActive(false);
            hasDataContent.SetActive(true);

            zoneText.text = data.currentZone;
            toLoadSceneName = data.currentSceneName;
            SetDisplayImage(toLoadSceneName);
        }
    } 

    public string GetSceneToLoad() {
        return toLoadSceneName;
    }

    public string GetProfileId() {
        return this.profileId;
    }

    public void SetInteractable(bool interactable) {
        saveSlotButton.interactable = interactable;
    }

    // FUNCTIONS FOR SETTING THE IMAGES WITHIN THE SAVE SLOTS DEPENDENT ON ZONE 
    private void SetDisplayImage(string zoneName)
    {
        switch (zoneName)
        {
            case "Zone 1":
                saveSlotImage.sprite = zone1Image;
                break;
            case "Zone 2":
                saveSlotImage.sprite = zone2Image;
                break;
            case "Zone 3":
                saveSlotImage.sprite = zone3Image;
                break;
            case "Zone 4":
                saveSlotImage.sprite = zone4Image;
                break;
            case "Zone 5":
                saveSlotImage.sprite = zone5Image;
                break;
            case "House":
                saveSlotImage.sprite = yourHomeImage;
                break;
            default:
                Debug.LogWarning("Unrecognizable scene name. Setting to default.");
                saveSlotImage.sprite = yourHomeImage;
                break;
        }
    }
}
