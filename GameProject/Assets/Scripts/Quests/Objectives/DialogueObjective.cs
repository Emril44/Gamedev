using UnityEngine;
// Objective whose completion is triggered by finishing a dialogue
[CreateAssetMenu(fileName = "New Dialogue Objective", menuName = "Dialogue Objective")]
public class DialogueObjective : Objective
{
    [SerializeField] private Dialogue dialogue;

    override public void SetActive(bool active)
    {
        if (active) dialogue.onDialogueEnd += Complete;
        else dialogue.onDialogueEnd -= Complete;
    }
}
