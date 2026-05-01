using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class GameData
{
    public long lastUpdated;
    public Vector3 playerPosition;
    public string currentZone;

    public GameData() {
        playerPosition = Vector3.zero;
        currentZone = "Test";
    }
}
