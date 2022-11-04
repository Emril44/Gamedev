using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class ColoredObject : MonoBehaviour
{
    [SerializeField] private PrismColor color;
    [SerializeField] private bool floatable = false;
    private bool isDynamic = false;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb.bodyType == RigidbodyType2D.Dynamic)
        {
            isDynamic = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isDynamic && other.CompareTag("Water"))
        {
            if (floatable)
            {
                rb.gravityScale = -0.3f;
            }
            else
            {
                rb.gravityScale = 0f;
            }
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
