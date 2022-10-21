using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnvironmentController : MonoBehaviour
{
    private ObjectPool<ColoredObstacle> pool;
    
    [SerializeField] private Background background;

    private void ChangeBackground(GameManager.KeyColor color)
    {
        background.SetColor(color);
    }

    public void SetNewColor(PrismPiece prismPiece)
    {
        ChangeBackground(prismPiece.getColor);
        RepaintPool(prismPiece.getColor);
    }
    
    private void RepaintPool(GameManager.KeyColor color)
    {
        for (int i = 0; i < pool.CountAll; i++)
        {
            pool.Get(out var obstacle);
            if (obstacle.getColor == color)
            {
                obstacle.gameObject.SetActive(true);
            }
            else
            {
                obstacle.gameObject.SetActive(false);
            }
        }
    }
}
