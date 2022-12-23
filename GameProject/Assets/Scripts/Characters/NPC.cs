using UnityEngine;

// Wrapper class for serialization of both NPC's DialogueTrigger and the NPC's position itself
public class NPC : MonoBehaviour
{
    [SerializeField] protected DialogueTrigger trigger;

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
        return new NPCSerializedData(gameObject.activeSelf, trigger.Serialize(), transform.position.x, transform.position.y, transform.position.z);
    }

    public void Deserialize(NPCSerializedData data)
    {
        gameObject.SetActive(data.active);
        trigger.Deserialize(data.dialogueTrigger);
        transform.position = new Vector3(data.x, data.y, data.z);
    }
}
