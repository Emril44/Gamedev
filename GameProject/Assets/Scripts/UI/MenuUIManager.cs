﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;
using System.Linq;

public class MenuUIManager : MonoBehaviour
{
    [SerializeField] private GameObject block;
    private GameObject blockInstance;
    [SerializeField] private GameObject yesNo;
    private GameObject yesNoInstance;
    [SerializeField] private Canvas canvas;
    [SerializeField] private Button loadButton;
    [SerializeField] private GameObject settings;
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
                canvas.transform.GetChild(1).GetChild(6).gameObject.transform.localPosition
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
    }


    public void Pause()
    {
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
        float speed = 1f;
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
        float speed = 1f;
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
            canvas.transform.GetChild(1).GetChild(6).gameObject
        };
        float time = 0;
        Vector3 deltaPos = new(915, 0, 0);
        var goalPositions = new Vector3[]
        {
            baseButtonPos[0] - deltaPos,
            baseButtonPos[1] - deltaPos,
            baseButtonPos[2] - deltaPos,
            baseButtonPos[3] - deltaPos
        };
        float speed = 0.01f;
        while (time < 1)
        {
            time += Time.unscaledDeltaTime * speed;
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].transform.localPosition = Vector3.Lerp(buttons[i].transform.localPosition, goalPositions[i], time);
            }
            yield return null;
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
            canvas.transform.GetChild(1).GetChild(6).gameObject
        };
        float time = 0;
        float speed = 0.01f;
        while (time < 1)
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
        StopAllCoroutines();
        var blackout = blockInstance.transform.GetChild(0).gameObject;
        var block = blockInstance.transform.GetChild(1).gameObject;
        block.transform.localPosition = new Vector3(-4.3f, 9);
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
    
    public void ShowNewGameScreen() 
    {
        if (blockInstance.transform.GetChild(1).childCount > 1)
        {
            Destroy(blockInstance.transform.GetChild(1).GetChild(1).gameObject);
        }
        GameObject s = new GameObject("Saves Block");
        s.transform.SetParent(blockInstance.transform.GetChild(1), false);
        s.transform.localPosition = new Vector3(4.8f, 31.6f, -14);
        s.transform.localScale = new Vector3(0.06f, 0.06f, 0.06f);
        var autosave = SavesManager.Instance.Autosave();
        if(save != null)
        {
            var card = SaveCard(autosaveString, autosave, s.transform, 0);
            card.GetComponent<Button>().onClick.RemoveAllListeners();
            card.GetComponent<Button>().onClick.AddListener(() => { Debug.Log("you don't overwrite autosave to start a new game"); }); //load
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
            }
            else
            {
                var card = Instantiate(newSave, s.transform);
                card.transform.localPosition = card.transform.localPosition + new Vector3(-246, -338 * (j+1) + 415);
                int n = j + 1;
                card.GetComponent<Button>().onClick.AddListener(() => { SavesManager.Instance.NewGame(n); });
            }
        }
        ShowBlock();
    }

    public GameObject SaveCard(string title, Save saveInfo, Transform parent, int n)
    {
        GameObject save = Instantiate(this.save, parent.transform);
        string t = title.Split('+')[PlayerPrefs.GetInt("Language")];
        string num = n > 0 ? " #" + n : "";
        save.GetComponent<SaveTexts>().saveNum.text = t + num;

        string day = dayString.Split('+')[PlayerPrefs.GetInt("Language")];
        save.GetComponent<SaveTexts>().day.text = day + ": " + saveInfo.day;

        save.GetComponent<SaveTexts>().sparksAmount.text = saveInfo.sparks.ToString();
        save.GetComponent<SaveTexts>().time.text = TimeString(saveInfo.time);

        save.GetComponent<Button>().onClick.AddListener(() => { SetYesNo($"Overwrite save{num}+Перезаписати збереження{num}", () => { SavesManager.Instance.NewGame(n); }) ; });
        var x = save.transform.GetChild(1).gameObject;
        x.GetComponent<Button>().onClick.AddListener(() => { SetYesNo($"Delete save{num}+Видалити збереження{num}", () => { SavesManager.Instance.RemoveSave(n); }); });
        return save;
    }

    public string TimeString(int seconds)
    {
        int hours = seconds / 3600;
        string h = hours > 0 ? hours + ":" : "";
        int minutes = (seconds - hours*3600)/ 60;
        string m = minutes >= 10 ? minutes.ToString() : 0 + minutes.ToString();
        seconds = seconds - hours * 3600 - minutes * 60;
        return $"{h}{m}:{seconds}";
    }

    public void SetYesNo(string message, UnityEngine.Events.UnityAction onYes)
    {
        blockInstance.transform.GetChild(0).gameObject.SetActive(false);
        yesNoInstance.SetActive(true);
        yesNoInstance.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = message.Split('+')[PlayerPrefs.GetInt("Language")];
        yesNoInstance.transform.GetChild(1).GetChild(1).GetComponent<Button>().onClick.AddListener(onYes);
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

        var refreshRate = settings.transform.GetChild(5).GetComponent<TMP_InputField>();
        if(Application.targetFrameRate < 0)
        {
            Application.targetFrameRate = 60;
        }
        refreshRate.text = Application.targetFrameRate.ToString();
        refreshRate.gameObject.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = Application.targetFrameRate.ToString();
        refreshRate.onEndEdit.AddListener(delegate { ChangeRefreshRate(int.Parse(refreshRate.text), refreshRate); });

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

    private void ChangeRefreshRate(int refreshRate, TMP_InputField input)
    {
        if(refreshRate < 10)
        {
            refreshRate = 60;
        }
        Application.targetFrameRate = refreshRate;
        Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, Screen.fullScreenMode, refreshRate);
        input.text = refreshRate.ToString();
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
}
