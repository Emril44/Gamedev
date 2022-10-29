using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 14f;
    [SerializeField] private float jumpVelocity = 14f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Collider2D feetCollider;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        float horizontalMove = Input.GetAxis("Horizontal");
        float verticalMove = rb.velocity.y;
        if (Input.GetAxis("Jump") > 0 && feetCollider.IsTouchingLayers(groundLayer) && verticalMove == 0)
        {
            verticalMove += jumpVelocity;
        }
        rb.velocity = new Vector2(horizontalMove * movementSpeed, verticalMove);
    }
}
