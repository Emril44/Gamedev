using System;

using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class PlayerMovement : MonoBehaviour
{
    private bool controllable;
    public bool Controllable
    { 
        get
        {
            return controllable;
        }
        set
        {
            controllable = value;
            if (!value) rb.velocity = new Vector2(rb.velocity.x / 10, rb.velocity.y);
        }
    }
    [SerializeField] private float movementSpeed = 14f;
    [SerializeField] private float jumpVelocity = 14f;
    [SerializeField] private float outOfWaterMultiplier = 1.6f;
    [SerializeField] private float outOfLavaMultiplier = 1.4f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Collider2D feetCollider;
    [Range(0f, 0.5f)]
    [SerializeField] private float coyoteThreshold = 0.1f; // time for which the player can still jump after leaving solid ground
    private float nonGroundedTime = 1.5f;
    private bool grounded;
    private Transform baseParent; // default parent when player is not moving synchronously with some other object, e.g. a moving platform
    private Rigidbody2D rb;
    private bool inWater = false;
    private bool inLava = false;
    private float environmentSpeed = 1f;
    private Quaternion rotationGoal;

    private Animator animator;
    private const string JUMP_NAME = "Player_Jump";

    public static PlayerMovement Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        rb = GetComponent<Rigidbody2D>();
        Controllable = true;
        baseParent = transform.parent;

        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        grounded = IsGrounded();
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

    public void SetInLava(bool inLava)
    {
        this.inLava = inLava;
        if (!inLava)
        {
            environmentSpeed = 1f;
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * outOfLavaMultiplier);
        }
        else
        {
            environmentSpeed = 0.3f;
        }
    }

    private bool IsGrounded()
    {
        var colliders = new List<Collider2D>();
        var filter = new ContactFilter2D
        {
            useTriggers = false,
            useLayerMask = true,
            layerMask = groundLayer,
        };
        feetCollider.OverlapCollider(filter, colliders);
        return colliders.Count > 0;
    }

    private void UpdateNonGroundedTime()
    {
        if (grounded) nonGroundedTime = 0;
        else nonGroundedTime += Time.fixedDeltaTime;

    }

    void FixedUpdate()
    {
        if (Controllable)
        {
            if (inWater || inLava)
            {
                float horizontalMove = Input.GetAxis("Horizontal");
                float verticalMove = Input.GetAxis("Vertical");
                rb.velocity = new Vector2(horizontalMove * movementSpeed, verticalMove * movementSpeed);
                rb.velocity *= environmentSpeed;
            }
            else
            {
                UpdateNonGroundedTime();
                float horizontalMove = Input.GetAxis("Horizontal");
                float verticalMove = rb.velocity.y;
                if (Input.GetAxis("Jump") > 0 && nonGroundedTime <= coyoteThreshold && !animator.GetCurrentAnimatorStateInfo(0).IsName(JUMP_NAME))
                {
                    ResetParent();
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

    public void ResetParent()
    {
        transform.SetParent(baseParent, true);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Moving"))
        {
            transform.SetParent(collision.transform, true);
        }
        if ((1 << collision.gameObject.layer & groundLayer.value) != 0)
        {
            grounded = IsGrounded(); // update grounded state when entering collision with ground objects
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Moving"))
        {
            ResetParent();
        }
        if ((1 << collision.gameObject.layer & groundLayer.value) != 0)
        {
            grounded = IsGrounded(); // update grounded state when exiting collision with ground objects
        }
    }

}
