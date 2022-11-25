using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelUIManager : MonoBehaviour
{
    public static LevelUIManager Instance { get; private set; }
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
        leverLookInstance = Instantiate(leverLook, canvasWorld.transform);
        leverLookInstance.SetActive(false);
    }

    [SerializeField] private Canvas canvasHUD;
    [SerializeField] private Canvas canvasWorld;

    [Header("HUDs")]
    [SerializeField] private GameObject leverLook;
    private GameObject leverLookInstance;
    [SerializeField] private GameObject questTitle;
    [SerializeField] private GameObject questLine;
    [SerializeField] private GameObject objectiveMassage;
    [SerializeField] private GameObject questPoint;

    private List<Quest> activeQuests;

    private void Start()
    {
        StartCoroutine(Test());
    }

    IEnumerator Test()
    {
        yield return new WaitForSeconds(2);
        ShowLeverLook(new Vector3(-2.31f,0.3f));
        yield return new WaitForSeconds(4);
        HideLeverLook();
    }

    public void ShowLeverLook(Vector3 position)
    {
        leverLookInstance.SetActive(true);
        leverLookInstance.transform.position = position + new Vector3(0, -1.2f);
        var image = leverLookInstance.transform.GetComponentInChildren<Image>();
        var text = leverLookInstance.transform.GetComponentInChildren<TextMeshProUGUI>();
        StartCoroutine(FadeIn(leverLookInstance, image, text, position));
    }

    public void HideLeverLook()
    {
        var image = leverLookInstance.transform.GetComponentInChildren<Image>();
        var text = leverLookInstance.transform.GetComponentInChildren<TextMeshProUGUI>();
        StopCoroutine("ShowLeverLook");
        StartCoroutine(FadeOut(leverLookInstance, image, text, leverLookInstance.transform.position + new Vector3(0, -2.6f)));
    }

    IEnumerator FadeOut(GameObject instance, Image image, TextMeshProUGUI text, Vector3 position)
    {
        float time = 0;
        while (time < 0.6f)
        {
            time += Time.deltaTime;
            image.color = new Color(1, 1, 1, image.color.a - Time.deltaTime);
            text.color = new Color(1, 1, 1, text.color.a - Time.deltaTime * (1/0.57f));
            instance.transform.position = Vector3.Lerp(instance.transform.position, position, Time.deltaTime*2f);
            yield return null;
        }
        instance.SetActive(false);
        yield return null;
    }

    IEnumerator FadeIn(GameObject instance, Image image, TextMeshProUGUI text, Vector3 position)
    {
        float time = 0;
        float duration = 1.4f;
        while (time < duration)
        {
            time += Time.deltaTime;
            image.color = new Color(1,1,1, (time/duration)*0.57f);
            text.color = new Color(1,1,1, time/duration);
            instance.transform.position = Vector3.Lerp(instance.transform.position, position, Time.deltaTime * 2.15f);
            yield return null;
        }
    }

    public void AddQuestCard(Quest quest)
    {
        activeQuests.Add(quest);
        quest.onComplete += () => { RemoveQuestCard(quest); };
        var title = quest.GetTitle();
        var objectives = quest.GetObjectives().ToArray();
        string[] messages = new string[objectives.Length];
        TextMeshProUGUI[] objectiveTexts = new TextMeshProUGUI[objectives.Length];
        for (int i = 0; i < objectives.Length; i++)
        {
            objectiveTexts[i].text = objectives[i].GetMessage();
            objectives[i].onComplete += () => { objectiveTexts[i].text = "<s>" + objectiveTexts[i].text + "</s>"; };
        }
    }
    
    public float QuestCardHeight(Quest quest)
    {
        //base height
        float height = 9999;
        foreach (var objective in quest.GetObjectives())
        {
            //objective height
            height += 22222;
        }
        return height;
    }

    public void RemoveQuestCard(Quest quest)
    {
        activeQuests.Remove(quest);
        
    }

    public void ShowPauseScreen() //aka Menu Options
    {
        //...
    }

    public void ShowSaveScreen() //SavesNew Canvas
    {
        //...
    }

    public void ShowLoadScreen() //SavesLoad Canvas
    {
        //...
    }

    public void ShowSettingsScreen()
    {
        //...
    }

    public void ShowAlbumScreen()
    {
        //...
    }

    public void ShowQuestsScreen()
    { 
        //...
    }

    public void ShowQuitScreen()
    {
        //...
    }

    //HUD stuff
}
