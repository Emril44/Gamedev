using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement;

public class MenuUIManager : MonoBehaviour
{
    [SerializeField] private GameObject block;
    private GameObject blockInstance;
    [SerializeField] private GameObject yesNo;
    private GameObject yesNoInstance;
    [SerializeField] private Canvas canvas;
    [SerializeField] private Button loadButton;
    [SerializeField] private Button saveButton;
    [SerializeField] private GameObject settings;
    [SerializeField] private GameObject about;
    [SerializeField] private GameObject playerSkin;
    [SerializeField] private Sprite vSprite;
    [SerializeField] private GameObject skinsScroll;
    [SerializeField] private Sprite[] playerSprites;
    private const int LANGUAGES = 2;
    [Header("Saves")]
    [SerializeField] private GameObject line;
    [SerializeField] private GameObject save;
    [SerializeField] private GameObject newSave;
    [Header("SavesLocalization")]
    [TextArea(1, 5)]
    [SerializeField] private string autosaveString;
    [TextArea(1, 5)]
    [SerializeField] private string saveString;
    [TextArea(1, 5)]
    [SerializeField] private string dayString;
    private bool paused = false;
    private Vector3[] baseButtonPos;
    private GameObject V;

    public static MenuUIManager Instance { get; private set; }
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
        yesNoInstance = Instantiate(yesNo);
        yesNoInstance.SetActive(false);
        yesNoInstance.transform.SetParent(canvas.transform, false);
        var no = yesNoInstance.transform.GetChild(1).GetChild(2).gameObject;
        no.GetComponent<Button>().onClick.AddListener(() => { RemoveYesNo(); });
        V = new GameObject("V");
        V.AddComponent<Image>().sprite = vSprite;
        V.SetActive(false);
        try
        {
            PlayerInteraction.Instance.onDeath += delegate { ShowDeathScreen(); };
        }
        catch { }
        try
        {
            PlayerInteraction.Instance.onCanSaveUpdate += b => saveButton.interactable = b;
        }
        catch { }
    }

    IEnumerator SetSkin()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            yield return SkinManager.Instance.LoadOnlyChosenSkin();
            gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = ((Skin)SkinManager.Instance.GetChosenSkinReference().Asset).Sprite;
        }
    }

    private void Start()
    {
        try
        {
            baseButtonPos = new Vector3[]
            {
                canvas.transform.GetChild(1).GetChild(3).gameObject.transform.localPosition,
                canvas.transform.GetChild(1).GetChild(4).gameObject.transform.localPosition,
                canvas.transform.GetChild(1).GetChild(5).gameObject.transform.localPosition,
                canvas.transform.GetChild(1).GetChild(6).gameObject.transform.localPosition,
                canvas.transform.GetChild(1).GetChild(7).gameObject.transform.localPosition
            };
        }
        catch { }
        if (SavesManager.Instance.HasSaves())
        {
            loadButton.interactable = true;
        }
        else
        {
            loadButton.interactable = false;
        }
        StartCoroutine(SetSkin());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && PlayerInteraction.Instance.Controllable)
        {
            Pause();
        }
    }
    
    public void Pause()
    {
        if (EnvironmentManager.Instance.CutsceneRunning)
        {
            return;
        }
        if (SavesManager.Instance.HasSaves())
        {
            loadButton.interactable = true;
        }
        else
        {
            loadButton.interactable = false;
        }
        if (!paused)
        {
            paused = true;
            StopAllCoroutines();
            StartCoroutine(SlowDownFadeIn());
            StartCoroutine(ShowPauseButtons());
        }
        else
        {
            paused = false;
            StopAllCoroutines();
            StartCoroutine(AccelerateFadeOut());
            StartCoroutine(HidePauseButtons());
        }
    }

    IEnumerator SlowDownFadeIn()
    {
        canvas.transform.GetChild(1).GetChild(0).gameObject.SetActive(true);
        var blackout = canvas.transform.GetChild(1).GetChild(0).gameObject.GetComponent<Image>();
        blackout.color = new Color(blackout.color.r, blackout.color.r, blackout.color.r, 0);
        float time = 0;
        float speed = 3f;
        while(time < 1)
        {
            time += Time.unscaledDeltaTime * speed;
            if(time < 1)
            {
                Time.timeScale = 1 - time;
            }
            blackout.color = new Color(0, 0, 0, time / 3);
            yield return null;
        }
        Time.timeScale = 0;
        yield return null;
    }
    IEnumerator AccelerateFadeOut()
    {
        var blackout = canvas.transform.GetChild(1).GetChild(0).gameObject.GetComponent<Image>();
        float time = Time.timeScale;
        float speed = 3f;
        while (time < 1)
        {
            time += Time.unscaledDeltaTime * speed;
            Time.timeScale = time;
            blackout.color = new Color(0, 0, 0, blackout.color.a - Time.unscaledDeltaTime * speed/3);
            yield return null;
        }
        Time.timeScale = 1;
        canvas.transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
        yield return null;
    }
    IEnumerator ShowPauseButtons()
    {
        var buttons = new GameObject[] 
        {
            canvas.transform.GetChild(1).GetChild(3).gameObject,
            canvas.transform.GetChild(1).GetChild(4).gameObject,
            canvas.transform.GetChild(1).GetChild(5).gameObject,
            canvas.transform.GetChild(1).GetChild(6).gameObject,
            canvas.transform.GetChild(1).GetChild(7).gameObject
        };
        float time = 0;
        Vector3 deltaPos = new(915, 0, 0);
        var goalPositions = new Vector3[]
        {
            baseButtonPos[0] - deltaPos,
            baseButtonPos[1] - deltaPos,
            baseButtonPos[2] - deltaPos,
            baseButtonPos[3] - deltaPos,
            baseButtonPos[4] - deltaPos
        };
        float speed = 0.01f;
        while (Mathf.Abs(buttons[2].transform.position.x - goalPositions[2].x) > 0.01f)
        {
            time += Time.unscaledDeltaTime * speed;
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].transform.localPosition = Vector3.Lerp(buttons[i].transform.localPosition, goalPositions[i], time);
            }
            yield return null;
        }
        foreach (var button in buttons)
        {
            button.transform.localPosition = goalPositions[Array.IndexOf(buttons, button)];
        }
        yield return null;
    }
    
    IEnumerator HidePauseButtons()
    {
        var buttons = new GameObject[]
        {
            canvas.transform.GetChild(1).GetChild(3).gameObject,
            canvas.transform.GetChild(1).GetChild(4).gameObject,
            canvas.transform.GetChild(1).GetChild(5).gameObject,
            canvas.transform.GetChild(1).GetChild(6).gameObject,
            canvas.transform.GetChild(1).GetChild(7).gameObject
        };
        float time = 0;        
        float speed = 0.01f;
        while (Mathf.Abs(buttons[2].transform.position.x - baseButtonPos[2].x) > 0.01f)
        {
            time += Time.unscaledDeltaTime * speed;
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].transform.localPosition = Vector3.Lerp(buttons[i].transform.localPosition, baseButtonPos[i], time);
            }
            yield return null;
        }
        yield return null;
    }

    public void GoToMenu()
    {
        SetYesNo("Are you sure you want to go to the menu?+Вийти до головного меню?", () => { SceneManager.LoadScene("MainMenu"); });
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
            localizedText.OnEnable();
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
        var blackout = blockInstance.transform.GetChild(0).gameObject;
        var block = blockInstance.transform.GetChild(1).gameObject;
        block.transform.localPosition = new Vector3(-4.3f, 9, 47);
        float y = 0.3f;
        if (paused)
        {
            block.transform.localPosition = new Vector3(-4.1f, 10);
            y = 0.4f;
        }
        blockInstance.SetActive(true);
        StartCoroutine(FadeIn(blackout, block, y));
    }
    
    IEnumerator FadeOut(GameObject blackout, GameObject block)
    {
        float time = 0;
        while (time < 0.6f)
        {
            time += Time.unscaledDeltaTime;
            blackout.GetComponent<Image>().color = new Color(0, 0, 0, blackout.GetComponent<Image>().color.a - Time.unscaledDeltaTime);

            block.transform.position = Vector3.Lerp(block.transform.position, new Vector2(block.transform.position.x, -30), Time.unscaledDeltaTime);
            yield return null;
        }
        blockInstance.SetActive(false);
        yield return null;
    }

    IEnumerator FadeIn(GameObject blackout, GameObject block, float y)
    {
        float time = 0;
        while (time < 2)
        {
            time += Time.unscaledDeltaTime;
            blackout.GetComponent<Image>().color = new Color(0, 0, 0, time/6);

            block.transform.localPosition = Vector3.Lerp(block.transform.localPosition, new Vector2(block.transform.localPosition.x, y), Time.unscaledDeltaTime * 2.15f);
            yield return null;
        }
    }
    
    public void StartNewGame()
    {
        SavesManager.Instance.LoadNewGame();
    }

    public void ShowSaveGameScreen()
    {
        if (blockInstance.transform.GetChild(1).childCount > 1)
        {
            Destroy(blockInstance.transform.GetChild(1).GetChild(1).gameObject);
        }
        GameObject s = new("Saves Block");
        s.transform.SetParent(blockInstance.transform.GetChild(1), false);
        s.transform.localPosition = new Vector3(4.8f, 31.6f, -14);
        s.transform.localScale = new Vector3(0.06f, 0.06f, 0.06f);
        SaveHeader[] saveHeaders = SavesManager.Instance.SaveHeaders();
        var autosave = saveHeaders[0];
        if (save != null && autosave != null)
        {
            var card = SaveCard(autosaveString, autosave, s.transform, 0);
            card.GetComponent<Button>().onClick.RemoveAllListeners();
            card.GetComponent<Button>().onClick.AddListener(() => { SavesManager.Instance.Load(0); });
            card.GetComponent<Button>().onClick.AddListener(() => { SetYesNo("Overwrite autosave?+Перезаписати автозбереження?", delegate { SavesManager.Instance.Save(0); RemoveBlock(); }); });
        }
        else
        {
            var card = Instantiate(newSave, s.transform);
            card.transform.localPosition = card.transform.localPosition + new Vector3(-246, 425);
            card.GetComponent<Button>().onClick.AddListener(() => { SavesManager.Instance.Save(0); RemoveBlock(); });
        }
        var l = Instantiate(line, s.transform);
        l.transform.localPosition = l.transform.localPosition + new Vector3(0, -375);
        int i = saveHeaders.Length;
        for (int j = 1; j < i; j++)
        {
            int n = j;
            if (saveHeaders[j] != null)
            {
                var card = SaveCard(saveString, saveHeaders[j], s.transform, j);
                card.transform.localPosition = card.transform.localPosition + new Vector3(0, -338 * (j));
                if (j == 0)
                {
                    card.transform.localPosition = card.transform.localPosition + new Vector3(0, -20);
                }
                card.GetComponent<Button>().onClick.AddListener(() => { SetYesNo($"Overwrite save #{n}?+Перезаписати збереження #{n}?", delegate { SavesManager.Instance.Save(n);RemoveBlock(); }); });
            }
            else
            {
                var card = Instantiate(newSave, s.transform);
                card.transform.localPosition = card.transform.localPosition + new Vector3(-246, -338 * (j) + 415);
                card.GetComponent<Button>().onClick.AddListener(() => { SavesManager.Instance.Save(n); RemoveBlock(); });
            }
        }
        ShowBlock();
    }

    public void ShowLoadGameScreen() 
    {
        if (blockInstance.transform.GetChild(1).childCount > 1)
        {
            Destroy(blockInstance.transform.GetChild(1).GetChild(1).gameObject);
        }
        GameObject s = new ("Saves Block");
        s.transform.SetParent(blockInstance.transform.GetChild(1), false);
        s.transform.localPosition = new Vector3(4.8f, 31.6f, -14);
        s.transform.localScale = new Vector3(0.06f, 0.06f, 0.06f);
        SaveHeader[] saveHeaders = SavesManager.Instance.SaveHeaders();
        var autosave = saveHeaders[0];
        if (save != null && autosave != null)
        {
            var card = SaveCard(autosaveString, autosave, s.transform, 0);
            card.GetComponent<Button>().onClick.RemoveAllListeners();
            card.GetComponent<Button>().onClick.AddListener(() => { SavesManager.Instance.Load(0); Time.timeScale = 1; }); 
        }
        else
        {
            var card = Instantiate(newSave, s.transform);
            card.transform.localPosition = card.transform.localPosition + new Vector3(-246, 425);
            card.GetComponentInChildren<LocalizedText>().text = "It's empty+Тут пусто";
            card.GetComponentInChildren<LocalizedText>().OnEnable();
        }
        var l = Instantiate(line, s.transform);
        l.transform.localPosition = l.transform.localPosition + new Vector3(0, -375);
        int i = saveHeaders.Length;
        for(int j = 1; j < i; j++)
        {
            if (saveHeaders[j] != null)
            {
                var card = SaveCard(saveString, saveHeaders[j], s.transform, j);
                card.transform.localPosition = card.transform.localPosition + new Vector3(0, -338 * (j));
                if (j == 0)
                {
                    card.transform.localPosition = card.transform.localPosition + new Vector3(0, -20);
                }
                int n = j;
                card.GetComponent<Button>().onClick.AddListener(() => { SavesManager.Instance.Load(n);Time.timeScale = 1; });
            }
            else
            {
                var card = Instantiate(newSave, s.transform);
                card.transform.localPosition = card.transform.localPosition + new Vector3(-246, -338 * (j) + 415);
                card.GetComponentInChildren<LocalizedText>().text = "It's empty+Тут пусто";
                card.GetComponentInChildren<LocalizedText>().OnEnable();
            }
        }
        ShowBlock();
    }

    public GameObject SaveCard(string title, SaveHeader saveInfo, Transform parent, int n)
    {
        GameObject save = Instantiate(this.save, parent.transform);
        string t = title.Split('+')[PlayerPrefs.GetInt("Language")];
        string num = n > 0 ? " #" + n : "";
        save.GetComponent<SaveTexts>().saveNum.text = t + num;

        string day = dayString.Split('+')[PlayerPrefs.GetInt("Language")];
        save.GetComponent<SaveTexts>().day.text = day + ": " + saveInfo.day;

        save.GetComponent<SaveTexts>().sparksAmount.text = saveInfo.sparks.ToString();
        save.GetComponent<SaveTexts>().time.text = TimeString(saveInfo.timePlayed);

        //save.GetComponent<Button>().onClick.AddListener(() => { SetYesNo($"Overwrite save{num}+Перезаписати збереження{num}", () => { SavesManager.Instance.NewGame(); }) ; });
        var x = save.transform.GetChild(1).gameObject;
        x.GetComponent<Button>().onClick.AddListener(() => { SetYesNo($"Delete save{num}+Видалити збереження{num}", () => 
        {   
            SavesManager.Instance.RemoveSave(n);
            Destroy(save);
        }); });
        return save;
    }

    public string TimeString(int seconds)
    {
        int hours = seconds / 3600;
        string h = hours > 0 ? (hours + ":") : "";
        int minutes = (seconds - hours*3600)/ 60;
        string m = minutes >= 10 ? minutes.ToString() : 0 + minutes.ToString();
        seconds = seconds - hours * 3600 - minutes * 60;
        string s = seconds >= 10 ? seconds.ToString() : 0 + seconds.ToString();
        return $"{h}{m}:{s}";
    }

    public void SetYesNo(string message, UnityEngine.Events.UnityAction onYes)
    {
        blockInstance.transform.GetChild(0).gameObject.SetActive(false);
        yesNoInstance.SetActive(true);
        yesNoInstance.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = message.Split('+')[PlayerPrefs.GetInt("Language")];
        yesNoInstance.transform.GetChild(1).GetChild(1).GetComponent<Button>().onClick.RemoveAllListeners();
        yesNoInstance.transform.GetChild(1).GetChild(1).GetComponent<Button>().onClick.AddListener(() => { onYes(); yesNoInstance.SetActive(false); blockInstance.transform.GetChild(0).gameObject.SetActive(true); });
    }

    public void ShowSkinpacks(bool showBlock)
    {
        if (blockInstance.transform.GetChild(1).childCount > 1)
        {
            Destroy(blockInstance.transform.GetChild(1).GetChild(1).gameObject);
        }   
        var block0 = blockInstance.transform.GetChild(1);
        GameObject skinBlock = new("Skins Block");
        skinBlock.transform.SetParent(block0, false);
        int i = 0;
        foreach (var skinPack in SkinManager.Instance.SkinPacks())
        {
            var text = skinPack.LocalizedName();
            GameObject s = Instantiate(playerSkin, skinBlock.transform);
            s.transform.localScale = new Vector2(0.3f, 0.3f);
            s.transform.localPosition = new Vector3(-30 + 30 * (i % 3), 17.5f + -40 * (i / 3));
            s.GetComponent<Button>().onClick.AddListener(() => { ShowAppearanceScreen(skinPack); });
            var skin = ((Skin)skinPack.Skins[0].Asset);
            s.GetComponent<Image>().sprite = skin.Sprite;
            TextMeshProUGUI t = s.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            t.text = text;
            i++;
        }
        if (showBlock)
        {
            ShowBlock();
        }
        else
        {
            block0.GetChild(0).gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
            block0.GetChild(0).gameObject.GetComponent<Button>().onClick.AddListener(() => { RemoveBlock(); });
        }
    }
    
    public void ShowAppearanceScreen(SkinPack pack)
    {
        if (blockInstance.transform.GetChild(1).childCount > 1)
        {
            Destroy(blockInstance.transform.GetChild(1).GetChild(1).gameObject);
        }
        Sprite[] sprites = new Sprite[pack.Skins.Count];
        for(int i = 0; i < sprites.Length; i++)
        {
            sprites[i] = ((Skin)pack.Skins[i].Asset).Sprite;
        }
        var block0 = blockInstance.transform.GetChild(1);
        block0.GetChild(0).gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
        block0.GetChild(0).gameObject.GetComponent<Button>().onClick.AddListener(() => { V.transform.SetParent(null, false); ShowSkinpacks(false); V.SetActive(false); });
        var block = new GameObject("Appearance");
        block.transform.SetParent(block0, false);
        var layout = Instantiate(this.skinsScroll, block.transform).transform.GetChild(1).GetChild(0);
        layout.GetComponent<RectTransform>().sizeDelta = new Vector2(9.4265f, 8.8214f + 2.9f * (sprites.Length/4));

        if (sprites.Length <= 12)
        {
            //remove scrollbar
            Destroy(block.transform.GetChild(0).GetChild(0).gameObject);
        }
        for (int r = 0; r < sprites.Length/4 + 1; r++)
        {
            GameObject row = layout.GetChild(r).gameObject;
            for(int i = 0; i < 4; i++)
            {
                var skinButton = row.transform.GetChild(i).gameObject;
                if (r * 4 + i < sprites.Length)
                {
                    skinButton.GetComponent<Button>().interactable = ((Skin)pack.Skins[r * 4 + i].Asset).IsAvailable();
                    skinButton.GetComponent<Image>().sprite = sprites[r * 4 + i];
                    skinButton.GetComponent<Image>().color = Color.white;
                    int j = r * 4 + i;
                    skinButton.GetComponent<Button>().onClick.AddListener(delegate { SetV(skinButton, j, pack); });
                }
                else
                {
                    Destroy(skinButton);
                }   
            }
        }
        if (pack.Skins.Contains(SkinManager.Instance.GetChosenSkinReference()))
        {
            SetV(layout.GetChild(
                pack.Skins.IndexOf(SkinManager.Instance.GetChosenSkinReference()) / 4).GetChild(pack.Skins.IndexOf(SkinManager.Instance.GetChosenSkinReference()) % 4).gameObject, 
                pack.Skins.IndexOf(SkinManager.Instance.GetChosenSkinReference()), 
                pack);
        }
        for (int i = sprites.Length / 4 + 1; i <= 7; i++)
        {
            Destroy(layout.GetChild(i).gameObject);
        }
    }

    public void SetV(GameObject button, int i, SkinPack pack)
    {
        V.SetActive(true);
        SkinManager.Instance.SetChosenSkinReference(pack.Skins[i]);
        gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = ((Skin)pack.Skins[i].Asset).Sprite;
        V.transform.SetParent(button.transform,false);
        V.transform.localPosition = new Vector3(93,-212);
        V.transform.localScale = new Vector3(3,3,3);
    }

    public void ShowSettingsScreen()
    {
        if (blockInstance.transform.GetChild(1).childCount > 1)
        {
            Destroy(blockInstance.transform.GetChild(1).GetChild(1).gameObject);
        }
        var settings = Instantiate(this.settings);
        settings.transform.SetParent(blockInstance.transform.GetChild(1), false);
        settings.transform.localPosition = new Vector3(21.5f, 21, 446.5f);
        settings.transform.localScale = new Vector3(0.05725f, 0.05725f, 0.05725f);
        settings.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() => { ChangeLanguage(); });

        var resolutionsDropdown = settings.transform.GetChild(3).GetComponent<TMP_Dropdown>();
        resolutionsDropdown.ClearOptions();
        resolutionsDropdown.AddOptions(Resolutions());
        
        resolutionsDropdown.value = resolutionsDropdown.options.Select(option => option.text).ToList().IndexOf(Screen.currentResolution.ToString().Split('@')[0]);
        resolutionsDropdown.onValueChanged.AddListener(delegate { ChangeResolution(resolutionsDropdown.options[resolutionsDropdown.value].text); });
       
        var fullscreen = settings.transform.GetChild(7).GetComponent<TMP_Dropdown>();
        fullscreen.ClearOptions();
        fullscreen.AddOptions(ScreenModes());
        fullscreen.value = fullscreen.options.Select(option => option.text).ToList().IndexOf(Screen.fullScreenMode.ToString());
        fullscreen.onValueChanged.AddListener(delegate { ChangeFullscreen(fullscreen.options[fullscreen.value].text); });

        var volume = settings.transform.GetChild(9).GetComponent<Slider>();
        try
        {
            volume.value = PlayerPrefs.GetFloat("Volume");
        }
        catch
        {
            volume.value = 1;
        }
        volume.onValueChanged.AddListener(delegate { PlayerPrefs.SetFloat("Volume", volume.value); });
        ShowBlock();
    }

    private void ChangeFullscreen(string text)
    {
        var fullScreen = text switch
        {
            "Windowed" => FullScreenMode.Windowed,
            "FullScreenWindow" => FullScreenMode.FullScreenWindow,
            "ExclusiveFullScreen" => FullScreenMode.ExclusiveFullScreen,
            _ => FullScreenMode.MaximizedWindow,
        };
        Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, fullScreen);
    }

    private List<string> Resolutions()
    {
        var res = new List<string>();
        foreach (var resolution in Screen.resolutions)
        {
            res.Add(resolution.ToString().Split('@')[0]);
        }
        for (int i = 0; i < res.Count; i++)
        {
            for (int j = i + 1; j < res.Count; j++)
            {
                if (res[i] == res[j])
                {
                    res.RemoveAt(j);
                    j--;
                }
            }
        }
        return res;
    }

    private List<string> ScreenModes()
    {
        var modes = new List<string>();
        foreach (var mode in Enum.GetNames(typeof(FullScreenMode)))
        {
            modes.Add(mode);
        }
        return modes;
    }

    private void ChangeResolution(string value)
    {
        var values = value.Split('x');
        Screen.SetResolution(int.Parse(values[0]), int.Parse(values[1]), Screen.fullScreenMode, 60);
    }

    public void ShowAboutScreen()
    {
        if (blockInstance.transform.GetChild(1).childCount > 1)
        {
            Destroy(blockInstance.transform.GetChild(1).GetChild(1).gameObject);
        }
        ShowBlock();
        var about = Instantiate(this.about);
        about.transform.SetParent(blockInstance.transform.GetChild(1), false);
    }

    private void RemoveYesNo()
    {
        blockInstance.transform.GetChild(0).gameObject.SetActive(true);
        yesNoInstance.SetActive(false);
    }

    public void ExitGame()
    {
        SetYesNo("Exit the game+Вийти з гри", () => { Application.Quit(); });
    }

    public void ShowDeathScreen()
    {
        SetYesNo(
        "Rectangle is dead. Load autosave or go to the main menu+Прямокутник помер. Завантаж автозбереження або перейди у головне меню",
        delegate 
        {
            if (SavesManager.Instance.HasSave(0))
            {
                SavesManager.Instance.Load(0);
            }
            else
            {
                SavesManager.Instance.LoadNewGame();
            }
        },
        "Autosave+Автозбереження",
        delegate { SceneManager.LoadScene("MainMenu"); },
        "Main Menu+Головне меню"
        );
    }

    public void SetYesNo(string message, UnityEngine.Events.UnityAction onLeft, string onLeftMessage, UnityEngine.Events.UnityAction onRight, string onRightMessage)
    {
        blockInstance.transform.GetChild(0).gameObject.SetActive(false);
        yesNoInstance.SetActive(true);
        yesNoInstance.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = message.Split('+')[PlayerPrefs.GetInt("Language")];
        yesNoInstance.transform.GetChild(1).GetChild(1).GetComponent<Button>().onClick.AddListener(onLeft);
        yesNoInstance.transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = onLeftMessage.Split('+')[PlayerPrefs.GetInt("Language")];
        yesNoInstance.transform.GetChild(1).GetChild(2).GetComponent<Button>().onClick.AddListener(onRight);
        yesNoInstance.transform.GetChild(1).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = onRightMessage.Split('+')[PlayerPrefs.GetInt("Language")];
    }
}
