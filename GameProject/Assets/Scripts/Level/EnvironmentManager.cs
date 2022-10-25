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
        /*
        obstaclesPool = new Queue<ColoredObject>();
        foreach (ColoredObject object in FindObjectsOfType<ColoredObject>())
        {
            coloredObjectsPool.Enqueue(object);
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
        
    }
}
