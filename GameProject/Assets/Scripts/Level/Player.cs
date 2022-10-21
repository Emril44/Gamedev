using UnityEngine;

public class Player : MonoBehaviour
{
    void PutPrism(PrismFragment prismFragment)
    {
        GameManager.Instance.SetNewColor(prismFragment);
    }
    void GetSparks(SparksPile sparksPile)
    {
        GameManager.Instance.AddSparks(sparksPile.sparksAmount);
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
