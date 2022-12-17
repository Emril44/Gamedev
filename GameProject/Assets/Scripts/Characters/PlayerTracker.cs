using System.Collections;
using UnityEngine;

// Radius of the CircleCollider2D is the effective detection radius
[RequireComponent(typeof(CircleCollider2D))]
public class PlayerTracker : MonoBehaviour
{
    private GameObject player;
    [SerializeField] private LayerMask obstructionMask;
    [SerializeField] private float checkDelaySeconds = 0.2f;
    private CircleCollider2D trigger;
    public bool PlayerInRange { get; private set; }
    public bool PlayerVisible { get; private set; }
    public Vector3 TargetPos { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        player = PlayerInteraction.Instance.gameObject;
        trigger = GetComponent<CircleCollider2D>();
        StartCoroutine(Track());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.Equals(player)) PlayerInRange = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.Equals(player)) PlayerInRange = false;
    }

    private IEnumerator Track()
    {
        WaitForSeconds wait = new WaitForSeconds(checkDelaySeconds);
        while(true)
        {
            if (PlayerInRange && PlayerInSight())
            {
                TargetPos = player.transform.position;
                PlayerVisible = true;
            }
            else
            {
                PlayerVisible = false;
            }
            yield return wait;
        }
    }

    // to be used when it is known that the player is in range
    private bool PlayerInSight()
    {
        Vector2 toPlayer = player.transform.position - transform.position;
        return !Physics2D.Raycast(transform.position, toPlayer.normalized, toPlayer.magnitude, obstructionMask);
    }

    public float GetDetectionRadius()
    {
        return trigger.radius;
    }

    public void SetDetectionRadius(float radius)
    {
        trigger.radius = radius;
    }
}
