using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class BlackSquare : MonoBehaviour
{
    [SerializeField] private int health = 2;
    [SerializeField] private float undamageableTime = 0.55f;
    [SerializeField] private Collider2D bodyCollider;
    private bool damageable = true;
    private Animator animator;
    private const string DAMAGE_NAME = "Enemy_Damage";
    private const string DEATH_NAME = "Enemy_Death";

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

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
        Destroy(gameObject);
    }
}
