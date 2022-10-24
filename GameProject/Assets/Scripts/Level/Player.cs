using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class Player : MonoBehaviour
{
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
    }
    
    void PutPrism(PrismFragment prismFragment)
    {
        GameManager.Instance.SetNewColor(prismFragment);
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
            PutPrism(other.gameObject.GetComponent<PrismFragment>());
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
