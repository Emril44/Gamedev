using System.Collections;
using UnityEngine;

public class QuestDay1Main1 : Quest
{
    [SerializeField] private Cutscene[] cutscenes;
    [SerializeField] private NPC notestone;
    [SerializeField] private NPC circle;
    [SerializeField] private DialogueBatch notestoneNextBatch;
    [SerializeField] private DialogueBatch circleNextBatch;

    protected override void FirstEnableSequence()
    {
        StartCoroutine(Intro());
    }

    private IEnumerator Intro()
    {
        yield return cutscenes[0].Play();
        GameObject.FindGameObjectWithTag("SavesManager").SendMessage("Autosave");

    }

    protected override void ActOnObjective(int objective)
    {
        switch (objective)
        {
            case 1:
                notestone.SetDialogueBatch(notestoneNextBatch);
                circle.SetDialogueBatch(circleNextBatch);
                StartCoroutine(cutscenes[1].Play());
                break;
        }
    }

}
