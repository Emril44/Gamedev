using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Analytics;

public class AnalyticsManager : MonoBehaviour
{
    public static AnalyticsManager Instance { get;private set; }
    public void Awake()
    {
        if (Instance == null || Instance != this)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    async void Start()
    {
        await UnityServices.InitializeAsync();
        await AnalyticsService.Instance.CheckForRequiredConsents();
    }

    public void DayFinishedEvent(int day, int sparksAmount, int seconds)
    {
        AnalyticsService.Instance.CustomData("dayFinished", new Dictionary<string, object>
        {
            { "Day", day },
            { "Sparks", sparksAmount },
            { "TimeM", seconds/60f }
        });
    }
    
    public void DeathEvent(int day, string gameObjectName)
    {
        AnalyticsService.Instance.CustomData("death", new Dictionary<string, object>
        {
            { "Day", day },
            { "Reason", gameObjectName }
        });
    }

    public void QuestFinishedEvent(string questName, int seconds)
    {
        AnalyticsService.Instance.CustomData("questFinished", new Dictionary<string, object>
        {
            { "QuestName", questName },
            { "TimeM", seconds/60f }
        });
    }
}
