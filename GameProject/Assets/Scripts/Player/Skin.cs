using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skin", menuName = "Skin")]
public class Skin : ScriptableObject
{
    [field: SerializeField] public Sprite Sprite { get; private set; }
    // Availability conditions based on string values from PlayerPrefs. If keys in PlayerPrefs with key names from all indices have values at corresponding indices, skin is available
    [field: SerializeField] public List<string> conditionKeyNames { get; private set; }

    public bool IsAvailable()
    {
        for (int i = 0; i < conditionKeyNames.Count; i++) if (!PlayerPrefs.GetString(conditionKeyNames[i], "False").Equals("True")) return false;
        return true;
    }
}
