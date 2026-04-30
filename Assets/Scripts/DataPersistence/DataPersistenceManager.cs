using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField] private string fileName;
    [SerializeField] private bool useEncryption;

    public static DataPersistenceManager instance { get; private set; }
    private List<IDataPersistence> dataPersistenceObjects;
    private GameData gameData;
    private FileDataHandler dataHandler;

    private void Awake() {
        if (instance != null) {
            Debug.LogError("Found more than one Data Persistence Manager in the scene. Destroying the newest one.");
        }
        instance = this;
    }
    
    private void Start() {
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadGame();
    }

    public void NewGame() {
        this.gameData = new GameData();
    }

    public void LoadGame() {
        // TODO: Load the serialized data from a file using the FileDataHandler
        this.gameData = dataHandler.Load();

        if (this.gameData == null) {
            Debug.Log("No data found. Initializing data to defaults.");
            NewGame();
        }

        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects) {
            dataPersistenceObj.LoadData(gameData);
        }

        // Debug.Log("Player loaded position: " + gameData.playerPosition);


        // TODO: Push the loaded data to other scripts that need to use it
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects() {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();
        return new List<IDataPersistence>(dataPersistenceObjects);
    }

    public void SaveGame() {
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects) {
            dataPersistenceObj.SaveData(gameData);
        }

        // Debug.Log("Saved player position: " + gameData.playerPosition);

        dataHandler.Save(gameData);

    }

    private void OnApplicationQuit() {
        SaveGame();
    }

}
