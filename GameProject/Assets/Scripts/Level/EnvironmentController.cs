using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentController : MonoBehaviour
{
    private Queue<ColoredObstacle> obstaclesPool;
    
    [SerializeField] private Background background;

    private void ChangeBackground(GameManager.KeyColor color)
    {
        background.SetColor(color);
    }

    public void SetNewColor(GameManager.KeyColor color)
    {
        ChangeBackground(color);
        RepaintPool(color);
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
