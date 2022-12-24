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
                yield return null;
                break;
            case EventType.UnlockColor:
                DataManager.Instance.UnlockColor();
                EnvironmentManager.Instance.ActivateColoredObjects();
                yield return null;
                break;
            case EventType.FadeIn:
                yield return ScreenFade.Instance.FadeIn(1);
                break;
            case EventType.FadeOut:
                yield return ScreenFade.Instance.FadeOut(1);
                break;
            case EventType.DisplayText:
                yield return DialogueManager.Instance.DisplayTextCoroutine(displayText, time);
                break;
            default:
                yield return null;
                break;
        }
    }

    public void Skip()
    {
        switch (Type)
        {
            case EventType.Dialogue:
                dialogue.Finish();
                break;
            case EventType.CharacterMove:
                character.transform.position = targetLocation.position;
                break;
            case EventType.SetActivity:
                foreach (GameObject t in targets) t.SetActive(setTargetToActive);
                break;
            case EventType.SetPosition:
                target.transform.position = targetLocation.position;
                break;
            case EventType.UnlockColor:
                DataManager.Instance.UnlockColor();
                EnvironmentManager.Instance.ActivateColoredObjects();
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