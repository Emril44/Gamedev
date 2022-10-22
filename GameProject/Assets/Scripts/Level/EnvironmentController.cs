using System.Collections.Generic;
using UnityEngine;

public class EnvironmentController : MonoBehaviour
{
    private Queue<ColoredObstacle> obstaclesPool;

    public static EnvironmentController Instance { get; private set; }
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

    private void ChangeBackground(GameManager.KeyColor color)
    {
        //...
    }

    public void SetNewColor(GameManager.KeyColor color)
    {
        ChangeBackground(color);
        RepaintPool(color);
    }

    void FillPool()
    {
        /*
        obstaclesPool = new Queue<ColoredObstacle>();
        foreach (ColoredObstacle obstacle in FindObjectsOfType<ColoredObstacle>())
        {
            obstaclesPool.Enqueue(obstacle);
        }
        */
    }

    private void RepaintPool(GameManager.KeyColor color)
    {
        for (int i = 0; i < obstaclesPool.Count; i++)
        {
            var obstacle = obstaclesPool.Dequeue();
            if (obstacle.getColor() == color)
            {
                obstacle.gameObject.SetActive(false);
            }
            else
            {
                obstacle.gameObject.SetActive(true);
            }
            obstaclesPool.Enqueue(obstacle);
        }
        
    }
}
