using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class CutsceneEvent
{
    public enum EventType
    {
        None,
        Dialogue,
        CharacterMove,
        CharacterJump,
        Wait,
        SetActivity,
        SetPosition,
        UnlockColor,
        FadeOut,
        FadeIn,
        DisplayText
    }

    public EventType Type { get { return type;  } }
    [SerializeField] private EventType type;

    [SerializeField] private Dialogue dialogue;
    [SerializeField] private MobileCharacter character;
    [SerializeField] private Transform targetLocation;
    [SerializeField] private float velocity;
    [SerializeField] private string displayText;
    [SerializeField] private float time;
    [SerializeField] private GameObject target;
    [SerializeField] private bool setTargetToActive = true;
    [SerializeField] private GameObject[] targets; // for activity management, because it can be reasonably done in bulk

    public IEnumerator Run()
    {
        switch(Type)
        {
            case EventType.Dialogue:
                yield return RunDialogue(dialogue);
                break;
            case EventType.CharacterMove:
                yield return character.MoveTo(targetLocation.transform.position, velocity);
                break;
            case EventType.CharacterJump:
                character.Jump(velocity);
                yield return null;
                break;
            case EventType.Wait:
                yield return new WaitForSeconds(time);
                break;
            case EventType.SetActivity:
                foreach(GameObject t in targets) t.SetActive(setTargetToActive);
                yield return null;
                break;
            case EventType.SetPosition:
                target.transform.position = targetLocation.position;
                break;
            case EventType.UnlockColor:
                DataManager.Instance.UnlockColor();
                break;
            case EventType.FadeIn:
                yield return null; // TODO when effect is available
                break;
            case EventType.FadeOut:
                yield return null; // TODO when effect is available
                break;
            case EventType.DisplayText:
                yield return null; // TODO
                break;
            default:
                yield return null;
                break;
        }

    }

    private IEnumerator RunDialogue(Dialogue dialogue)
    {
        bool dialogueEnded = false;
        void ReactToEnd()
        {
            dialogueEnded = true;
            dialogue.onDialogueEnd -= ReactToEnd;
        }
        dialogue.onDialogueEnd += ReactToEnd;
        DialogueManager.Instance.GetTriggered(dialogue);
        yield return new WaitUntil(() => { return dialogueEnded; });

    }
}