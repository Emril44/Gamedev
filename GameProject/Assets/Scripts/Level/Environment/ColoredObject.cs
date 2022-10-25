using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class ColoredObject : MonoBehaviour
{
    [SerializeField] private PrismColor color;

    public PrismColor getColor()
    {
        return color;
    }
}
