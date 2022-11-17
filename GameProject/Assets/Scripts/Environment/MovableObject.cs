using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class MovableObject : MonoBehaviour
{
    private Rigidbody2D rb;
    [NonSerialized] public bool Grabbed;
    [SerializeField] private float resetDelay;
    [SerializeField] private bool grabbable;
    private Vector3 defPosition; // position by default
    private Vector3 defScale; // scale by default
    private Quaternion defRotation; // rotation by default
    private float timeFromMoveStart = -1; // if -1, object is considered currently static

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Static;
        defPosition = transform.position;
        defScale = transform.localScale;
        defRotation = transform.rotation;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Water"))
        {
            rb.gravityScale = 0.1f;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Water"))
        {
            rb.gravityScale = 1f;
        }
    }

    private void Update()
    {
        if (timeFromMoveStart >= 0 && !Grabbed)
        {
            timeFromMoveStart += Time.deltaTime;
            if (timeFromMoveStart >= resetDelay)
            {
                transform.position = defPosition;
                transform.localScale = defScale;
                transform.rotation = defRotation;
                timeFromMoveStart = -1;
                gameObject.tag = "Movable";
                rb.bodyType = RigidbodyType2D.Static;
            }
        }
    }

    public void StartMove()
    {
        timeFromMoveStart = 0;
        if (grabbable) gameObject.tag = "Grabbable";
        else gameObject.tag = "Untagged";
        rb.bodyType = RigidbodyType2D.Dynamic;
    }
}
