// Data that is serialized on an NPC-by-NPC basis and includes dynamic information about them, such as batch dialogue state index and position
using System.Collections.Generic;

[System.Serializable]
public class NPCSerializedData
{
    public bool active;
    public DialogueTriggerSerializedData dialogueTrigger;
    public float x, y, z; // position of the NPC in the world (rotation and scale are not dynamic)
    
    public NPCSerializedData(bool active, DialogueTriggerSerializedData dialogueTrigger, float x, float y, float z)
    {
        this.active = active;
        this.dialogueTrigger = dialogueTrigger;
        this.x = x;
        this.y = y;
        this.z = z;
    }
}
