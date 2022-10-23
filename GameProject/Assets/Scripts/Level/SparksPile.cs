using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class SparksPile : MonoBehaviour
{
    public int sparksAmount;

    private void Awake()
    {
        if (sparksAmount <= 0)
        {
            Debug.LogError("There are " + sparksAmount + " sparks in \"" + gameObject.name + "\"");
        }
    }
}
