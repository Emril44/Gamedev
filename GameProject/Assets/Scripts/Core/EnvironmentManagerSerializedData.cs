using System.Collections.Generic;

[System.Serializable]
public class EnvironmentManagerSerializedData
{
    public List<int> activeSparks; // list of indices of active spark objects (this includes black squares)
    public List<int> activeObjects; // lists of indices of any other GameObjects that are active (among ones that exist dynamically)
    public List<NPCSerializedData> npcData; // state data for all NPCs

    public EnvironmentManagerSerializedData(List<int> activeSparks, List<int> activeObjects, List<NPCSerializedData> npcData)
    {
        this.activeSparks = activeSparks;
        this.activeObjects = activeObjects;
        this.npcData = npcData;
    }
}
