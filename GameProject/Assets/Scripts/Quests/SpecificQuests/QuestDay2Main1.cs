using System.Collections;
using UnityEngine;

public class QuestDay2Main1 : Quest
{
    [SerializeField] private Cutscene[] cutscenes;
    [SerializeField] private NPC notestone;
    [SerializeField] private NPC circle;
    [SerializeField] private DialogueBatch circleInitBatch;
    [SerializeField] private DialogueBatch notestoneNextBatch;

    protected override void FirstEnableSequence()
    {
        StartCoroutine(Intro());
    }

    private IEnumerator Intro()
    {
        yield return cutscenes[0].Play();
        yield return cutscenes[1].Play();
    }

    protected override void ActOnObjective(int objective)
    {
        switch (objective)
        {
            case 0:
                circle.SetDialogueBatch(circleInitBatch);
                break;
            case 1:
                notestone.SetDialogueBatch(notestoneNextBatch);
                StartCoroutine(cutscenes[2].Play());
                break;
        }
    }

}

