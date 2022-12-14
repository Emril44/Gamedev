using System;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    public Action onColorChange;
    [NonSerialized] public bool CutsceneRunning;
    private Queue<ColoredObject> coloredObjectsPool;
    [Header("Colored BG Management")]
    [SerializeField] private SpriteRenderer background;
    [SerializeField] private Color[] colors;
    [SerializeField] private GameObject[] cracks;
    [Header("Spark-giving objects")]
    [SerializeField] private List<GameObject> sparks; // listed separately from generic objects simply for editing convenience
    [Header("Other dynamic objects")]
    [SerializeField] private List<GameObject> objects;
    [Header("NPCs")]
    [SerializeField] private List<NPC> npcs; // objects should have the NPC (or a derived) component
    [Header("Letters")]
    [SerializeField] private List<CrackLetter> letters;
    public PrismColor CurrentColor { get; private set; }

    public static EnvironmentManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public int SparksObjectsCount()
    {
        return sparks.Count;
    }

    private void Start()
    {
        ActivateColoredObjects();
    }

    public EnvironmentManagerSerializedData Serialize()
    {
        List<int> activeSparks = new List<int>();
        for (int i = 0; i < sparks.Count; i++) if (sparks[i].activeSelf) activeSparks.Add(i);
        List<int> activeObjects = new List<int>();
        for (int i = 0; i < objects.Count; i++) if (objects[i].activeSelf) activeObjects.Add(i);
        List<NPCSerializedData> npcData = new List<NPCSerializedData>();
        foreach (NPC npc in npcs) npcData.Add(npc.Serialize());
        List<int> letterTouchedLists = new();
        for (int i = 0; i < letters.Count; i++)
        {
            if (letters[i].Touched)
            {
                letterTouchedLists.Add(i);
            }
        }
        return new EnvironmentManagerSerializedData(activeSparks, activeObjects, npcData, letterTouchedLists);
    }

    // if data is null, prepare for new game instead of deserializing
    public void Deserialize(EnvironmentManagerSerializedData data)
    {
        if (data == null) return;
        foreach (GameObject spark in sparks) spark.SetActive(false);
        foreach (int spark in data.activeSparks) sparks[spark].SetActive(true);
        foreach (GameObject go in objects) go.SetActive(false);
        foreach (int go in data.activeObjects) objects[go].SetActive(true);
        for (int i = 0; i < npcs.Count; i++) npcs[i].Deserialize(data.npcData[i]);
        foreach (int id in data.letterTouchedLists)
        {
            letters[id].Touched = true;
        }
        ActivateColoredObjects();
    }

    private void ChangeBackground(PrismColor color)
    {
        background.color = colors[(int)color];
        foreach (GameObject go in cracks)
        {
            go.SetActive(false);
        }
        cracks[(int)color].SetActive(true);
    }

    public void SetNewColor(PrismColor color)
    {
        if (DataManager.Instance.unlockedColors >= (int)color)
        {
            ChangeBackground(color);
            RepaintPool(color);
            CurrentColor = color;
            onColorChange?.Invoke();
        }
        else
        {
            Debug.LogWarning("The prism color (" + (int)color + ") is locked; max unlocked = " + DataManager.Instance.unlockedColors);
        }
    }

    public void ActivateColoredObjects()
    {
        coloredObjectsPool = new Queue<ColoredObject>();
        foreach (ColoredObject obj in FindObjectsOfType<ColoredObject>())
        {
            if (DataManager.Instance.unlockedColors >= (int)obj.getColor())
            {
                coloredObjectsPool.Enqueue(obj);
            }
            obj.SetColored(DataManager.Instance.unlockedColors >= (int)obj.getColor());
        }
    }

    private void RepaintPool(PrismColor color)
    {
        for (int i = 0; i < coloredObjectsPool.Count; i++)
        {
            var obstacle = coloredObjectsPool.Dequeue();
            if (obstacle.getColor() == color)
            {
                obstacle.gameObject.SetActive(false);
            }
            else
            {
                obstacle.gameObject.SetActive(true);
            }
            coloredObjectsPool.Enqueue(obstacle);
        }
    }
}
