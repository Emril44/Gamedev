using System;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private List<DialogueBatch> batches = new List<DialogueBatch>(); // predefined dialogue states, indexed by this list (used in saves)
    private Dictionary<DialogueBatch, int> batchesToIds = new Dictionary<DialogueBatch, int>();
    private int batchIndex;
    private DialogueBatch batch;
    private int dialogueIndex;

    public void Awake()
    {
        batchIndex = 0;
        batch = batches[batchIndex];
        dialogueIndex = 0;
        if (dialogueIndex >= batch.dialogueList.Count && batch.fallbackDialogue == null) gameObject.tag = "Untagged";
        for (int i = 0; i < batches.Count; i++) batchesToIds.Add(batches[i], i);
    }

    public Dialogue TriggerDialogue()
    {
        if (dialogueIndex < batch.dialogueList.Count)
        {
            Dialogue dialogue = batch.dialogueList[dialogueIndex];
            DialogueManager.Instance.GetTriggered(dialogue);
            dialogueIndex++;
            if (dialogueIndex >= batch.dialogueList.Count && batch.fallbackDialogue == null) gameObject.tag = "Untagged"; // remove trigger if there are no dialogues and no fallback dialogue left
            return dialogue;
        }
        else
        {
            Dialogue dialogue = batch.fallbackDialogue;
            DialogueManager.Instance.GetTriggered(dialogue);
            return dialogue;
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
        if (dialogueIndex >= batch.dialogueList.Count && batch.fallbackDialogue == null) gameObject.tag = "Untagged";
        else gameObject.tag = "DialogueTrigger";
    }

    public void SetBatch(DialogueBatch batch)
    {
        if (!batchesToIds.ContainsKey(batch)) throw new ArgumentException("Unknown dialogue batch");
        SetBatchIndex(batchesToIds[batch]);
    }

    public int GetDialogueIndex()
    {
        return dialogueIndex;
    }

    public void SetDialogueIndex(int index)
    {
        batchIndex = index;
        if (dialogueIndex >= batch.dialogueList.Count && batch.fallbackDialogue == null) gameObject.tag = "Untagged";
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

    public DialogueTriggerSerializedData Serialize()
    {
        return new DialogueTriggerSerializedData(batchIndex, dialogueIndex);
    }

    public void Deserialize(DialogueTriggerSerializedData data)
    {
        SetBatchIndex(data.dialogueBatchIndex);
        SetDialogueIndex(data.dialogueIndex);
    }
}
