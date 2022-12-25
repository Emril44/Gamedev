using System;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SavesManager : MonoBehaviour
{
    [SerializeField] private bool forceLoadNewGame; // only for debug, specifically when starting game in GameScene
    public static SavesManager Instance { get; private set; }
    private SaveHeader[] saveHeaders = new SaveHeader[4]; // lightweight UI-targeted containers with core information about saves
    private bool switchRunning = false;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        for (int i = 0; i < saveHeaders.Length; i++)
        {
            string savePath = Application.persistentDataPath + "/Saves/" + (i == 0 ? "Autosave" : "Save " + i);
            DataManagerSerializedData data = LoadDataManager(savePath);
            saveHeaders[i] = data == null ? null : new SaveHeader(data);
        }
    }

    private void Start()
    {
        if (forceLoadNewGame && SceneManager.GetActiveScene().name.Equals("GameScene"))
        {
            DataManager.Instance.Deserialize(null);
            EnvironmentManager.Instance.Deserialize(null);
            QuestManager.Instance.Deserialize(null);
        }
    }

    public bool HasSave(int index)
    {
        return saveHeaders[index] != null;
    }

    public bool HasSaves()
    {
        for (int i = 0; i < saveHeaders.Length; i++)
        {
            if (HasSave(i)) return true;
        }
        return false;
    }
    
    public void Autosave()
    {
        Save(0);
    }

    public void Save(int saveIndex)
    {
        string savePath = Application.persistentDataPath + "/Saves/" + (saveIndex == 0 ? "Autosave" : "Save " + saveIndex);
        if (!HasSave(saveIndex)) Directory.CreateDirectory(savePath);
        saveHeaders[saveIndex] = new SaveHeader(SaveDataManager(savePath)); // simultenously write into file and update the header
        SaveEnvironmentManager(savePath);
        SaveQuestManager(savePath);
    }
    
    public void LoadNewGame()
    {
        if (!switchRunning) StartCoroutine(SwitchToSceneAndDo("GameScene", () =>
        {
            DataManager.Instance.Deserialize(null);
            EnvironmentManager.Instance.Deserialize(null);
            QuestManager.Instance.Deserialize(null);
        }));
    }

    public void Load(int saveIndex)
    {
        string savePath = Application.persistentDataPath + "/Saves/" + (saveIndex == 0 ? "Autosave" : "Save " + saveIndex);
        if (!HasSave(saveIndex))
        {
            Debug.LogError("Unable to load save " + saveIndex + ", save will not be loaded");
            return;
        }
        DataManagerSerializedData dataManager = LoadDataManager(savePath);
        EnvironmentManagerSerializedData environmentManager = LoadEnvironmentManager(savePath);
        QuestManagerSerializedData questManager = LoadQuestManager(savePath);
        if (dataManager == null || environmentManager == null || questManager == null)
        {
            Debug.LogError("Corrupted save " + saveIndex + ", save will not be loaded");
            Directory.Delete(savePath);
            return;
        }
        saveHeaders[saveIndex] = new SaveHeader(dataManager); // regenerate header just in case
        if (!switchRunning) StartCoroutine(SwitchToSceneAndDo("GameScene", () =>
        {
            DataManager.Instance.Deserialize(dataManager);
            EnvironmentManager.Instance.Deserialize(environmentManager);
            QuestManager.Instance.Deserialize(questManager);
        }));
    }

    private DataManagerSerializedData SaveDataManager(string savePath)
    {
        DataManagerSerializedData data = DataManager.Instance.Serialize();
        string path = savePath + "/_1.prism";
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);
        formatter.Serialize(stream, data);
        stream.Close();
        return data;
    }

    private EnvironmentManagerSerializedData SaveEnvironmentManager(string savePath)
    {
        EnvironmentManagerSerializedData data = EnvironmentManager.Instance.Serialize();
        string path = savePath + "/_2.prism";
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);
        formatter.Serialize(stream, data);
        stream.Close();
        return data;
    }

    private QuestManagerSerializedData SaveQuestManager(string savePath)
    {
        QuestManagerSerializedData data = QuestManager.Instance.Serialize();
        string path = savePath + "/_3.prism";
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);
        formatter.Serialize(stream, data);
        stream.Close();
        return data;
    }

    private DataManagerSerializedData LoadDataManager(string savePath)
    {
        string path = savePath + "/_1.prism";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            DataManagerSerializedData data = formatter.Deserialize(stream) as DataManagerSerializedData;
            stream.Close();
            return data;
        }
        else
        {
            return null;
        }
    }
    
    private EnvironmentManagerSerializedData LoadEnvironmentManager(string savePath)
    {
        string path = savePath + "/_2.prism";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            EnvironmentManagerSerializedData data = formatter.Deserialize(stream) as EnvironmentManagerSerializedData;
            stream.Close();
            return data;
        }
        else
        {
            return null;
        }
    }

    private QuestManagerSerializedData LoadQuestManager(string savePath)
    {
        string path = savePath + "/_3.prism";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            QuestManagerSerializedData data = formatter.Deserialize(stream) as QuestManagerSerializedData;
            stream.Close();
            return data;
        }
        else
        {
            return null;
        }
    }

    public SaveHeader[] SaveHeaders()
    {
        SaveHeader[] saveHeaders = new SaveHeader[4];
        this.saveHeaders.CopyTo(saveHeaders, 0);
        return saveHeaders;
    }

    public void RemoveSave(int saveIndex)
    {
        string savePath = Application.persistentDataPath + "/Saves/" + (saveIndex == 0 ? "Autosave" : "Save " + saveIndex);
        for (int i = 1; i <= 3; i++)
        {
            string path = savePath + "/_" + i + ".prism";
            if (File.Exists(path)) File.Delete(path);
        }
        Directory.Delete(savePath);
        saveHeaders[saveIndex] = null;
    }

    private IEnumerator SwitchToSceneAndDo(string scene, Action action)
    {
        Time.timeScale = 1;
        switchRunning = true;
        ScreenFade.Instance.SetOverUI(true);
        yield return ScreenFade.Instance.FadeOut(2);
        bool loaded = false;
        void ProcessLoad(Scene s, LoadSceneMode m)
        {
            loaded = true;
            SceneManager.sceneLoaded -= ProcessLoad;
        }
        SceneManager.sceneLoaded += ProcessLoad;
        SceneManager.LoadSceneAsync(scene);
        yield return new WaitUntil(() => loaded);
        action();
        yield return ScreenFade.Instance.FadeIn(2);
        ScreenFade.Instance.SetOverUI(false);
        switchRunning = false;
    }

    public void ExitToMenu()
    {
        if (!switchRunning) StartCoroutine(SwitchToSceneAndDo("MainMenu", () => { }));
    }
}
