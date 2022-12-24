using System;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public event Action onSparksUpdate;
    public int sparksAmount { get; private set; }
    public int unlockedColors { get; private set; }
    //TODO: day
    public int day;
    private int timePlayed;
    private float startTime;
    public static DataManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        sparksAmount = 0;
        unlockedColors = 0;
        day = 1;
        startTime = Time.time;
    }

    public DataManagerSerializedData Serialize()
    {
        return new DataManagerSerializedData(sparksAmount, unlockedColors, timePlayed + (int)(Time.time - startTime));
    }

    public void Deserialize(DataManagerSerializedData data)
    {
        if (data == null) return;
        sparksAmount = data.sparksAmount;
        unlockedColors = data.unlockedColors;
        day = data.day;
        timePlayed = data.timePlayed;
        onSparksUpdate?.Invoke();
    }

    public void AddSpark()
    {
        sparksAmount++;
        onSparksUpdate?.Invoke();
    }

    public void UnlockColor()
    {
        unlockedColors++;
    }
}
