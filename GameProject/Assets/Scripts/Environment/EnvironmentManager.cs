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
        FillPool();
    }

    private void ChangeBackground(PrismColor color)
    {
        //...
    }

    public void SetNewColor(PrismColor color)
    {
        ChangeBackground(color);
        RepaintPool(color);
    }

    void FillPool()
    {
        coloredObjectsPool = new Queue<ColoredObject>();
        foreach (ColoredObject obj in FindObjectsOfType<ColoredObject>())
        {
            coloredObjectsPool.Enqueue(obj);
        }
    }

    private void RepaintPool(PrismColor color)
    {
        //TODO: if grabbable
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
