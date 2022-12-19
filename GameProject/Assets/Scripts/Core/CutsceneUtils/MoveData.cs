using UnityEngine;

[System.Serializable]
public class MoveData
{
    public GameObject target;
    public float velocity;

    public MoveData(GameObject target, float velocity)
    {
        this.target = target;
        this.velocity = velocity;
    }
}
