using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuUIManager : MonoBehaviour
{
    [SerializeField] private GameObject block;
    private GameObject blockInstance;
    [SerializeField] private GameObject yesNo;
    private GameObject yesNoInstance;
    [SerializeField] private Canvas canvas;

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
        //yesNoInstance = Instantiate(yesNo);
        //yesNoInstance.SetActive(false);
    }

    public void RemoveBlock()
    {
        StopAllCoroutines();
        var blackout = blockInstance.transform.GetChild(0).gameObject;
        var block = blockInstance.transform.GetChild(1).gameObject;
        StartCoroutine(FadeOut(blackout, block));
    }
    
    IEnumerator FadeOut(GameObject blackout, GameObject block)
    {
        float time = 0;
        while (time < 0.6f)
        {
            time += Time.deltaTime;
            blackout.GetComponent<Image>().color = new Color(0, 0, 0, 2/7f - time);

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
    
    public void ShowNewGameScreen() //SavesNew Canvas
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void ShowLoadScreen() //SavesLoad Canvas
    {
        //...
    }

    public void ShowSettingsScreen()
    {
        StopAllCoroutines();
        var blackout = blockInstance.transform.GetChild(0).gameObject;
        var block = blockInstance.transform.GetChild(1).gameObject;
        block.transform.position = new Vector3(-1.65f,9);
        blockInstance.SetActive(true);
        StartCoroutine(FadeIn(blackout, block));
    }

    public void ShowAboutScreen()
    {
        //...
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
