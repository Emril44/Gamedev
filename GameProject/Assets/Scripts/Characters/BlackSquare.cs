using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider2D)), RequireComponent(typeof(Rigidbody2D))]
public class BlackSquare : MonoBehaviour
{    public enum State
    {
        Idle,
        Pursuing,
        Alert
    }
    private State state = State.Idle;
    [SerializeField] private int health = 2;
    [SerializeField] private float undamageableTime = 0.55f;
    [SerializeField] private Collider2D bodyCollider;
    [Range(1f, 15f)]
    [SerializeField] private float pursueSpeed = 5f;
    [SerializeField] private float pursuitTimeout = 10f; // time for which the square keeps Pursuing a target without updates (updates happen when the square can see the player); square's detection radius is doubled when pursuing
    [SerializeField] private float alertTimeout = 10f; // time for which the square stays in place after unsuccessfully pursuing the player, its detection radius is still doubled; if the square still can't see the player, it will reset to original position
    [SerializeField] private PlayerTracker tracker;
    private Rigidbody2D rb;
    private float timeSinceTargetUpdate = 0;
    private Vector3 origin;
    private Transform baseParent;
    private bool damageable = true;
    private Animator animator;
    private const string DAMAGE_NAME = "Enemy_Damage";
    private const string DEATH_NAME = "Enemy_Death";


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        origin = transform.position;
        baseParent = transform.parent;
    }

    void FixedUpdate()
    {
        switch(state)
        {
            case State.Idle:
                ProcessIdleState();
                break;
            case State.Pursuing:
                ProcessPursuingState();
                break;
            case State.Alert:
                ProcessAlertState();
                break;
        }
    }

    private void Reset()
    {
        // trigger VFX and SFX...

        transform.position = origin;
    }

    private void ProcessIdleState()
    {
        if (tracker.PlayerVisible)
        {
            state = State.Pursuing;
            timeSinceTargetUpdate = 0;
            tracker.SetDetectionRadius(tracker.GetDetectionRadius() * 2);
            Debug.Log("Now Pursuing");
        }
    }

    private void ProcessPursuingState()
    {
        if (tracker.PlayerVisible)
        {
            timeSinceTargetUpdate = 0;
        }
        else
        {
            timeSinceTargetUpdate += Time.fixedDeltaTime;
        }
        if (timeSinceTargetUpdate >= pursuitTimeout)
        {
            state = State.Alert;
            Debug.Log("Now Alert");
            return;
        }
        else
        {
            float xDiff = tracker.TargetPos.x - transform.position.x;
            Debug.Log(tracker.TargetPos);
            if (Mathf.Abs(xDiff) >= 0.5)
            {
                if (xDiff > 0) rb.velocity = new Vector2(pursueSpeed, rb.velocity.y);
                else rb.velocity = new Vector2(-pursueSpeed, rb.velocity.y);
            }
        }
    }
    

    private void ProcessAlertState()
    {
        if (tracker.PlayerVisible)
        {
            state = State.Pursuing;
            Debug.Log("Now Pursuing");
        }
        else
        {
            timeSinceTargetUpdate += Time.fixedDeltaTime;
        }
        if (timeSinceTargetUpdate >= pursuitTimeout + alertTimeout)
        {
            state = State.Idle;
            tracker.SetDetectionRadius(tracker.GetDetectionRadius() / 2);
            Reset();
            Debug.Log("Now Idle");
            return;
        }
    }

    private void GetDamaged()
    {
        if (damageable)
        {
            damageable = false;
            health--;
            animator.Play(DAMAGE_NAME);
            StartCoroutine(Undamageable());
        }
        if (health <= 0)
        {
            animator.SetTrigger("EnemyDeath");
            StartCoroutine(Die());
        }
    }

    IEnumerator Undamageable()
    {
        yield return new WaitForSeconds(undamageableTime);
        damageable = true;
    }

    private IEnumerator Die()
    {
        animator.Play(DEATH_NAME);
        yield return new WaitForSeconds(GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
        DataManager.Instance.AddSpark();
        gameObject.SetActive(false);
    }
    void OnTriggerStay2D(Collider2D other)
    {
        switch (other.gameObject.tag)
        {
            case "Lava":
                GetDamaged();
                break;
            case "Damage":
                if (other.gameObject.GetComponent<BlackSquare>() == null) GetDamaged(); // avoid damaging from collision with fellow black squares
                break;
        }
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
