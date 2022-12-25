using UnityEngine;
using UnityEngine.U2D;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class ColoredObject : MonoBehaviour
{
    [SerializeField] private PrismColor color;
    private bool isDynamic = false;
    private Rigidbody2D rb;
    private Color actualColor;
    private static Color locked = new(0.2264151f, 0.2187255f, 0.2187255f, 1);

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb.bodyType == RigidbodyType2D.Dynamic)
        {
            isDynamic = true;
        }
        actualColor = GetComponent<SpriteShapeRenderer>().color;
    }

    public void SetColored(bool colored)
    {
        if (colored) GetComponent<SpriteShapeRenderer>().color = actualColor;
        else GetComponent<SpriteShapeRenderer>().color = locked;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isDynamic && other.CompareTag("Water"))
        {
            rb.gravityScale = 0.1f;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (isDynamic && other.CompareTag("Water"))
        {
            rb.gravityScale = 1f;
        }
    }

    public PrismColor getColor()
    {
        return color;
    }
}
