using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    private Queue<ColoredObject> coloredObjectsPool;

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
        FillPool();
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

    void FillPool()
    {
        /*
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
        */
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
