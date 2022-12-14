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
        circle.SetDialogueBatch(circleInitBatch);
    }

    private IEnumerator Intro()
    {
        yield return cutscenes[0].Play();
        yield return cutscenes[1].Play();
        GameObject.FindGameObjectWithTag("SavesManager").SendMessage("Autosave");
        PlayerPrefs.SetString("FlagUAUnlocked", "True");
    }

    protected override void ActOnObjective(int objective)
    {
        switch (objective)
        {
            case 1:
                notestone.SetDialogueBatch(notestoneNextBatch);
                StartCoroutine(cutscenes[2].Play());
                break;
        }
    }

}

