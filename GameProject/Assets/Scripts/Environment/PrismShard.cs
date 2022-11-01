using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PrismShard : MonoBehaviour
{
    [SerializeField] private PrismColor color;

    public PrismColor getColor()
    {
        return color;
    }
}
