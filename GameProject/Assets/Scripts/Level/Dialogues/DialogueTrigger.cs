using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private List<Dialogue> dialogues; // must always have at least 1 fallback dialogue, which will repeat when other dialogues are exhausted

    public void TriggerDialogue()
    {
        //...
    }
}
