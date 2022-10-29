using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PlayerInteraction : MonoBehaviour
{
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
        else if (other.gameObject.CompareTag("Damage"))
        {
            //...
        }
        else if (other.gameObject.CompareTag("DeadlyDamage"))
        {
            //...
        }
        else if (other.gameObject.CompareTag("Door"))
        {
            //...
        }
    }
}
