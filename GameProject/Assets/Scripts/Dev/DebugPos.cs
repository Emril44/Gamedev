using UnityEngine;

public class DebugPos : MonoBehaviour
{
    void Update()
    {
        Debug.Log(name + ": " + transform.position);
    }
}
