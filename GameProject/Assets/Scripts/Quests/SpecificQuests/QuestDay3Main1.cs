using System.Collections;
using UnityEngine;

public class QuestDay3Main1 : Quest
{
    [SerializeField] private Cutscene[] cutscenes;


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
            case 3:
                StartCoroutine(cutscenes[2].Play());
                break;

        }
    }

}
