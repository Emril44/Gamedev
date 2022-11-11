using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    private GameObject player;
    private Dialogue dialogue;
    private DialogueNode firstNode;

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
        phraseCounter = 0;
        dialogueCanvas.SetActive(true);
        this.dialogue = Instantiate(dialogue);
        firstNode = dialogue.GetCurrent();
        var node = dialogue.GetCurrent();
        if (node == null)
        {
            return;
        }
        if (node.IsOption())
        {
            AddButtons(node.options);
        }
        else
        {
            AddText(node.text[phraseCounter]);
        }
    }

    void Update()
    {
        if (dialogueCanvas.activeSelf)
        {
            if (Input.GetMouseButtonDown(0) && !dialogue.GetCurrent().IsOption())
            {
                GetNext();
            }
        }
    }
    
    private void GetNext()
    {
        var node = dialogue.GetCurrent();
        if (node == null)
        {
            dialogueCanvas.SetActive(false);
            dialogue.Finish();
            dialogue = null;
            firstNode = null;
            dialogueText.text = "";
            return;
        }
        if (node.IsOption())
        {
            dialogueText.text = "";
            AddButtons(node.options);
        }
        else
        {
            if (phraseCounter < node.text.Length)
            {
                AddText(node.text[phraseCounter]);
            }
            else
            {
                dialogue.GetNext();
                phraseCounter = 0;
                GetNext();
            }
        }
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
        foreach (var option in dialogOptions)
        {
            if (option != null)
            {
                option.SetActive(false);
            }
        }
        if (!dialogue.GetCurrent().IsOption())
        {
            Debug.LogError("It should not be called as it is not an option");
        }
        if (dialogue.GetCurrent().IsOption())
        {
            dialogue.GetNext(i);
            GetNext();
        } 
    }
}
