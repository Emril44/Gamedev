using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Stores dynamic macro information about quest progress
public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; }
    // Reliable links to all quests. Quests are identified by their indices in this list, so indices should not change
    [SerializeField] private List<Quest> quests = new List<Quest>();
    private Dictionary<QuestData, int> questsToIds = new Dictionary<QuestData, int>();
    // indices of completed quests
    private List<int> completedQuests = new List<int>();
    // Indices of available quests, i.e. quests for which objectives are being tracked (an available quest is not necessarily active in quest menu)
    private List<int> availableQuests = new List<int>();
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        for (int i = 0; i < quests.Count; i++) questsToIds.Add(quests.ElementAt(i).GetData(), i); // backwards-map QuestData instances to ids of quests
    }

    public QuestManagerSerializedData Serialize()
    {
        List<int> objectiveIds = new List<int>();
        for (int i = 0; i < availableQuests.Count; i++) objectiveIds.Add(QuestById(availableQuests[i]).GetCurrentObjectiveIndex()); // add ids in the same order as their corresponding objectives
        return new QuestManagerSerializedData(completedQuests, availableQuests, objectiveIds);
    }

    public void Deserialize(QuestManagerSerializedData data)
    {
        completedQuests = data.completedQuests;
        availableQuests = data.availableQuests;
        foreach (Quest quest in quests) quest.gameObject.SetActive(false);
        for (int i = 0; i < availableQuests.Count; i++)
        {
            Quest quest = QuestById(availableQuests[i]);
            quest.SetCurrentObjectiveIndex(data.availableQuestObjectiveIDs[i]);
            quest.gameObject.SetActive(true);
        }
    }

    // Makes a quest available (not yet active in the quest menu)
    public void AddAvailableQuest(int id)
    {
        Quest quest = QuestById(id);
        if (!availableQuests.Contains(id))
        {
            quest.gameObject.SetActive(true);
            availableQuests.Add(id);
            void CompleteQuest()
            {
                completedQuests.Add(id);
                quest.onComplete -= CompleteQuest;
                // add all direct next quests (whose prerequisites are met)
                foreach (QuestData data in quest.GetData().NextQuests)
                {
                    bool legal = true;
                    foreach (QuestData req in data.Prerequisites) {
                        if (!IsQuestCompleted(req))
                        {
                            legal = false;
                            break;
                        }
                    }
                    if (legal) AddAvailableQuest(IdByQuestData(data));
                }
            }
            quest.onComplete += CompleteQuest;
        }
    }
    
    public void AddAvailableQuest(QuestData data)
    {
        AddAvailableQuest(IdByQuestData(data));
    }

    // Make quest unavailable without completing it (e.g. if the day ended)
    public void RemoveAvailableQuest(int id)
    {
        Quest quest = QuestById(id);
        quest.ResetEvents();
        availableQuests.Remove(id);
        quest.gameObject.SetActive(false);
    }
    public void RemoveAvailableQuest(QuestData data)
    {
        RemoveAvailableQuest(IdByQuestData(data));
    }

    private Quest QuestById(int id)
    {
        if (id < 0 || id > quests.Count) return null;
        return quests.ElementAt(id);
    }

    private int IdByQuestData(QuestData questData)
    {
        return questsToIds[questData];
    }

    public bool IsQuestCompleted(int id)
    {
        return completedQuests.Contains(id);
    }
    public bool IsQuestCompleted(QuestData questData)
    {
        return completedQuests.Contains(IdByQuestData(questData));
    }

    public List<QuestData> GetCompletedQuestsData()
    {
        List<QuestData> list = new List<QuestData>();
        foreach (int q in completedQuests) list.Add(quests.ElementAt(q).GetData());
        return list;
    }

    public List<Quest> GetAvailableQuests()
    {
        List<Quest> list = new List<Quest>();
        foreach (int q in availableQuests) list.Add(quests.ElementAt(q));
        return list;
    }

    public List<Quest> GetActiveQuests()
    {
        List<Quest> list = new List<Quest>();
        foreach (int q in availableQuests)
        {
            Quest quest = quests.ElementAt(q);
            if (quest.IsActive()) list.Add(quest);
        }
        return list;
    }
}
