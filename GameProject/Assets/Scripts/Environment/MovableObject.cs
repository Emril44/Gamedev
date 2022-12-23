using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class MovableObject : MonoBehaviour
{
    private Rigidbody2D rb;
    [NonSerialized] public bool Grabbed;
    [SerializeField] private float resetDelay;
    [SerializeField] private bool grabbable;
    [SerializeField] private OverlapHandler overlap;
    private Transform baseParent;
    private Vector3 defPosition; // position by default
    private Vector3 defScale; // scale by default
    private Quaternion defRotation; // rotation by default
    private float timeFromMoveStart = -1; // if -1, object is considered currently static

    private void Awake()
    {
        
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Static;
        baseParent = transform.parent;
        defPosition = transform.position;
        defScale = transform.localScale;
        defRotation = transform.rotation;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Water") || other.CompareTag("Lava"))
        {
            rb.gravityScale = 0.1f;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Water") || other.CompareTag("Lava"))
        {
            rb.gravityScale = 1f;
        }
    }

    private void Update()
    {
        if (timeFromMoveStart >= 0 && !Grabbed)
        {
            
            timeFromMoveStart += Time.deltaTime;
            if (overlap.Overlapping || timeFromMoveStart >= resetDelay) Reset();
        }
    }

    private void Reset()
    {
        ScreenFade.Instance.FadeOut(5);
        transform.position = defPosition;
        transform.localScale = defScale;
        transform.rotation = defRotation;
        timeFromMoveStart = -1;
        gameObject.tag = "Movable";
        rb.bodyType = RigidbodyType2D.Static;
        // play sound
        ScreenFade.Instance.FadeIn(5);
    }

    public void StartMove()
    {
        timeFromMoveStart = 0;
        if (grabbable) gameObject.tag = "Grabbable";
        else gameObject.tag = "Untagged";
        rb.bodyType = RigidbodyType2D.Dynamic;
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
