using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class Player : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 3f;
    [SerializeField] private float jumpVelocity = 0.2f;
    private PolygonCollider2D groundCollider;
    private Rigidbody2D rb;

 
    public static Player Instance { get; private set; }
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
        groundCollider = GetComponent<PolygonCollider2D>();
        rb.freezeRotation = true;
    }

    void FixedUpdate()
    {
        float horizontalMove = Input.GetAxis("Horizontal");
        float verticalMove = rb.velocity.y;
        if (Input.GetAxis("Jump") > 0 && groundCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            verticalMove += jumpVelocity;
        }
        rb.velocity = new Vector2(horizontalMove * movementSpeed, verticalMove);
    }
        
    void PutPrism(PrismShard prismShard)
    {
        GameManager.Instance.SetNewColor(prismShard);
    }
    
    void GetSparks(GameObject spark)
    {
        GameManager.Instance.AddSparks(1);
        Destroy(spark.gameObject);
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Prism"))
        {
            PutPrism(other.gameObject.GetComponent<PrismShard>());
        }
        else if (other.gameObject.CompareTag("Spark"))
        {
            GetSparks(other.gameObject);
        }
        else if (other.gameObject.CompareTag("Enemy"))
        {
            //...
        }
        else if (other.gameObject.CompareTag("Trap"))
        {
            //...
        }
        else if (other.gameObject.CompareTag("Door"))
        {
            //...
        }
    }
}
