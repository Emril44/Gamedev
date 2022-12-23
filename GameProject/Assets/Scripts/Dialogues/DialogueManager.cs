using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    private Dialogue dialogue;
    private DialogueNode currentNode;

    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private GameObject textWithName;
        private TextMeshProUGUI nameText;
        private TextMeshProUGUI dialogueText;
    [SerializeField] private GameObject textWithoutName;
        private TextMeshProUGUI onlyText;
    [SerializeField] private GameObject dialogueOption;

    private GameObject[] dialogOptions;
    private int phraseCounter;
    private bool animatingText = false;
    private TextMeshProUGUI animatedTMP;
    private string animatedText;

    public static DialogueManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        dialogOptions = new GameObject[5];
        for (int i = 0; i < 5; i++)
        {
            dialogOptions[i] = Instantiate(dialogueOption, dialogueBox.transform);
            dialogOptions[i].SetActive(false);
        }
        nameText = textWithName.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        dialogueText = textWithName.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        onlyText = textWithoutName.GetComponent<TextMeshProUGUI>();
        dialogueText.text = "";
    }
    
    public void GetTriggered(Dialogue dialogue)
    {
        this.dialogue = dialogue;
        currentNode = dialogue.firstNode;
        if (currentNode == null)
        {
            return;
        }
        phraseCounter = 0;
        dialogueBox.SetActive(true);
        GetNext();
    }

    void Update()
    {
        if (dialogueBox.activeSelf)
        {
            if (animatingText && Input.GetMouseButtonDown(0))
            {
                StopAllCoroutines();
                ShowAnimatedText();
            }
            else if (Input.GetMouseButtonDown(0) && !currentNode.IsOption())
            {
                GetNext();
            }
        }
    }
    
    private void GetNext()
    {
        if (currentNode == null)
        {
            dialogueBox.SetActive(false);
            dialogue.Finish();
            dialogue = null;
            currentNode = null;
            dialogueText.text = "";
            return;
        }
        if (currentNode.IsOption())
        {
            dialogueText.text = "";
            textWithoutName.SetActive(false);
            textWithName.SetActive(false);
            AddButtons(GetLocalizedOptions(((DialogueOption)currentNode).options, currentNode.separator));
        }
        else
        {
            DialogueText textNode = (DialogueText)currentNode;
            if (phraseCounter < textNode.text.Length)
            {
                if((int)textNode.character == -1)
                {
                    textWithoutName.SetActive(true);
                    textWithName.SetActive(false);
                    AddOnlyText(GetLocalizedText(textNode.text[phraseCounter], currentNode.separator));
                }
                else
                {
                    textWithoutName.SetActive(false);
                    textWithName.SetActive(true);
                    AddTextWithName(textNode, GetLocalizedText(textNode.text[phraseCounter], currentNode.separator));
                }
            }
            else
            {
                currentNode = textNode.next;
                phraseCounter = 0;
                GetNext();
            }
        }
    }

    void ShowAnimatedText()
    {
        animatedTMP.text = animatedText;
        animatingText = false;
    }

    void AnimateText(TextMeshProUGUI tmp, string text)
    {
        animatingText = true;
        animatedTMP = tmp;
        animatedText = text;
        StartCoroutine(AnimateTextCoroutine(tmp, text));
    }
    
    IEnumerator AnimateTextCoroutine(TextMeshProUGUI tmp, string text)
    {
        tmp.text = "";
        for (int i = 0; i < text.Length; i++)
        {
            tmp.text += text[i];
            yield return new WaitForSeconds(0.03f);
        }
        animatingText = false;
    }
    
    private void AddTextWithName(DialogueText node, string text)
    {
        phraseCounter++;
        nameText.text = CharacterName.GetLocalizedCharachterName(node.character);
        AnimateText(dialogueText, text);
    }
    public void AddOnlyText(string text)
    {
        phraseCounter++;
        AnimateText(onlyText, text);
    }

    private string GetLocalizedText(string text, char separator)
    {
        if (separator == ' ' || separator == 0)
        {
            separator = '+';
        }
        string[] texts = text.Split(separator);
        return texts[PlayerPrefs.GetInt("Language")];
    }

    private string[] GetLocalizedOptions(DialogueOption.Option[] options, char separator)
    {
        if (separator == ' ')
        {
            separator = '+';
        }
        string[] localizedOptions = new string[options.Length];
        for (int i = 0; i < options.Length; i++)
        {
            localizedOptions[i] = GetLocalizedText(options[i].option, separator);
        }
        return localizedOptions;
    }

    public void AddButtons(string[] options)
    {
        if (options.Length > 5)
        {
            Debug.LogError("Too many options");
        }
        dialogueText.enabled = false;
        SetOptionsPosition(options.Length);
        for (int i = 0; i < options.Length; i++)
        {
            dialogOptions[i].SetActive(true);
            dialogOptions[i].GetComponentInChildren<TMP_Text>().text = options[i];
            int j = i;
            dialogOptions[i].GetComponent<Button>().onClick.RemoveAllListeners();
            dialogOptions[i].GetComponent<Button>().onClick.AddListener(() => GetButtonRes(j));
        }
    }

    public void SetOptionsPosition(int amount)
    {
        int offset = 225;
        if(amount%2 == 0)
        {
            for (int i = 0; i < amount; i++)
            {
                dialogOptions[i].transform.localPosition = new Vector3(dialogOptions[i].transform.localPosition.x, -(float)offset/2 - (amount/2 - 1)*offset + i*offset, dialogOptions[i].transform.localPosition.z);
            }
        }
        else
        {
            for (int i = 0; i < amount; i++)
            {
                dialogOptions[i].transform.localPosition = new Vector3(dialogOptions[i].transform.localPosition.x, -amount/2*offset + offset*i, dialogOptions[i].transform.localPosition.z);
            }
        }
    }
    
    public void GetButtonRes(int i)
    {
        dialogueText.enabled = true;
        foreach (var option in dialogOptions)
        {
            if (option != null)
            {
                option.SetActive(false);
            }
        }
        if (!currentNode.IsOption())
        {
            Debug.LogError("It should not be called as it is not an option");
        }
        if (currentNode.IsOption())
        {
            currentNode = ((DialogueOption)currentNode).options[i].optionBranch;
            GetNext();
        } 
    }
}
