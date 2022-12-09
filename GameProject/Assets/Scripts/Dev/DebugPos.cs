using UnityEngine;

public class DebugPos : MonoBehaviour
{
    public bool local;
    void Update()
    {
        if (local)
            Debug.Log(transform.localPosition);
        else
            Debug.Log(transform.position);
    }
}
