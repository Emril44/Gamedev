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
    void GetSparks(SparksPile sparksPile)
    {
        GameManager.Instance.AddSparks(sparksPile.sparksAmount);
        Destroy(sparksPile.gameObject);
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Prism"))
        {
            PutPrism(other.gameObject.GetComponent<PrismFragment>());
        }
        else if (other.gameObject.CompareTag("Sparks"))
        {
            GetSparks(other.gameObject.GetComponent<SparksPile>());
        }
        else if (other.gameObject.CompareTag("Enemy"))
        {
            //...
        }
        else if (other.gameObject.CompareTag("Door"))
        {
            //...
        }
    }
}
