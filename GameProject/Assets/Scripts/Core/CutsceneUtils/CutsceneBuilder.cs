using System;
using System.Collections;
using UnityEngine;

// Collection of common methods for programming cutscenes
public class CutsceneBuilder
{

    // Waits for dialogue completion
    public static IEnumerator WaitForDialogue(Dialogue dialogue)
    {
        bool dialogueEnded = false;
        void ReactToEnd()
        {
            dialogueEnded = true;
            dialogue.onDialogueEnd -= ReactToEnd;
        }
        dialogue.onDialogueEnd += ReactToEnd;
        yield return new WaitUntil(() => { return dialogueEnded; });
    }

    // Triggers a DialogueTrigger's current dialogue and waits for completion
    public static IEnumerator WaitForDialogue(NPC npc)
    {
        yield return WaitForDialogue(npc.TriggerDialogue());
    }

    public static IEnumerator MoveTo(IMobileCharacter character, MoveData move)
    {
        yield return character.MoveTo(move.target.transform.position, move.velocity);
    }

    public static IEnumerator PerformDuringBlackout(Action action)
    {
        yield return null; // placeholder
        // call scene blackout effect (when implemented)
        action.Invoke();
        // call reverse of blackout
    }
}
