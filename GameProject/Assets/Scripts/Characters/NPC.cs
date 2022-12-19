using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DialogueTrigger))]
public class NPC : MonoBehaviour
{
    protected Dictionary<string, string> tags;
    protected DialogueTrigger trigger;

    protected virtual void Awake()
    {
        tags = new Dictionary<string, string>();
        trigger = GetComponent<DialogueTrigger>();
    }

    public void SetDialogueBatch(int i)
    {
        trigger.SetBatchIndex(i);
    }

    public void SetDialogueBatch(DialogueBatch batch)
    {
        trigger.SetBatch(batch);
    }

    public Dialogue TriggerDialogue()
    {
        return trigger.TriggerDialogue();
    }

    public NPCSerializedData Serialize()
    {
        List<string> tagNames = new List<string>();
        List<string> tagValues = new List<string>();
        foreach (string key in tags.Keys)
        {
            tagNames.Add(key);
            tagValues.Add(tags[key]);
        }
        return new NPCSerializedData(gameObject.activeSelf, trigger.GetBatchIndex(), trigger.GetDialogueIndex(), transform.position.x, transform.position.y, transform.position.z, tagNames, tagValues);
    }

    public void Deserialize(NPCSerializedData data)
    {
        gameObject.SetActive(data.active);
        trigger.SetBatchIndex(data.dialogueBatchIndex);
        trigger.SetDialogueIndex(data.dialogueIndex);
        transform.position = new Vector3(data.x, data.y, data.z);
        tags.Clear();
        for (int i = 0; i < data.tagNames.Count; i++) tags[data.tagNames[i]] = data.tagValues[i];
    }
}
