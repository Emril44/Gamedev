using System.Collections;
using UnityEngine;

// Container class for a list of sequenced events that happen without player's control (except for choices in dialogues)
public class Cutscene : MonoBehaviour
{
    [SerializeField] private bool skipAhead; // in development purposes only
    [SerializeField] private MobileCharacter[] NPCParticipants;
    [SerializeField] private CutsceneEvent[] events;

    private void Start()
    {
        if (events.Length == 0)
        {
            Debug.LogWarning("A cutscene is not populated with events, so nothing will happen when it is played");
            events = new CutsceneEvent[1];
            events[0] = new CutsceneEvent();
        }
    }
    public IEnumerator Play()
    {
        if (skipAhead)
        {
            foreach (CutsceneEvent cutsceneEvent in events)
            {
                cutsceneEvent.Skip();
            }
            yield return null;
        }
        else
        {
            EnvironmentManager.Instance.NotifyCutscenePlaying(true);
            bool oldControllable = PlayerInteraction.Instance.Controllable;
            PlayerInteraction.Instance.Controllable = false;
            foreach (MobileCharacter character in NPCParticipants)
            {
                character.SetMobile(true);
            }
            foreach (CutsceneEvent cutsceneEvent in events)
            {
                yield return cutsceneEvent.Run();
            }
            foreach (MobileCharacter character in NPCParticipants)
            {
                character.SetMobile(false);
            }
            PlayerInteraction.Instance.Controllable = oldControllable;
            EnvironmentManager.Instance.NotifyCutscenePlaying(false);
        }
    }
}
