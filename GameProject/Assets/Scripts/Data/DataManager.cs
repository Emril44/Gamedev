using System;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public event Action onSparksUpdate;
    public event Action<int> onTouchedLettersGathered; // parameter is day (1-3)
    public int sparksAmount { get; private set; }
    public int unlockedColors { get; private set; }
    public int day;
    private int timePlayed;
    private float startTime;
    private int[] touchedLetters; // array for days
    [SerializeField] private int[] dayLetterTargets;
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
        touchedLetters = new int[dayLetterTargets.Length];
    }

    public DataManagerSerializedData Serialize()
    {
        return new DataManagerSerializedData(sparksAmount, unlockedColors, timePlayed + (int)(Time.time - startTime), day, touchedLetters);
    }

    public void Deserialize(DataManagerSerializedData data)
    {
        if (data == null) return;
        sparksAmount = data.sparksAmount;
        unlockedColors = data.unlockedColors;
        day = data.day;
        timePlayed = data.timePlayed;
        touchedLetters = data.touchedLetters;
        onSparksUpdate?.Invoke();
    }

    public void AddSpark()
    {
        sparksAmount++;
        onSparksUpdate?.Invoke();
    }

    // day starts from 1
    public void TouchLetter(int day)
    {
        if (++touchedLetters[day - 1] == dayLetterTargets[day - 1]) onTouchedLettersGathered?.Invoke(day);
    }

    public void UnlockColor()
    {
        unlockedColors++;
    }
}
