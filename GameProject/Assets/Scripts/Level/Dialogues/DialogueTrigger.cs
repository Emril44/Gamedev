using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private Dialogue dialogue; // must always have at least 1 fallback dialogue, which will repeat when other dialogues are exhausted

    public void TriggerDialogue(GameObject NPC)
    {
        DialogueManager.Instance.GetTriggered(NPC, dialogue);
    }
}