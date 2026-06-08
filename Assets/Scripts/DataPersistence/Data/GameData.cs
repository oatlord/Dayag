using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]

public class GameData
{
    public long lastUpdated;
    public Vector3 playerPosition;
    public bool hasLastSavedPlayerPosition;
    public Vector3 playerRevivePoint;
    public bool hasPlayerRevivePoint;
    public string currentZone;
    public string currentSceneName;

    // STORY DIALOGUE FLAGS
    // public bool HasHelped;
    // public bool HasLetter;
    // public string NameOfChoice;

    // DIALOGUE TEST SAVE
    // public 
    public string savedStoryJson;

    public GameData() {
        playerPosition = Vector3.zero;
        hasLastSavedPlayerPosition = false;

        playerRevivePoint = Vector3.zero;
        hasPlayerRevivePoint = false;

        currentZone = "Wrecked Hometown";
        currentSceneName = "Zone 1";

        // HasHelped = false;
        // HasLetter = false;
        // NameOfChoice = "";

        savedStoryJson = "";
    }
}
