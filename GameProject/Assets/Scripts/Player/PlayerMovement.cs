using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class PlayerMovement : MonoBehaviour
{
    public bool Controllable;
    [SerializeField] private float movementSpeed = 14f;
    [SerializeField] private float jumpVelocity = 14f;
    [SerializeField] private float outOfWaterMultiplier = 1.6f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Collider2D feetCollider;
    private Rigidbody2D rb;
    private bool inWater = false;
    private float environmentSpeed = 1f;
    private Quaternion rotationGoal;

    private Animator animator;
    private const string JUMP_NAME = "Player_Jump";

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        Controllable = true;

        animator = GetComponent<Animator>();
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

    private bool CanJump()
    {
        var colliders = new List<Collider2D>();
        var filter = new ContactFilter2D
        {
            useTriggers = false,
            layerMask = groundLayer,
        };
        feetCollider.OverlapCollider(filter, colliders);
        return colliders.Count > 1;
    }

    void FixedUpdate()
    {
        if (Controllable)
        {
            if (inWater)
            {
                float horizontalMove = Input.GetAxis("Horizontal");
                float verticalMove = Input.GetAxis("Vertical");
                rb.velocity = new Vector2(horizontalMove * movementSpeed, verticalMove * movementSpeed);
                rb.velocity *= environmentSpeed;
            }
            else
            {
                float horizontalMove = Input.GetAxis("Horizontal");
                float verticalMove = rb.velocity.y;
                if (Input.GetAxis("Jump") > 0 && CanJump())
                {
                    verticalMove = jumpVelocity;
                    animator.Play(JUMP_NAME);
                }
                rb.velocity = new Vector2(horizontalMove * movementSpeed, verticalMove);
            }
        }
        float offset = 0.45f;
        float rayLength = 1.25f;
        RaycastHit2D left = Physics2D.Raycast(transform.position - new Vector3(offset, 0), -Vector2.up, rayLength, groundLayer);
        RaycastHit2D right = Physics2D.Raycast(transform.position + new Vector3(offset, 0), -Vector2.up, rayLength, groundLayer);
        //Debug.DrawRay(transform.position - new Vector3(offset, 0), -Vector2.up * rayLength, Color.red);
        //Debug.DrawRay(transform.position + new Vector3(offset, 0), -Vector2.up * rayLength, Color.red);
        if (left.collider != null && right.collider != null)
        {
            if (left.normal == right.normal)
            {
                rotationGoal = Quaternion.Euler(0, 0, Mathf.Atan2(left.normal.y, left.normal.x) * Mathf.Rad2Deg - 90);
            }
            else
            {
                rotationGoal = Quaternion.Euler(0, 0, 0);
            }
        }
        else
        {
            rotationGoal = Quaternion.Euler(0, 0, 0);
        }
        transform.rotation = Quaternion.Lerp(transform.rotation, rotationGoal, 0.3f);
    }
}
