using UnityEngine;

public class QuestDay3Main1 : Quest
{
    [SerializeField] private Cutscene[] cutscenes;


    protected override void FirstEnableSequence()
    {
        StartCoroutine(cutscenes[0].Play());
        StartCoroutine(cutscenes[1].Play());
    }

    protected override void ActOnObjective(int objective)
    {
        switch (objective)
        {
            case 3:
                StartCoroutine(cutscenes[2].Play());
                break;

        }
    }

}
