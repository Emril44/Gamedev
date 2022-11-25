using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    private GameObject player;
    private Dialogue dialogue;
    private DialogueNode currentNode;

    [SerializeField] private GameObject dialogueCanvas;
    
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private GameObject dialogueOption;

    private GameObject[] dialogOptions;
    private int phraseCounter;

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
        player = GameObject.FindGameObjectWithTag("Player");

        dialogueCanvas.SetActive(false);
        dialogOptions = new GameObject[5];
        for (int i = 0; i < 5; i++)
        {
            dialogOptions[i] = Instantiate(dialogueOption, dialogueCanvas.transform);
            dialogOptions[i].SetActive(false);
            dialogOptions[i].transform.localPosition += new Vector3(0, -50 * i);
        }
        dialogueText.text = "";
    }
    
    public void GetTriggered(GameObject NPC, Dialogue dialogue)
    {
        this.dialogue = dialogue;
        currentNode = dialogue.firstNode;
        if (currentNode == null)
        {
            return;
        }
        phraseCounter = 0;
        dialogueCanvas.SetActive(true);
        if (currentNode.IsOption())
        {
            AddButtons(GetLocalizedOptions(currentNode.options, currentNode.separator));
        }
        else
        {
            AddText(GetLocalizedText(currentNode.text[phraseCounter], currentNode.separator));
        }
    }

    void Update()
    {
        if (dialogueCanvas.activeSelf)
        {
            if (Input.GetMouseButtonDown(0) && !currentNode.IsOption())
            {
                GetNext();
            }
        }
    }
    
    private void GetNext()
    {
        if (currentNode == null)
        {
            dialogueCanvas.SetActive(false);
            dialogue.Finish();
            dialogue = null;
            currentNode = null;
            dialogueText.text = "";
            return;
        }
        if (currentNode.IsOption())
        {
            dialogueText.text = "";
            AddButtons(GetLocalizedOptions(currentNode.options, currentNode.separator));
        }
        else
        {
            if (phraseCounter < currentNode.text.Length)
            {
                AddText(GetLocalizedText(currentNode.text[phraseCounter], currentNode.separator));
            }
            else
            {
                currentNode = currentNode.next;
                phraseCounter = 0;
                GetNext();
            }
        }
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

    private string[] GetLocalizedOptions(DialogueNode.Option[] options, char separator)
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

    public void AddText(string text)
    {
        phraseCounter++;
        dialogueText.text = text;
    }

    public void AddButtons(string[] options)
    {
        if (options.Length > 5)
        {
            Debug.LogError("Too many options");
        }
        dialogueText.enabled = false;
        for (int i = 0; i < options.Length; i++)
        {
            dialogOptions[i].SetActive(true);
            dialogOptions[i].GetComponentInChildren<TMP_Text>().text = options[i];
            int j = i;
            dialogOptions[i].GetComponent<Button>().onClick.RemoveAllListeners();
            dialogOptions[i].GetComponent<Button>().onClick.AddListener(() => GetButtonRes(j));
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
            currentNode = currentNode.options[i].optionBranch;
            GetNext();
        } 
    }
}
