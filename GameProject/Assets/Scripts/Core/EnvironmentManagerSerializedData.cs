using System;
using System.Collections.Generic;

[System.Serializable]
public class EnvironmentManagerSerializedData
{
    [Serializable]
    public struct SerListWrapper<T>
    {
        public List<T> list;
    }
    public List<int> activeSparks; // list of indices of active spark objects (this includes black squares)
    public List<int> activeObjects; // lists of indices of any other GameObjects that are active (among ones that exist dynamically)
    public List<NPCSerializedData> npcData; // state data for all NPCs
    public List<int> letterTouchedLists;

    public EnvironmentManagerSerializedData(List<int> activeSparks, List<int> activeObjects, List<NPCSerializedData> npcData, List<int> letterTouchedLists)
    {
        this.activeSparks = activeSparks;
        this.activeObjects = activeObjects;
        this.npcData = npcData;
        this.letterTouchedLists = letterTouchedLists;
    }
}
