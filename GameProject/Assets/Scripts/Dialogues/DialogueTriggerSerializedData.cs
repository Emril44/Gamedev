// Data that is serialized on an NPC-by-NPC basis and includes dynamic information about them, such as batch dialogue state index and position
[System.Serializable]
public class DialogueTriggerSerializedData
{
    public int dialogueBatchIndex; // index of the predefined dialogue collection that is the basis of the current dialogue set
    public int dialogueIndex; // current dialogue index in the batch
    
    public DialogueTriggerSerializedData(int dialogueBatchIndex, int dialogueIndex)
    {
        this.dialogueBatchIndex = dialogueBatchIndex;
        this.dialogueIndex = dialogueIndex;
    }

}
