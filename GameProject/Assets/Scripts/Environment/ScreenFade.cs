using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScreenFade : MonoBehaviour
{
    [SerializeField] private Image BlackScreen;
    private Canvas canvas;
    public static ScreenFade Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        canvas = GetComponent<Canvas>();
        SceneManager.sceneLoaded += (Scene scene, LoadSceneMode mode) =>
        {
            canvas.worldCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        };
    }

    public void SetOverUI(bool overlaying)
    {
        if (overlaying)
        {
            canvas.sortingOrder = 101;
        }
        else
        {
            canvas.sortingOrder = 99;
        }
    }

    public IEnumerator FadeOut(int speed)
    {
        yield return Fade(true, speed);
    }

    public IEnumerator FadeIn(int speed)
    {
        yield return Fade(false, speed);
    }

    IEnumerator Fade(bool fadeToBlack = true, int fadeSpeed = 5)
    {
        Color blackScreenColor = BlackScreen.color;
        float fadeAmount;

        // screen fading out
        if (fadeToBlack)
        {
            while (BlackScreen.color.a < 1)
            {
                fadeAmount = blackScreenColor.a + (fadeSpeed * Time.deltaTime);

                blackScreenColor = new Color(blackScreenColor.r, blackScreenColor.g, blackScreenColor.b, fadeAmount);
                BlackScreen.color = blackScreenColor;
                yield return null;
            }
        }
        // screen fading in
        else
        {
            while (BlackScreen.GetComponent<Image>().color.a > 0)
            {
                fadeAmount = blackScreenColor.a - (fadeSpeed * Time.deltaTime);

                blackScreenColor = new Color(blackScreenColor.r, blackScreenColor.g, blackScreenColor.b, fadeAmount);
                BlackScreen.color = blackScreenColor;
                yield return null;
            }
        }
    }
}
