using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;

    [SerializeField] private string fileName;
    [SerializeField] private bool encryptData;

    private GameData gameData;
    private List<ISaveManager> saveManagers;
    private FileDataHandler dataHandler;

    [ContextMenu("Delete Saved File")]

    public void DeleteSavedData()
    {
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName,encryptData);
        dataHandler.Delete();

    }

    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;

    }

    private void Start()
    {
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName,encryptData);

        saveManagers = FindAllISaveManager();

        LoadGame();
    }

    public void NewGame()
    {
        gameData = new GameData();
    }

    public void LoadGame()
    {
        gameData = dataHandler.Load();

        if (this.gameData == null)
        {
            NewGame();
        }

        foreach (ISaveManager saveManager in saveManagers)
        {
            saveManager.LoadData(gameData);
        }
    }

    public void SaveGame()
    {
        foreach (ISaveManager saveManager in saveManagers)
        {
            saveManager.SaveData(ref gameData);
        }

        dataHandler.Save(gameData);
    }

    private void OnApplicationQuit()
    {
        SaveGame(); 
    }

    private List<ISaveManager> FindAllISaveManager()
    {
        // true is added to ensure skill tree is saved
        IEnumerable<ISaveManager> saveManagers = FindObjectsOfType<MonoBehaviour>(true).OfType<ISaveManager>();

        return new List<ISaveManager>(saveManagers);
    }

    public bool HasSavedData()
    {
        if (dataHandler.Load() != null)
            return true;

        return false;
    }
}
