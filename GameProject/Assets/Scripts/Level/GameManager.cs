using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public List<Quest> quests = new List<Quest>();

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

    public void SetNewColor(PrismShard prismShard)
    {
        EnvironmentManager.Instance.SetNewColor(prismShard.getColor());
    }

    public void AddSparks(int sparksAmount)
    {
        PlayerPrefs.SetInt("Sparks", PlayerPrefs.GetInt("Sparks") + sparksAmount);
        // update current day's sparks quest
    }
}
