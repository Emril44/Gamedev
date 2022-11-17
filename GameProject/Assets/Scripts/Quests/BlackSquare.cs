using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class BlackSquare : MonoBehaviour
{
    [SerializeField] private int health = 2;
    [SerializeField] private float undamageableTime = 0.55f;
    [SerializeField] private Collider2D bodyCollider;
    private bool damageable = true;

    void FixedUpdate()
    {
        if (bodyCollider.IsTouchingLayers(LayerMask.GetMask("Damage")))
        {
            GetDamaged();
        }
    }
    
    private void GetDamaged()
    {
        if (damageable)
        {
            damageable = false;
            health--;
            StartCoroutine(Undamageable());
        }
        if (health <= 0)
        {
            Die();
        }
    }

    IEnumerator Undamageable()
    {
        yield return new WaitForSeconds(undamageableTime);
        damageable = true;
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
