using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private List<DialogueBatch> batches = new List<DialogueBatch>(); // predefined dialogue states, indexed by this list (used in saves)
    private int batchIndex;
    private DialogueBatch batch;
    private int dialogueIndex;

    public void Awake()
    {
        batchIndex = 0;
        batch = batches[batchIndex];
        dialogueIndex = 0;
        if (dialogueIndex > batch.dialogueList.Count && batch.fallbackDialogue == null) gameObject.tag = "Untagged";
    }

    public void TriggerDialogue()
    {
        if (dialogueIndex < batch.dialogueList.Count)
        {
            DialogueManager.Instance.GetTriggered(batch.dialogueList[dialogueIndex]);
            dialogueIndex++;
            if (dialogueIndex > batch.dialogueList.Count && batch.fallbackDialogue == null) gameObject.tag = "Untagged"; // remove trigger if there are no dialogues and no fallback dialogue left
        }
        else
        {
            DialogueManager.Instance.GetTriggered(batch.fallbackDialogue);
        }
    }

    public int GetBatchIndex()
    {
        return batchIndex;
    }

    public void SetBatchIndex(int index)
    {
        batchIndex = index;
        batch = batches[index];
        dialogueIndex = 0;
        if (dialogueIndex > batch.dialogueList.Count && batch.fallbackDialogue == null) gameObject.tag = "Untagged";
        else gameObject.tag = "DialogueTrigger";
    }

    public int GetDialogueIndex()
    {
        return dialogueIndex;
    }

    public void SetDialogueIndex(int index)
    {
        batchIndex = index;
        if (dialogueIndex > batch.dialogueList.Count && batch.fallbackDialogue == null) gameObject.tag = "Untagged";
        else gameObject.tag = "DialogueTrigger";
    }

    public Dialogue GetCurrentDialogue()
    {
        if (dialogueIndex < batch.dialogueList.Count) return batch.dialogueList[dialogueIndex];
        return batch.fallbackDialogue;
    }

    public DialogueBatch GetBatchAtIndex(int i)
    {
        return batches[i];
    }
}
