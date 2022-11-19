using UnityEngine;

public class Save : ScriptableObject
{
    public int time;
    public int day;
    public int sparks;
    
    public Save(int time, int day, int sparks)
    {
        this.time = time;
        this.day = day;
        this.sparks = sparks;
    }
}
