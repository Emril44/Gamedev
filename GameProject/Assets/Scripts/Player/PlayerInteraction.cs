using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private Collider2D bodyCollider;
    private bool isGrabbing = false;
    private Transform oldParent;
    private GameObject grabbedObject;
    
    void PutPrism(PrismShard prismShard)
    {
        GameManager.Instance.SetNewColor(prismShard);
    }

    void GetSparks(GameObject spark)
    {
        GameManager.Instance.AddSparks(1);
        Destroy(spark.gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            //drop
            if (isGrabbing)
            {
                isGrabbing = false;
                grabbedObject.transform.parent = oldParent;
                grabbedObject.GetComponent<Rigidbody2D>().simulated = true;
                grabbedObject.GetComponent<Collider2D>().enabled = true;
                grabbedObject = null;
            }
            else //grab
            {
                var colliders = new List<Collider2D>();
                var filter = new ContactFilter2D();
                filter.useTriggers = true;
                bodyCollider.OverlapCollider(filter, colliders);
                foreach (Collider2D collider in colliders)
                {
                    if (collider.gameObject.CompareTag("Grabbable"))
                    {
                        grabbedObject = collider.gameObject;
                        oldParent = grabbedObject.transform.parent;
                        grabbedObject.transform.SetParent(transform);
                        grabbedObject.GetComponent<Collider2D>().enabled = false;
                        grabbedObject.GetComponent<Rigidbody2D>().simulated = false;
                        grabbedObject.transform.localPosition = Vector3.zero;
                        isGrabbing = true;
                        break;
                    }
                }
            }
            
        }
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
