using UnityEngine;

// common parent class for dialogue options and linear dialogue texts
public abstract class DialogueNode : ScriptableObject
{
    [Header("Basic separator is '+'")]
    public char separator = '+';
    public bool IsOption()
    {
        return this.GetType() == typeof(DialogueOption);
    }
}
