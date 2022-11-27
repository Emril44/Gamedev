using UnityEngine;

// linear dialogue phrases sequence, spoken by one character
[CreateAssetMenu(fileName = "New Dialogue Text Sequence", menuName = "Dialogue Text Sequence")]
public class DialogueText : DialogueNode
{
    [Header("Phrase sequence")]
    public CharacterName.Character character;
    [TextArea(1, 5)]
    public string[] text;
    public DialogueNode next;

    //for reference
    public DialogueText(string[] text, DialogueNode next)
    {
        this.text = text;
        this.next = next;
    }
}
