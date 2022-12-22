using System;
using System.Collections;
using UnityEngine;

// Character that can move and jump (in cutscenes)
[RequireComponent(typeof(Rigidbody2D))]
public class MobileCharacter : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private bool animated;
    [SerializeField] private string jumpAnimationName;
    private Transform baseParent;
    private Animator animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        baseParent = transform.parent;
        if (animated)
        {
            animator = GetComponent<Animator>();
            if (animator == null)
            {
                animated = false;
                Debug.LogWarning("MobileCharacter is set to animated, but Animator component was not found. Switching to non-animated");
            }
        }
    }

    // Makes the character move to the position similarly to how a player would, with fixed velocity. Location must be reachable without jumping
    public IEnumerator MoveTo(Vector3 position, float velocity)
    {
        if (!IsMobile()) Debug.LogWarning("Attempting to move a currently immobile MobileCharacter");
        if (velocity <= 0) throw new ArgumentException("Illegal velocity " + velocity);
        float xDiff = position.x - transform.position.x;
        Quaternion rotationGoal;
        float offset = 0.45f;
        float rayLength = 1.25f;
        while (Mathf.Abs(xDiff) >= 0.25)
        {
            rb.velocity = new Vector2(xDiff < 0 ? -velocity : velocity, rb.velocity.y);
            RaycastHit2D left = Physics2D.Raycast(transform.position - new Vector3(offset, 0), -Vector2.up, rayLength, groundLayer);
            RaycastHit2D right = Physics2D.Raycast(transform.position + new Vector3(offset, 0), -Vector2.up, rayLength, groundLayer);
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
            yield return new WaitForFixedUpdate();
            xDiff = position.x - transform.position.x;
        }
        rb.velocity = new Vector2(0, rb.velocity.y);
    }

    public void Jump(float jumpVelocity)
    {
        if (!IsMobile()) Debug.LogWarning("Attempting to use jump on a currently immobile MobileCharacter");
        transform.SetParent(baseParent, true);
        if (animated)
        {
            animator.Play(jumpAnimationName);
        }
        rb.velocity = new Vector2(rb.velocity.x, jumpVelocity);
    }

    public void SetMobile(bool mobile)
    {
        rb.bodyType = mobile ? RigidbodyType2D.Dynamic : RigidbodyType2D.Static;
    }

    public bool IsMobile()
    {
        return rb.bodyType == RigidbodyType2D.Dynamic;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Moving"))
        {
            transform.SetParent(collision.transform, true);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Moving"))
        {
            transform.SetParent(baseParent, true);
        }
    }
}