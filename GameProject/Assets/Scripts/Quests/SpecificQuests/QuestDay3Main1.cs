using System.Collections;
using UnityEngine;

public class QuestDay3Main1 : Quest
{
    [SerializeField] private NPC cat;
    [SerializeField] private DialogueBatch fireResBatch;
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

    private IEnumerator Outro()
    {
        yield return cutscenes[2].Play();
        yield return cutscenes[3].Play();
        PlayerPrefs.SetString("EndingSkinUnlocked", "True");
    }

    protected override void ActOnObjective(int objective)
    {
        switch (objective)
        {
            case 1:
                cat.SetDialogueBatch(fireResBatch);
                break;
            case 3:
                StartCoroutine(Outro());
                break;

        }
    }

}
