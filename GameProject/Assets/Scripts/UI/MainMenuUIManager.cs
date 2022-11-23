using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenuUIManager : MonoBehaviour
{
    [SerializeField] private GameObject block;
    private GameObject blockInstance;
    [SerializeField] private GameObject yesNo;
    private GameObject yesNoInstance;
    [SerializeField] private Canvas canvas;
    [SerializeField] private Button loadButton;
    [SerializeField] private GameObject languageSettings;
    private const int LANGUAGES = 2;
    [Header("Saves")]
    [SerializeField] private GameObject line;
    [SerializeField] private GameObject save;
    [SerializeField] private GameObject newSave;
    [Header("SavesLocalization")]
    [SerializeField] private string autosaveString;
    [SerializeField] private string saveString;
    [SerializeField] private string dayString;

    public static MainMenuUIManager Instance { get; private set; }
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
        blockInstance = Instantiate(block);
        blockInstance.SetActive(false);
        blockInstance.transform.SetParent(canvas.transform, false);
        var x = blockInstance.transform.GetChild(1).GetChild(0).gameObject;
        x.GetComponent<Button>().onClick.AddListener(() => { RemoveBlock(); });
        if(SavesManager.Instance.HasSaves())
        {
            loadButton.interactable = true;
        }
        else
        {
            loadButton.interactable = false;
        }
        yesNoInstance = Instantiate(yesNo);
        yesNoInstance.SetActive(false);
        yesNoInstance.transform.SetParent(canvas.transform, false);
        var no = yesNoInstance.transform.GetChild(1).GetChild(2).gameObject;
        Debug.Log(no.name);
        no.GetComponent<Button>().onClick.AddListener(() => { RemoveYesNo(); });
    }

    public void ChangeLanguage()
    {
        int current = PlayerPrefs.GetInt("Language");
        current++;
        if (current >= LANGUAGES)
        {
            current = 0;
        }
        PlayerPrefs.SetInt("Language", current);
        foreach (LocalizedText localizedText in canvas.transform.GetComponentsInChildren<LocalizedText>())
        {
            localizedText.Awake();
        }
    }

    public void RemoveBlock()
    {
        StopAllCoroutines();
        var blackout = blockInstance.transform.GetChild(0).gameObject;
        var block = blockInstance.transform.GetChild(1).gameObject;
        StartCoroutine(FadeOut(blackout, block));
    }
    
    public void ShowBlock()
    {
        StopAllCoroutines();
        var blackout = blockInstance.transform.GetChild(0).gameObject;
        var block = blockInstance.transform.GetChild(1).gameObject;
        block.transform.position = new Vector3(-1.65f, 9);
        blockInstance.SetActive(true);
        StartCoroutine(FadeIn(blackout, block));
    }
    
    IEnumerator FadeOut(GameObject blackout, GameObject block)
    {
        float time = 0;
        while (time < 0.6f)
        {
            time += Time.deltaTime;
            blackout.GetComponent<Image>().color = new Color(0, 0, 0, blackout.GetComponent<Image>().color.a - Time.deltaTime);

            block.transform.position = Vector3.Lerp(block.transform.position, new Vector2(block.transform.position.x, -30), Time.deltaTime);
            yield return null;
        }
        blockInstance.SetActive(false);
        yield return null;
    }

    IEnumerator FadeIn(GameObject blackout, GameObject block)
    {
        float time = 0;
        while (time < 2)
        {
            time += Time.deltaTime;
            blackout.GetComponent<Image>().color = new Color(0, 0, 0, time/6);

            block.transform.position = Vector3.Lerp(block.transform.position, new Vector2(block.transform.position.x, -0.06f), Time.deltaTime*2.15f);
            yield return null;
        }
    }
    
    public void ShowNewGameScreen() 
    {
        if (blockInstance.transform.GetChild(1).childCount > 1)
        {
            Destroy(blockInstance.transform.GetChild(1).GetChild(1).gameObject);
        }
        GameObject s = new GameObject();
        s.transform.SetParent(blockInstance.transform.GetChild(1), false);
        s.transform.localPosition = new Vector3(4.8f, 31.6f, -14);
        s.transform.localScale = new Vector3(0.06f, 0.06f, 0.06f);
        var save = SavesManager.Instance.Autosave();
        if(save != null)
        {
            var card = SaveCard(autosaveString, save, s.transform, 0);
            card.GetComponent<Button>().onClick.AddListener(() => { Debug.Log("autosave"); }); //load
            card.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() => { Debug.Log("X"); }); //yesNo delete
        }
        var l = Instantiate(line, s.transform);
        l.transform.localPosition = l.transform.localPosition + new Vector3(0, -375);
        int i = SavesManager.Instance.Saves().Length;
        for(int j = 0; j < i; j++)
        {
            if(SavesManager.Instance.Saves()[j] != null)
            {
                var card = SaveCard(saveString, SavesManager.Instance.Saves()[j], s.transform, j + 1);
                card.transform.localPosition = card.transform.localPosition + new Vector3(0, -338 * (j + 1));
                if (j == 0)
                {
                    card.transform.localPosition = card.transform.localPosition + new Vector3(0, -20);
                }
                card.GetComponent<Button>().onClick.AddListener(() => { });
            }
            else
            {
                var card = Instantiate(newSave, s.transform);
                card.transform.localPosition = card.transform.localPosition + new Vector3(-246, -338 * (j+1) + 415);
                card.GetComponent<Button>().onClick.AddListener(() => { });
            }
        }
        ShowBlock();
    }

    public GameObject SaveCard(string title, Save saveInfo, Transform parent, int n)
    {
        GameObject save = Instantiate(this.save, parent.transform);
        string t = title.Split('+')[PlayerPrefs.GetInt("Language")];
        save.GetComponent<SaveTexts>().saveNum.text = t + (n > 0 ? " #" + n : "");

        string day = dayString.Split('+')[PlayerPrefs.GetInt("Language")];
        save.GetComponent<SaveTexts>().day.text = day + ": " + saveInfo.day;

        save.GetComponent<SaveTexts>().sparksAmount.text = saveInfo.sparks.ToString();
        save.GetComponent<SaveTexts>().time.text = saveInfo.time / 60 + ":" + (((saveInfo.time % 60) < 10) ? "0" + saveInfo.time % 60 : saveInfo.time % 60);

        save.GetComponent<Button>().onClick.AddListener(() => { SetYesNo("Overwrite this save", () => { SavesManager.Instance.Load(n); }) ; });
        var x = save.transform.GetChild(1).gameObject;
        x.GetComponent<Button>().onClick.AddListener(() => { SetYesNo("Delete this save", () => { SavesManager.Instance.RemoveSave(n); }); });
        return save;
    }

    public void SetYesNo(string message, UnityEngine.Events.UnityAction onYes)
    {
        blockInstance.transform.GetChild(0).gameObject.SetActive(false);
        yesNoInstance.SetActive(true);
        yesNoInstance.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = message;
        yesNoInstance.transform.GetChild(1).GetChild(1).GetComponent<Button>().onClick.AddListener(onYes);
    }

    public void ShowLoadScreen() 
    {
        //...
    }

    public void ShowSettingsScreen()
    {
        if (blockInstance.transform.GetChild(1).childCount > 1)
        {
            Destroy(blockInstance.transform.GetChild(1).GetChild(1).gameObject);
        }
        var lengSettings = Instantiate(languageSettings);
        lengSettings.transform.SetParent(blockInstance.transform.GetChild(1), false);
        lengSettings.transform.localPosition = new Vector3(21.5f, 21, 446.5f);
        lengSettings.transform.localScale = new Vector3(0.05725f, 0.05725f, 0.05725f);
        lengSettings.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() => { ChangeLanguage(); });
        ShowBlock();
    }

    public void ShowAboutScreen()
    {
        if (blockInstance.transform.GetChild(1).childCount > 1)
        {
            Destroy(blockInstance.transform.GetChild(1).GetChild(1).gameObject);
        }
        ShowBlock();
    }

    private void RemoveYesNo()
    {
        blockInstance.transform.GetChild(0).gameObject.SetActive(true);
        yesNoInstance.SetActive(false);
    }

    public void ExitGame()
    {
        //ShowYesNo
        Application.Quit();
    }
}
