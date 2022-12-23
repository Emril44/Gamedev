using System;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public event Action onSparksUpdate;
    public int sparksAmount { get; private set; }
    public int unlockedColors { get; private set; }
    //TODO: day
    public int day;
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
        unlockedColors = 3;
        day = 0;
    }

    public DataManagerSerializedData Serialize()
    {
        return new DataManagerSerializedData(sparksAmount, unlockedColors);
    }

    public void Deserialize(DataManagerSerializedData data)
    {
        if (data == null) return;
        sparksAmount = data.sparksAmount;
        unlockedColors = data.unlockedColors;
        day = data.day;
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
