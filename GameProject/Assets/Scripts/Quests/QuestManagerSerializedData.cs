using System.Collections.Generic;

[System.Serializable]
public class QuestManagerSerializedData
{
    public List<int> completedQuests; // QuestManager indices of completed quests
    public List<int> availableQuests; // QuestManager indices of available quests
    public List<int> availableQuestObjectiveIDs; // Objective indices in available quests, with index at i in this list corresponding to quest at i in availableQuests list

    public QuestManagerSerializedData(List<int> completedQuests, List<int> availableQuests, List<int> availableQuestObjectiveIDs)
    {
        this.completedQuests = completedQuests;
        this.availableQuests = availableQuests;
        this.availableQuestObjectiveIDs = availableQuestObjectiveIDs;
    }
}
