using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestDay1Main2 : Quest
{
    [SerializeField] private Cutscene[] cutscenes;
    [SerializeField] private NPC druid;
    [SerializeField] private DialogueBatch[] druidBatches; // Druid's dialogue batches in order
    protected override void ActOnObjective(int objective)
    {
        switch (objective)
        {
            case 0:
                druid.SetDialogueBatch(druidBatches[0]);
                break;
            case 1:
                druid.SetDialogueBatch(druidBatches[1]);
                break;
            case 2:
                StartCoroutine(cutscenes[1].Play());
                break;
        }
    }

    private void Start()
    {
        StartCoroutine(cutscenes[0].Play());
    }

}

