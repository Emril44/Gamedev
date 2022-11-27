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
        quests = new GameObject("Quests");
        quests.transform.SetParent(canvasHUD.transform);
        quests.transform.localScale = Vector3.one;
        quests.transform.localPosition = new Vector3(1021,0,0);
    }

    [SerializeField] private Canvas canvasHUD;
    [SerializeField] private Canvas canvasWorld;

    [Header("HUDs")]
    [SerializeField] private GameObject leverLook;
    private GameObject leverLookInstance;
    [SerializeField] private GameObject questTitle;
    [SerializeField] private GameObject questLine;
    [SerializeField] private GameObject objectiveGO;
    [SerializeField] private GameObject sampleQuest;
    [SerializeField] private GameObject sampleQuest2;
    [SerializeField] private GameObject sampleQuest3;
    [SerializeField] private GameObject sampleQuest4;
    [SerializeField] private Transform player;
    [SerializeField] private AnimationCurve cardMoveCurve;
    [SerializeField] private AnimationCurve fadeOutCurve;

    private List<Quest> activeQuests = new List<Quest>();
    private Queue<IEnumerator> questCoroutines = new Queue<IEnumerator>();
    private int cardsMoving = 0;
    private bool isCoroutineRunning = false;
    private GameObject quests;

    private void Start()
    {
        StartCoroutine(Test());
    }

    IEnumerator Test()
    {
        yield return new WaitForSeconds(2.5f);
        AddQuestCard(sampleQuest.GetComponent<Quest>());
        AddQuestCard(sampleQuest2.GetComponent<Quest>());
        AddQuestCard(sampleQuest4.GetComponent<Quest>());
        AddQuestCard(sampleQuest3.GetComponent<Quest>());
        RemoveQuestCard(activeQuests[1]);
        
        RemoveQuestCard(activeQuests[0]);
        
        RemoveQuestCard(activeQuests[2]);
        RemoveQuestCard(activeQuests[3]);
    }

    /*
    public void ShowLeverLook(Vector3 position)
    {
        leverLookInstance.SetActive(true);
        leverLookInstance.transform.position = position + new Vector3(0, -1.2f);
        var image = leverLookInstance.transform.GetComponentInChildren<Image>();
        var text = leverLookInstance.transform.GetComponentInChildren<TextMeshProUGUI>();
        StartCoroutine(FadeIn(leverLookInstance, image, 0.57f, text, position));
    }

    public void HideLeverLook()
    {
        var image = leverLookInstance.transform.GetComponentInChildren<Image>();
        var text = leverLookInstance.transform.GetComponentInChildren<TextMeshProUGUI>();
        StopCoroutine("ShowLeverLook");
        StartCoroutine(FadeOut(leverLookInstance, image, 0.57f, text, leverLookInstance.transform.position + new Vector3(0, -2.6f)));
    }
    
    
    IEnumerator FadeOut(GameObject instance, Image image, float imageBaseA, TextMeshProUGUI text, Vector3 position)
    {
        isCoroutineRunning = true;
        float time = 0;
        while (time < 0.6f)
        {
            time += Time.deltaTime;
            image.color = new Color(1, 1, 1, image.color.a - Time.deltaTime);
            text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a - Time.deltaTime * (1/ imageBaseA));
            instance.transform.position = Vector3.Lerp(instance.transform.position, position, Time.deltaTime*2f);
            yield return null;
        }
        instance.SetActive(false);
        if (questCoroutines.Count > 0)
        {
            StartCoroutine(questCoroutines.Dequeue());
        }
        else
        {
            isCoroutineRunning = false;
        }
        yield return null;
    }
    */
    
    IEnumerator FadeIn(GameObject instance, Image image, float imageTargetA, TextMeshProUGUI text, Vector3 position, float duration)
    {
        instance.SetActive(true);
        isCoroutineRunning = true;
        float time = 0;
        while (time < duration)
        {
            time += Time.deltaTime;
            image.color = new Color(1,1,1, (time/duration)* imageTargetA);
            text.color = new Color(text.color.r, text.color.g, text.color.b, time/duration);
            instance.transform.localPosition = Vector3.Lerp(instance.transform.localPosition, position, Time.deltaTime * 2.15f);
            yield return null;
        }
        questCoroutines.Dequeue();
        if (questCoroutines.Count > 0)
        {
            StartCoroutine(questCoroutines.Peek());
        }
        else
        {
            isCoroutineRunning = false;
        }
    }
    
    IEnumerator StrikeText(TextMeshProUGUI text, float durationOne)
    {
        int symbols = text.text.Length;
        float time = 0;
        string originalText = text.text;
        float duration = durationOne * symbols;
        while (time < duration)
        {
            time += Time.deltaTime;
            text.text = "<s>" + originalText.Substring(0, (int)(symbols * (time / duration))) + "</s>" + originalText.Substring((int)(symbols * (time / duration)));
            yield return null;
        }
    }

    IEnumerator FadeInQuestTitle(GameObject title, Image line)
    {
        isCoroutineRunning = true;
        var text = title.GetComponent<TextMeshProUGUI>();
        float duration = 0.8f;
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
        title.SetActive(true);
        float time = 0;
        while (time < duration)
        {
            time += Time.deltaTime;
            text.color = new Color(text.color.r, text.color.g, text.color.b, time/duration);
            line.fillAmount = time/duration;
            yield return null;
        }
        line.fillAmount = 1;
        text.color = new Color(text.color.r, text.color.g, text.color.b, 1);
        questCoroutines.Dequeue();
        if (questCoroutines.Count > 0)
        {
            StartCoroutine(questCoroutines.Peek());
        }
        else
        {
            isCoroutineRunning = false;
        }
    }

    public void AddQuestCard(Quest quest)
    {
        float baseY = 4.02f + player.localPosition.y + 3.2108f;
        foreach (Quest q in activeQuests)
        {
            baseY -= QuestCardHeight(q);
        }
        GameObject questGO = new GameObject("Quest");
        questGO.transform.SetParent(canvasHUD.transform.GetChild(0), false);

        var titleGO = Instantiate(questTitle, questGO.transform);
        titleGO.GetComponent<TextMeshProUGUI>().text = quest.GetTitle();
        titleGO.transform.position = new Vector2(titleGO.transform.position.x, baseY);

        baseY -= 0.29f;
        var line = Instantiate(questLine, questGO.transform);
        line.transform.position = new Vector2(line.transform.position.x, baseY);
        baseY += 0.05f;

        var objectives = quest.GetObjectives().ToArray();
        
        line.GetComponent<Image>().fillAmount = 0;
        titleGO.SetActive(false);
        questCoroutines.Enqueue(FadeInQuestTitle(titleGO, line.GetComponent<Image>()));
        var objectiveGOs = new GameObject[objectives.Length];
        TextMeshProUGUI[] objectiveTexts = new TextMeshProUGUI[objectives.Length];
        for (int i = 0; i < objectives.Length; i++)
        {
            objectiveGOs[i] = Instantiate(objectiveGO, questGO.transform);
            objectiveTexts[i] = objectiveGOs[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            objectiveGOs[i].transform.position = new Vector2(objectiveGOs[i].transform.position.x, baseY - 0.38f * i);
        }
        string[] messages = new string[objectives.Length];
        for (int i = 0; i < objectives.Length; i++)
        {
            objectiveTexts[i].text = objectives[i].GetMessage();
            objectiveGOs[i].transform.localPosition = objectiveGOs[i].transform.localPosition + new Vector3(0, -90);
            objectiveGOs[i].SetActive(false);
            questCoroutines.Enqueue(FadeIn(objectiveGOs[i], objectiveGOs[i].transform.GetChild(0).GetComponent<Image>(), 1, objectiveTexts[i], objectiveGOs[i].transform.localPosition + new Vector3(0, 90), 0.7f));
            objectives[i].onComplete += () => { StartCoroutine(StrikeText(objectiveTexts[i], 0.017f)); };
        }
        
        if (!isCoroutineRunning)
        {
            StartCoroutine(questCoroutines.Peek());
        }      
        activeQuests.Add(quest);
        quest.onComplete += () => { RemoveQuestCard(quest); };
    }
    
    public float QuestCardHeight(Quest quest)
    {
        //base height
        float height = 0.59f + 0.07f;
        foreach (var objective in quest.GetObjectives())
        {
            //objective height
            height += 0.38f;
        }
        return height;
    }

    public void RemoveQuestCard(Quest quest)
    {
        questCoroutines.Enqueue(RemoveQuestCardCoroutine(quest, QuestCardHeight(quest)));
        if (questCoroutines.Count < 2 && !isCoroutineRunning)
        {
            StartCoroutine(questCoroutines.Peek());
        }
    }

    IEnumerator RemoveQuestCardCoroutine(Quest quest, float height)
    {
        var card = quests.transform.GetChild(activeQuests.IndexOf(quest)).gameObject;
        int i = activeQuests.IndexOf(quest);
        activeQuests.Remove(quest);
        float time = 0;
        List<Image> images = new List<Image>();
        List<TextMeshProUGUI> texts = new List<TextMeshProUGUI>();
        texts.Add(card.transform.GetChild(0).GetComponent<TextMeshProUGUI>());
        images.Add(card.transform.GetChild(1).GetComponent<Image>());
        for (int k = 2; k < card.transform.childCount; k++)
        {
            images.Add(card.transform.GetChild(k).GetChild(0).GetComponent<Image>());
            texts.Add(card.transform.GetChild(k).GetChild(1).GetComponent<TextMeshProUGUI>());
        }
        while (images[0].color.a > 0)
        {
            time += Time.deltaTime;
            float speed = fadeOutCurve.Evaluate(time);
            foreach (Image image in images)
            {
                image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a - Time.deltaTime*speed);
            }
            foreach (TextMeshProUGUI text in texts)
            {
                text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a - Time.deltaTime * speed);
            }
            yield return null;
        }
        Destroy(card);
        yield return null;
        questCoroutines.Dequeue();
        StartCoroutine(MoveQuestCards(i, height));
    }

    IEnumerator MoveQuestCards(int n, float height)
    {
        for (int i = n; i < activeQuests.Count; i++)
        {
            var card = quests.transform.GetChild(i).gameObject;
            StartCoroutine(MoveCard(card, new Vector3(0, height * 265)));
            yield return null;
        }
        if (cardsMoving == 0)
        {
            if (questCoroutines.Count > 0)
            {
                StartCoroutine(questCoroutines.Peek());
            }
        }
    }

    IEnumerator MoveCard(GameObject card, Vector3 delta)
    {
        cardsMoving++;
        float speed = 0.2f;
        Vector3 pos = card.transform.localPosition;
        Vector3 goal = card.transform.localPosition + delta;
        float time = 0;
        while ((card.transform.localPosition - goal).magnitude > 0.6f)
        {
            time += (Time.smoothDeltaTime * speed);
            speed = 0.2f + cardMoveCurve.Evaluate(time);
            card.transform.localPosition = Vector3.Lerp(pos, goal, time);
            yield return null;
        }
        card.transform.localPosition = goal;
        cardsMoving--;
        if(cardsMoving == 0)
        {
            if (questCoroutines.Count > 0)
            {
                StartCoroutine(questCoroutines.Peek());
            }
        }
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
