using UnityEngine;

public class SampleQuest : Quest
{
    void Start()
    {
        onStart += () => { Debug.Log("Start sample quest"); };
        onComplete += () => { Debug.Log("Complete sample quest"); };
    }

    protected override void ActOnObjective(int objective)
    {
        switch(objective)
        {
            case 0:
                Debug.Log("Complete objective 0, dialogue, " + GetObjective(objective).LocalizedMessage());
                break;
            case 1:
                Debug.Log("Complete objective 1, location, " + GetObjective(objective).LocalizedMessage());
                break;
            case 2:
                Debug.Log("Complete objective 2, location, " + GetObjective(objective).LocalizedMessage());
                break;
        }
    }
}
