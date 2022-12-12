using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Dialogue Batch", menuName = "Dialogue Batch")]
public class DialogueBatch : ScriptableObject
{
    [field: SerializeField] public List<Dialogue> dialogueList { get; private set; }
    [field: SerializeField] public Dialogue fallbackDialogue { get; private set; }
}
