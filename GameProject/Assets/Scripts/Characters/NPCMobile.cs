using System;
using System.Collections;
using UnityEngine;

// NPC that can move and jump (in cutscenes)
[RequireComponent(typeof(DialogueTrigger)), RequireComponent(typeof(Rigidbody2D))]
public class NPCMobile : NPC
{
    private Rigidbody2D rb;
    [SerializeField] private LayerMask groundLayer;
    private Transform baseParent;

    override protected void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
        baseParent = transform.parent;
    }

    // Makes the NPC move to the position similarly to how a player would, with fixed velocity. Location must be reachable without jumping
    public IEnumerator MoveTo(Vector3 position, float velocity)
    {
        SetMobile(true);
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
        SetMobile(false);
    }

    // Should be made explicitly immobile at some point after jumping
    public void Jump(float jumpVelocity)
    {
        SetMobile(true);
        transform.SetParent(baseParent, true);
        // animation maybe?
        rb.velocity = new Vector2(rb.velocity.x, jumpVelocity);
    }

    public void SetMobile(bool mobile)
    {
        rb.bodyType = mobile ? RigidbodyType2D.Dynamic : RigidbodyType2D.Static;
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
