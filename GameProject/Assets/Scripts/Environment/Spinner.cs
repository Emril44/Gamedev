using UnityEngine;

public class Spinner : MonoBehaviour
{
    [SerializeField] private float spinSpeed;
    private void FixedUpdate()
    {
        Vector3 euler = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(euler.x, euler.y, euler.z + spinSpeed);
    }
}
