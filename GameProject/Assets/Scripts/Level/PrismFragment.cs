using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PrismFragment : MonoBehaviour
{
    [SerializeField] private GameManager.KeyColor keyColor;

    public GameManager.KeyColor getColor()
    {
        return keyColor;
    }
}
