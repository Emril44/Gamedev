using UnityEngine;

public class DebugPos : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        Debug.Log(name + ": " + transform.position);
    }
}
