using UnityEngine;

public class QuestDay1Main1 : Quest
{
    [SerializeField] private Cutscene[] cutscenes;
    [SerializeField] private NPC notestone;
    [SerializeField] private NPC circle;
    [SerializeField] private DialogueBatch notestoneNextBatch;
    [SerializeField] private DialogueBatch circleNextBatch;
    protected override void ActOnObjective(int objective)
    {
        switch (objective)
        {
            case 1:
                StartCoroutine(cutscenes[1].Play());
                notestone.SetDialogueBatch(notestoneNextBatch);
                circle.SetDialogueBatch(circleNextBatch);
                break;
        }
    }

    private void Start()
    {
        StartCoroutine(cutscenes[0].Play());
    }

}
