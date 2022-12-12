// Data that is serialized on an NPC-by-NPC basis and includes dynamic information about them, such as batch dialogue state index and position
using System.Collections.Generic;

[System.Serializable]
public class NPCSerializedData
{
    public bool active;
    public int dialogueBatchIndex; // index of the predefined dialogue collection that is the basis of the current dialogue set
    public int dialogueIndex; // current dialogue index in the batch
    public float x, y, z; // position of the NPC in the world (rotation and scale are not dynamic)]
    // any freeform information that NPCs may require; saves system is not responsible for how this information interpreted
    public List<string> tagNames;
    public List<string> tagValues;
    
    public NPCSerializedData(bool active, int dialogueBatchIndex, int dialogueIndex, float x, float y, float z, List<string> tagNames, List<string> tagValues)
    {
        this.active = active;
        this.dialogueBatchIndex = dialogueBatchIndex;
        this.dialogueIndex = dialogueIndex;
        this.x = x;
        this.y = y;
        this.z = z;
        this.tagNames = tagNames;
        this.tagValues = tagValues;
    }
}
