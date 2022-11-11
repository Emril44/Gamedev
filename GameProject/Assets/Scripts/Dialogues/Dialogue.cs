using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue")]
public class Dialogue : ScriptableObject
{
    public event Action onDialogueEnd;

    [SerializeField] private DialogueNode currentNode;

    public DialogueNode GetCurrent()
    {
        return currentNode;
    }

    public void SetCurrent(DialogueNode node)
    {
        currentNode = node;
    }

    public DialogueNode GetNext()
    {
        currentNode = currentNode.next;
        return currentNode;
    }

    public DialogueNode GetNext(int i)
    {
        currentNode = currentNode.optionsBranches[i];
        return currentNode;
    }

    public void Finish()
    {
        onDialogueEnd?.Invoke();
    }
}
