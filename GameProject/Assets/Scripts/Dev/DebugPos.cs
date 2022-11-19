using UnityEngine;

public class DebugPos : MonoBehaviour
{
    public string name;

    // Update is called once per frame
    void Update()
    {
        Debug.Log(name + ": " + transform.position);
    }
}
