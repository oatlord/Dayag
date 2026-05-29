using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class GameData
{
    public long lastUpdated;
    public Vector3 playerPosition;
    public Vector3 playerCheckpointPosition;
    public bool hasPlayerCheckpoint;
    public string currentZone;

    // STORY DIALOGUE FLAGS
    // public bool HasHelped;
    // public bool HasLetter;
    // public string NameOfChoice;

    // DIALOGUE TEST SAVE
    // public 
    public string savedStoryJson;

    public GameData() {
        playerPosition = Vector3.zero;
        playerCheckpointPosition = Vector3.zero;
        hasPlayerCheckpoint = false;
        currentZone = "Test";

        // HasHelped = false;
        // HasLetter = false;
        // NameOfChoice = "";

        savedStoryJson = "";
    }
}
