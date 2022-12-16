using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    private Queue<ColoredObject> coloredObjectsPool;
    [Header("Spark-giving objects")]
    [SerializeField] private List<GameObject> sparks; // listed separately from generic objects simply for editing convenience
    [Header("Other dynamic objects")]
    [SerializeField] private List<GameObject> objects;
    [Header("NPCs")]
    [SerializeField] private List<NPC> npcs; // objects should have the NPC (or a derived) component

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
        return new EnvironmentManagerSerializedData(activeSparks, activeObjects, npcData);
    }

    public void Deserialize(EnvironmentManagerSerializedData data)
    {
        foreach (GameObject spark in sparks) spark.SetActive(false);
        foreach (int spark in data.activeSparks) sparks[spark].SetActive(true);
        foreach (GameObject go in objects) go.SetActive(false);
        foreach (int go in data.activeObjects) objects[go].SetActive(true);
        for (int i = 0; i < npcs.Count; i++) npcs[i].Deserialize(data.npcData[i]);

    }

    private void ChangeBackground(PrismColor color)
    {
        //TODO:...
    }

    public void SetNewColor(PrismColor color)
    {
        if (DataManager.Instance.unlockedColors >= (int)color)
        {
            ChangeBackground(color);
            RepaintPool(color);
        }
        else
        {
            Debug.LogWarning("The prism color (" + (int)color + ") is locked; max unlocked = " + DataManager.Instance.unlockedColors);
        }
    }

    void ActivateColoredObjects()
    {
        coloredObjectsPool = new Queue<ColoredObject>();
        foreach (ColoredObject obj in FindObjectsOfType<ColoredObject>())
        {
            if (DataManager.Instance.unlockedColors >= (int)obj.getColor())
            {
                coloredObjectsPool.Enqueue(obj);
            }
            else
            {
                obj.GetComponent<SpriteRenderer>().color = Color.gray;
                //TODO: disable?
            }
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
        var obj = PlayerInteraction.Instance.GetGrabbed();
        if (obj != null && obj.GetComponent<ColoredObject>().getColor() == color)
        {
            obj.SetActive(true);
            StartCoroutine(DropGrabbed(obj));
        }
    }

    IEnumerator DropGrabbed(GameObject obj)
    {
        PlayerInteraction.Instance.Drop();
        yield return new WaitForSeconds(0.5f);
        obj.SetActive(false);
    }
}
