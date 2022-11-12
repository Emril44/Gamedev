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
    }

    public void Load()
    {
        sparksAmount = 0;
        unlockedColors = 0;
    }

    public void Save()
    {

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
