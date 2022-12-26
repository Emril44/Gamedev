using UnityEngine;

public class QuestDay1Main2 : Quest
{
    [SerializeField] private Cutscene[] cutscenes;
    [SerializeField] private NPC druid;
    [SerializeField] private DialogueBatch[] druidBatches; // Druid's dialogue batches in order

    protected override void FirstEnableSequence()
    {
        StartCoroutine(cutscenes[0].Play());
    }

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
            case 3:
                druid.SetDialogueBatch(druidBatches[2]);
                break;
            case 4:
                druid.SetDialogueBatch(druidBatches[3]);
                DataManager.Instance.day++;
                break;
        }
    }

}

