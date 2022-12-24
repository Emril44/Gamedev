using UnityEngine;

public class QuestDay2Main2 : Quest
{
    [SerializeField] private Cutscene[] cutscenes;
    [SerializeField] private NPC cat;
    [SerializeField] private DialogueBatch[] catBatches; // Druid's dialogue batches in order

    protected override void FirstEnableSequence()
    {
        StartCoroutine(cutscenes[0].Play());
    }

    protected override void ActOnObjective(int objective)
    {
        switch (objective)
        {
            case 1:
                cat.SetDialogueBatch(catBatches[0]);
                StartCoroutine(cutscenes[1].Play());
                break;
            case 3:
                cat.SetDialogueBatch(catBatches[1]);
                break;
            case 4:
                StartCoroutine(cutscenes[2].Play());
                break;
        }
    }

}

