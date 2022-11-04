using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 14f;
    [SerializeField] private float jumpVelocity = 14f;
    [SerializeField] private float outOfWaterMultiplier = 1.6f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Collider2D feetCollider;
    private Rigidbody2D rb;
    private bool inWater = false;
    private float environmentSpeed = 1f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void SetInWater(bool inWater)
    {
        this.inWater = inWater;
        if (!inWater)
        {
            environmentSpeed = 1f;
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * outOfWaterMultiplier);
        }
        else
        {
            environmentSpeed = 0.4f;
        }
    }

    void FixedUpdate()
    {
        if (inWater)
        {
            float horizontalMove = Input.GetAxis("Horizontal");
            float verticalMove = Input.GetAxis("Vertical");
            if (Input.GetAxis("Jump") > 0 && feetCollider.IsTouchingLayers(groundLayer) && verticalMove == 0)
            {
                verticalMove += jumpVelocity * environmentSpeed;
            }
            rb.velocity = new Vector2(horizontalMove * movementSpeed, verticalMove * movementSpeed);
            rb.velocity *= environmentSpeed;
        }
        else
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
}
