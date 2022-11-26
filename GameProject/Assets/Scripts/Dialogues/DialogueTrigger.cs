using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private List<Dialogue> dialogueList; // main sequence of one-time dialogues. each interaction with the trigger starts the first dialogue and removes it from the list
    [SerializeField] private Dialogue fallbackDialogue; // one dialogue that will keep repeating after the main sequence was finished

    public void Awake()
    {
        if (dialogueList.Count == 0 && fallbackDialogue == null) gameObject.tag = "Untagged";
    }

    public void TriggerDialogue()
    {
        if (dialogueList.Count > 0)
        {
            DialogueManager.Instance.GetTriggered(dialogueList.First());
            dialogueList.RemoveAt(0);
            if (dialogueList.Count == 0 && fallbackDialogue == null) gameObject.tag = "Untagged"; // remove DialogueTrigger tag from the base object if the trigger has no dialogues
        }
        else
        {
            DialogueManager.Instance.GetTriggered(fallbackDialogue);
        }
    }

    public void SetDialogueData(List<Dialogue> dialogueList, Dialogue fallbackDialogue)
    {
        this.dialogueList = dialogueList;
        this.fallbackDialogue = fallbackDialogue;
        if (dialogueList.Count > 0 || fallbackDialogue != null) gameObject.tag = "DialogueTrigger";
        else gameObject.tag = "Untagged";
    }

    public Dialogue GetCurrentDialogue()
    {
        if (dialogueList.Count > 0) return dialogueList.First();
        return fallbackDialogue;
    }
}
