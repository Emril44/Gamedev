using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ObjectiveZone : MonoBehaviour
{
    [SerializeField] private LocationObjective objective;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) objective.ReactToEntry();
    }
}
