using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quest", menuName = "Quest")]
public class QuestData : ScriptableObject
{
    // Display title of the quest in quest menu
    [field: TextArea(1, 5)]
    [field: SerializeField] public string Title { get; private set; }
    // Description in quest menu
    [field: TextArea(1, 5)]
    [field: SerializeField] public string Description { get; private set; }
    // Sequential list of objectives. The first objective (Element 0) is considered an unlock objective - quest will only show up after its completion
    [field: SerializeField] public List<Objective> Objectives { get; private set; }
    // Quests that need to be completed before this one (optional). If prerequisites are not completed at the moment the quest attempts to become available, it will not become available
    [field: SerializeField] public List<QuestData> Prerequisites { get; private set; }
    // Quests that will try to become available (not active) after this one is completed. Quests whose prerequisite quests are not completed will not become available
    [field: SerializeField] public List<QuestData> NextQuests { get; private set; }

}