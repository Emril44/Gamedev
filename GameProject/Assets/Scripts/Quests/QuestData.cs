using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quest", menuName = "Quest")]
public class QuestData : ScriptableObject
{
    // Display title of the quest in quest menu
    [field: SerializeField] public string Title { get; private set; }
    // Description in quest menu
    [field: SerializeField] public string Description { get; private set; }
    // Sequential list of objectives. The first objective (Element 0) is considered an unlock objective - quest will only show up after its completion
    [field: SerializeField] public List<Objective> Objectives { get; private set; }

}