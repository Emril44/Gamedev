using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue")]
public class Dialogue : ScriptableObject
{
    [SerializeField] private DialogueNode currentNode;

    public DialogueNode GetCurrent()
    {
        return currentNode;
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
}
