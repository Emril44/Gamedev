using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private Dialogue dialogue; 
    
    public void TriggerDialogue(GameObject NPC)
    {
        DialogueManager.Instance.GetTriggered(NPC, this, dialogue);
    }

    public void OnDialogueFinished()
    {
        //...
    }
}
