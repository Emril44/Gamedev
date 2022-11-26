using System;
using UnityEngine;


// dialogue options block for pseudo non-linear dialogue
[CreateAssetMenu(fileName = "New Dialogue Option Block", menuName = "Dialogue Option Block")]
public class DialogueOption : DialogueNode
{
    [Header("Options dialogue block")]
    public Option[] options;

    //for reference
    public DialogueOption(string[] options, DialogueNode[] optionsBranches)
    {
        this.options = new Option[options.Length];
        for (int i = 0; i < options.Length; i++)
        {
            this.options[i] = new Option(options[i], optionsBranches[i]);
        }
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
