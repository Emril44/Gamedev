using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [field: SerializeField] public Dialogue Dialogue { get; private set; } 
    
    public void TriggerDialogue(GameObject NPC)
    {
        DialogueManager.Instance.GetTriggered(NPC, Dialogue);
    }
}
