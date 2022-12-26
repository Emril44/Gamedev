using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

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
    }

    [SerializeField] private Canvas canvasHUD;
    [SerializeField] private GameObject quests;
    
    [Header("HUDs")]
    [SerializeField] private GameObject questTitle;
    [SerializeField] private GameObject questLine;
    [SerializeField] private GameObject objectiveGO;
    [SerializeField] private GameObject fireResistance;
    [SerializeField] private Transform player;
    [SerializeField] private AnimationCurve cardMoveCurve;
    [SerializeField] private AnimationCurve fadeOutCurve;

    private List<Quest> activeQuests = new();
    private Queue<IEnumerator> questCoroutines = new();
    private bool isCoroutineRunning = false;
    
    private int cardsMoving = 0;
    private float fireResistanceFull;

    private void OnEnable()
    {
        PlayerInteraction.Instance.onHealthUpdate += delegate { UpdateHealthbar(); };
        if (PlayerInteraction.Instance.IsFireproof())
        {
            fireResistance.SetActive(true);
        }
        else
        {
            fireResistance.SetActive(false);
        }
        PlayerInteraction.Instance.onFireproofApply += x => UpdateFireproof(x, x);
        PlayerInteraction.Instance.onFireproofUpdate += x => UpdateFireproof(x);
        PlayerInteraction.Instance.onFireproofEnd += delegate { fireResistance.SetActive(false); };
        QuestManager.Instance.onQuestActivate += q => AddQuestCard(q);
    }

    void UpdateFireproof(float full, float current)
    {
        fireResistance.SetActive(true);
        fireResistanceFull = full;
        UpdateFireproof(current);
    }
    void UpdateFireproof(float current)
    {
        var image = fireResistance.GetComponent<Image>();
        image.fillAmount = current / fireResistanceFull;
        if (current < 0.001f)
        {
            fireResistance.SetActive(false);
        }
    }
        
    public void UpdateHealthbar()
    {
        int newHealth = PlayerInteraction.Instance.health;
        var healthbar = canvasHUD.transform.GetChild(1).GetChild(2).gameObject;
        healthbar.GetComponent<Slider>().value = newHealth;
        var healthbarText = healthbar.transform.GetChild(2).GetChild(0).GetChild(0).gameObject;
        healthbarText.GetComponent<TextMeshProUGUI>().text = newHealth.ToString();
    }
    
    IEnumerator FadeIn(GameObject instance, Image image, float imageTargetA, TextMeshProUGUI text, Vector3 position, float duration)
    {
        isCoroutineRunning = true;
        instance.SetActive(true);
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
        isCoroutineRunning = false;
        TryMoveQueue();
    }
    
    IEnumerator StrikeText(TextMeshProUGUI text)
    {
        isCoroutineRunning = true;
        float time = 0;
        string originalText = text.text;
        float durationOne = 0.05f;
        int symbols = text.text.Length;
        float duration = durationOne * symbols;
        while (time < duration)
        {
            time += Time.deltaTime;
            text.text = "<s>" + originalText.Substring(0, (int)(symbols * (time / duration))) + "</s>" + originalText.Substring((int)(symbols * (time / duration)));
            yield return null;
        }
        questCoroutines.Dequeue();
        isCoroutineRunning = false;
        TryMoveQueue();
    }
    IEnumerator MoveToNextObjective(TextMeshProUGUI tmp, Quest quest)
    {
        if (quest == null || quest.GetCurrentObjective() == null)
        {
            questCoroutines.Dequeue();
            isCoroutineRunning = false;
            TryMoveQueue();
            yield break;
        }
        var text = quest.GetCurrentObjective().LocalizedMessage();
        isCoroutineRunning = true;
        float time = 0;
        float duration = text.Length * 0.05f;
        while (time < duration)
        {
            tmp.text = text.Substring(0, (int)(text.Length * (time / duration)));
            yield return null;
            time += Time.deltaTime;
        }
        tmp.text = text;
        questCoroutines.Dequeue();
        isCoroutineRunning = false;
        void del()
        {
            EnqueMove(StrikeText(tmp), MoveToNextObjective(tmp, quest));
            quest.onObjectiveComplete -= del;
        }
        quest.onObjectiveComplete += del;
        TryMoveQueue();
    }
    
    IEnumerator FadeInQuestTitle(GameObject title, Image line)
    {
        float duration = 0.8f;
        isCoroutineRunning = true;
        var text = title.GetComponent<TextMeshProUGUI>();
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
        isCoroutineRunning = false;
        TryMoveQueue();
    }

    public void AddQuestCard(Quest quest)
    {
        if (Camera.main.orthographicSize > 4.0001 || !CamMovement.Instance.onPlayer)
        {
            StartCoroutine(AddQuestCardCoroutine(quest));
            return;
        }
        float y = 1.54f + player.localPosition.y;
        foreach (Quest q in activeQuests)
        {
            y -= QuestCardHeight(q);
        }
        
        GameObject questGO = new("Quest");
        questGO.transform.SetParent(canvasHUD.transform.GetChild(0), false);

        var titleGO = Instantiate(questTitle, questGO.transform);
        titleGO.GetComponent<TextMeshProUGUI>().text = quest.GetData().LocalizedTitle();
        titleGO.transform.position = new Vector2(titleGO.transform.position.x, y);
        titleGO.SetActive(false);

        y -= 0.29f;
        var line = Instantiate(questLine, questGO.transform);
        line.transform.position = new Vector2(line.transform.position.x, y);
        y += 0.05f;

        line.GetComponent<Image>().fillAmount = 0;
        questCoroutines.Enqueue(FadeInQuestTitle(titleGO, line.GetComponent<Image>()));
        TryMoveQueue();

        /*
        var objectives = quest.GetObjectives().ToArray();
        var objectiveGOs = new GameObject[objectives.Length];
        TextMeshProUGUI[] objectiveTMPs = new TextMeshProUGUI[objectives.Length];
        for (int i = 0; i < objectives.Length; i++)
        {
            objectiveGOs[i] = Instantiate(objectiveGO, questGO.transform);
            objectiveTMPs[i] = objectiveGOs[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            objectiveGOs[i].transform.position = new Vector2(objectiveGOs[i].transform.position.x, y - 0.38f * i);
        }
        for (int i = 0; i < objectives.Length; i++)
        {
            objectiveTMPs[i].text = objectives[i].GetMessage();
            objectiveGOs[i].transform.localPosition = objectiveGOs[i].transform.localPosition + new Vector3(0, -90);
            objectiveGOs[i].SetActive(false);
            int j = i;
            questCoroutines.Enqueue(FadeIn(objectiveGOs[j], objectiveGOs[j].transform.GetChild(0).GetComponent<Image>(), 1, objectiveTMPs[j], objectiveGOs[j].transform.localPosition + new Vector3(0, 90), 0.7f));
            objectives[j].onComplete += () => { questCoroutines.Enqueue(StrikeText(objectiveTMPs[j])); TryMoveQueue(); };
        }
        */

        var objective = Instantiate(objectiveGO, questGO.transform);
        var objectiveTMP = objective.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        objective.transform.position = new Vector2(objective.transform.position.x, y);
        objectiveTMP.text = quest.GetCurrentObjective().LocalizedMessage();
        objective.transform.localPosition = objective.transform.localPosition + new Vector3(0, -90);
        objective.SetActive(false);
        questCoroutines.Enqueue(FadeIn(objective, objective.transform.GetChild(0).GetComponent<Image>(), 1, objectiveTMP, objective.transform.localPosition + new Vector3(0, 90), 0.7f));
        void del()
        {
            EnqueMove(StrikeText(objectiveTMP), MoveToNextObjective(objectiveTMP, quest));
            quest.onObjectiveComplete -= del;
        }
        quest.onObjectiveComplete += del;

        activeQuests.Add(quest);
        quest.onUpdate += () => { UpdateQuestText(quest, objectiveTMP); };
        quest.onComplete += () => { RemoveQuestCard(quest); };
    }

    private void EnqueMove(IEnumerator a, IEnumerator b)
    {
        questCoroutines.Enqueue(a);
        TryMoveQueue();
        questCoroutines.Enqueue(b); 
    }
    
    private void UpdateQuestText(Quest quest, TextMeshProUGUI objectiveTMP)
    {
        Objective obj = quest.GetCurrentObjective();
        if (obj == null)
            return;
        if (obj.GetType() == typeof(SparksObjective))
        {
            // check if objective base messages are matching
            SparksObjective objSparks = obj as SparksObjective;
            string checkAgainst = objSparks.LocalizedMessageBase();
            string check = objectiveTMP.text;
            if (check.Length > checkAgainst.Length)
            {
                check = check.Substring(0, checkAgainst.Length);
                if (check.Equals(checkAgainst)) objectiveTMP.text = quest.GetCurrentObjective().LocalizedMessage();
            }
        }
    }

    private void UpdateQuestTexts(Quest quest, TextMeshProUGUI[] objectiveTMPs)
    {
        var objectives = quest.GetObjectives().ToArray();
        for (int i = 0; i < objectives.Length; i++)
        {
            objectiveTMPs[i].text = objectives[i].LocalizedMessage();
        }
    }

    private IEnumerator AddQuestCardCoroutine(Quest quest)
    {
        isCoroutineRunning = true;
        yield return new WaitUntil(() => Camera.main.orthographicSize < 4.0001);
        yield return new WaitUntil(() => CamMovement.Instance.onPlayer);
        isCoroutineRunning = false;
        AddQuestCard(quest);
    }

    private float QuestCardHeight(Quest quest)
    {
        //base height
        float height = 0.59f + 0.07f;
        //foreach (var objective in quest.GetObjectives())
        //{
            //objective height
            height += 0.38f;
        //}
        return height;
    }
    
    private void RemoveQuestCard(Quest quest)
    {
        questCoroutines.Enqueue(RemoveQuestCardCoroutine(quest, QuestCardHeight(quest)));
        TryMoveQueue();
    }

    IEnumerator RemoveQuestCardCoroutine(Quest quest, float height)
    {
        isCoroutineRunning = true;
        var card = quests.transform.GetChild(activeQuests.IndexOf(quest)).gameObject;
        int i = activeQuests.IndexOf(quest);
        activeQuests.Remove(quest);
        List<Image> images = new()
        {
            card.transform.GetChild(1).GetComponent<Image>()
        };
        List<TextMeshProUGUI> texts = new()
        {
            card.transform.GetChild(0).GetComponent<TextMeshProUGUI>()
        };
        for (int k = 2; k < card.transform.childCount; k++)
        {
            images.Add(card.transform.GetChild(k).GetChild(0).GetComponent<Image>());
            texts.Add(card.transform.GetChild(k).GetChild(1).GetComponent<TextMeshProUGUI>());
        }
        float time = 0;
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
    
    IEnumerator MoveQuestCards(int n, float distance)
    {
        for (int i = n; i < activeQuests.Count; i++)
        {
            var card = quests.transform.GetChild(i).gameObject;
            StartCoroutine(MoveCard(card, new Vector3(0, distance * 270)));
            yield return null;
        }
        isCoroutineRunning = false;
        TryMoveQueue();
    }

    IEnumerator MoveCard(GameObject card, Vector3 delta)
    {
        cardsMoving++;
        Vector3 start = card.transform.localPosition;
        Vector3 goal = card.transform.localPosition + delta;
        float speed = 0.2f;
        float time = 0;
        while ((card.transform.localPosition - goal).magnitude > 0.5f)
        {
            time += (Time.smoothDeltaTime * speed);
            speed = 0.2f + cardMoveCurve.Evaluate(time);
            card.transform.localPosition = Vector3.Lerp(start, goal, time);
            yield return null;
        }
        card.transform.localPosition = goal;
        cardsMoving--;
        TryMoveQueue();
    }
    
    private void TryMoveQueue()
    {
        if (questCoroutines.Count >= 1 && cardsMoving <= 0 && !isCoroutineRunning)
        {
            StartCoroutine(questCoroutines.Peek());
        }
    }    
}
