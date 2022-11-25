using UnityEngine;
using System;

[CreateAssetMenu(fileName = "New Dialogue Node", menuName = "Dialogue Node")]
//either dialogueOptions menu or dialogue text array
public class DialogueNode : ScriptableObject
{
    [Header("Basic separator is '+'")]
    public char separator = '+';
    [Header("Options dialogue block")]
    public Option[] options;
    [Space(40)]
    [Header("Dialogue branch")]
    [TextArea(1, 5)]
    public string[] text;
    public DialogueNode next;

    public bool IsOption()
    {
        return options != null && options.Length > 0;
    }

    //for reference
    public DialogueNode(string[] options, DialogueNode[] optionsBranches)
    {
        this.options = new Option[options.Length];
        for (int i = 0; i < options.Length; i++)
        {
            this.options[i] = new Option(options[i], optionsBranches[i]);
        }
    }

    public DialogueNode(string[] text, DialogueNode next)
    {
        this.text = text;
        this.next = next;
    }

    
    [Serializable]
    public class Option
    {
        [TextArea(1, 5)]
        public string option;
        public DialogueNode optionBranch;

        public Option(string option, DialogueNode optionBranch)
        {
            this.option = option;
            this.optionBranch = optionBranch;
        }
    }
}
