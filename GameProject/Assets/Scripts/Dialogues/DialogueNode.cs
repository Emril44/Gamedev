using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue Node", menuName = "Dialogue Node")]
//either dialogueOptions menu or dialogue text array
public class DialogueNode : ScriptableObject
{
    [Header("Options dialogue block")]
    public string[] options;
    public DialogueNode[] optionsBranches;
    [Space(50)]
    [Header("Dialogue branch")]
    public string[] text;
    public DialogueNode next;

    public bool IsOption()
    {
        return options != null && options.Length > 0;
    }

    //for reference
    public DialogueNode(string[] options, DialogueNode[] optionsBranches)
    {
        this.options = options;
        this.optionsBranches = optionsBranches;
    }

    public DialogueNode(string[] text, DialogueNode next)
    {
        this.text = text;
        this.next = next;
    }
}
