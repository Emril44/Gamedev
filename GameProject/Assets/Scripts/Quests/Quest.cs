using System;
using System.Collections.Generic;
using UnityEngine;

// Class for logical grouping of objectives and processing their completion
public abstract class Quest : MonoBehaviour
{
    public event Action onStart, onComplete, onUpdate;

    [SerializeField] private QuestData questData;

    private int currentObjective = 0;

    void OnEnable()
    {
        if (currentObjective >= questData.Objectives.Count) return;
        questData.Objectives[currentObjective].onComplete += CompleteCurrentObjective;
        questData.Objectives[currentObjective].SetActive(true);
    }

    void OnDisable()
    {
        if (currentObjective >= questData.Objectives.Count) return;
        questData.Objectives[currentObjective].onComplete -= CompleteCurrentObjective;
        questData.Objectives[currentObjective].SetActive(false);
    }

    public void ResetEvents()
    {
        onStart = null;
        onComplete = null;
        onUpdate = null;
    }

    public bool IsActive()
    {
        return currentObjective > 0 && currentObjective < questData.Objectives.Count;
    }

    public QuestData GetData()
    {
        return questData;
    }

    public string GetTitle()
    {
        return questData.Title;
    }

    public string GetDescription()
    {
        return questData.Description;
    }

    public List<Objective> GetObjectives()
    {
        return questData.Objectives;
    }

    public Objective GetObjective(int i)
    {
        if (i < 0 || i >= questData.Objectives.Count)
        {
            Debug.LogWarning("Trying to get undefined objective " + i + " from quest " + questData.Title);
            return null;
        }
        return questData.Objectives[i];
    }

    public Objective GetCurrentObjective()
    {
        return currentObjective >= questData.Objectives.Count ? null : questData.Objectives[currentObjective];
    }

    // Is called in event of current objective's completion
    private void CompleteCurrentObjective()
    {
        questData.Objectives[currentObjective].onComplete -= CompleteCurrentObjective;
        questData.Objectives[currentObjective].SetActive(false); // only deactivate an objective when its parent quest registered completion
        ActOnObjective(currentObjective);
        currentObjective++;
        if (currentObjective >= questData.Objectives.Count)
        {
            onUpdate?.Invoke();
            onComplete?.Invoke();
            gameObject.SetActive(false);
        }
        else
        {
            questData.Objectives[currentObjective].onComplete += CompleteCurrentObjective;
            questData.Objectives[currentObjective].SetActive(true);
            if (currentObjective == 1) onStart?.Invoke();
            onUpdate?.Invoke();
        }
    }

    // Method that takes action depending on which objective was completed
    protected abstract void ActOnObjective(int objective);

}
