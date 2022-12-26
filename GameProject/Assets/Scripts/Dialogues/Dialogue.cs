using System;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue")]
public class Dialogue : ScriptableObject
{
    public event Action onDialogueEnd;

    [field: SerializeField] public DialogueNode firstNode { get; private set; }
    private void Awake()
    {
        SceneManager.sceneLoaded += (Scene scene, LoadSceneMode mode) =>
        {
            onDialogueEnd = null;
        };
    }

    public void Finish()
    {
        onDialogueEnd?.Invoke();
    }
}
