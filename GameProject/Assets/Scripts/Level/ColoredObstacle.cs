using UnityEngine;

public class ColoredObstacle : MonoBehaviour
{
    [SerializeField] private GameManager.KeyColor keyColor;

    public GameManager.KeyColor getColor()
    {
        return keyColor;
    }
}
