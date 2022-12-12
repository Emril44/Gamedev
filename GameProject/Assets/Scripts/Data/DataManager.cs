using System;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public event Action onSparkCollect;
    public int sparksAmount { get; private set; }
    public int unlockedColors { get; private set; }
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
    }

    public DataManagerSerializedData Serialize()
    {
        return new DataManagerSerializedData(sparksAmount, unlockedColors);
    }

    public void Deserialize(DataManagerSerializedData data)
    {
        sparksAmount = data.sparksAmount;
        unlockedColors = data.unlockedColors;
    }

    public void AddSpark()
    {
        sparksAmount++;
        onSparkCollect?.Invoke();
    }

    public void Die()
    {
        //TODO: death screen
    }

    public void UnlockColor()
    {
        unlockedColors++;
    }
}
