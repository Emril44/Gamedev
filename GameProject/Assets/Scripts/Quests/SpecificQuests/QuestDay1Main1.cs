using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestDay1Main1 : Quest
{
    [SerializeField] private Cutscene[] cutscenes;
    [SerializeField] private NPC notestone;
    [SerializeField] private DialogueBatch notestoneNextBatch;
    protected override void ActOnObjective(int objective)
    {
        switch (objective)
        {
            case 1:
                StartCoroutine(cutscenes[1].Play());
                notestone.SetDialogueBatch(notestoneNextBatch);
                break;
        }
    }

    private void Start()
    {
        StartCoroutine(cutscenes[0].Play());
    }

}
