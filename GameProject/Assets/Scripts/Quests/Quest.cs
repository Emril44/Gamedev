using System;
using System.Collections.Generic;
using UnityEngine;

// Class for logical grouping of objectives and processing their completion
public abstract class Quest : MonoBehaviour
{
    public event Action onStart, onComplete, onUpdate;

    [SerializeField] private QuestData questData;

    private int currentObjective = 0;
    private void Start()
    {
        if (currentObjective == questData.RequisiteObjectives) onStart?.Invoke();
    }

    private void OnEnable()
    {
        if (currentObjective >= questData.Objectives.Count) return;
        questData.Objectives[currentObjective].onComplete += CompleteCurrentObjective;
        questData.Objectives[currentObjective].onUpdate += UpdateQuest;
        questData.Objectives[currentObjective].SetActive(true);
    }

    private void OnDisable()
    {
        if (currentObjective >= questData.Objectives.Count) return;
        questData.Objectives[currentObjective].onComplete -= CompleteCurrentObjective;
        questData.Objectives[currentObjective].onUpdate -= UpdateQuest;
        questData.Objectives[currentObjective].SetActive(false);
    }

    private void UpdateQuest()
    {
        onUpdate?.Invoke();
    }

    public void ResetEvents()
    {
        onStart = null;
        onComplete = null;
        onUpdate = null;
    }

    public bool IsActive()
    {
        return gameObject.activeSelf && currentObjective >= questData.RequisiteObjectives && currentObjective < questData.Objectives.Count;
    }

    public QuestData GetData()
    {
        return questData;
    }

    public List<Objective> GetObjectives()
    {
        return questData.Objectives;
    }

    public Objective GetObjective(int i)
    {
        if (i < 0 || i >= questData.Objectives.Count)
        {
            Debug.LogWarning("Trying to get undefined objective " + i + " from quest " + questData.LocalizedTitle());
            return null;
        }
        return questData.Objectives[i];
    }

    public Objective GetCurrentObjective()
    {
        return currentObjective >= questData.Objectives.Count ? null : questData.Objectives[currentObjective];
    }

    public int GetCurrentObjectiveIndex()
    {
        return currentObjective;
    }

    public void SetCurrentObjectiveIndex(int index)
    {
        if (IsActive())
        {
            questData.Objectives[currentObjective].onComplete -= CompleteCurrentObjective;
            questData.Objectives[currentObjective].onUpdate -= UpdateQuest;
        }
        currentObjective = index;
        if (IsActive())
        {
            questData.Objectives[currentObjective].onComplete += CompleteCurrentObjective;
            questData.Objectives[currentObjective].onUpdate += UpdateQuest;
        }

    }

    // Is called in event of current objective's completion
    private void CompleteCurrentObjective()
    {
        questData.Objectives[currentObjective].onComplete -= CompleteCurrentObjective;
        questData.Objectives[currentObjective].onUpdate -= UpdateQuest;
        questData.Objectives[currentObjective].SetActive(false); // only deactivate an objective when its parent quest registered completion
        ActOnObjective(currentObjective);
        currentObjective++;
        if (currentObjective >= questData.Objectives.Count)
        {
            onUpdate?.Invoke();
            onComplete?.Invoke();
        }
        else
        {
            questData.Objectives[currentObjective].onComplete += CompleteCurrentObjective;
            questData.Objectives[currentObjective].onUpdate += UpdateQuest;
            questData.Objectives[currentObjective].SetActive(true);
            if (currentObjective == questData.RequisiteObjectives)
            {
                onStart?.Invoke();
            }
            onUpdate?.Invoke();
        }
    }

    // Method that takes action depending on which objective was completed
    protected abstract void ActOnObjective(int objective);

}
