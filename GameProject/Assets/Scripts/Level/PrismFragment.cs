using UnityEngine;

public class PrismFragment : MonoBehaviour
{
    [SerializeField] private GameManager.KeyColor keyColor;

    public GameManager.KeyColor getColor()
    {
        return keyColor;
    }
}
