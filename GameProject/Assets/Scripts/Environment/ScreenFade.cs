using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFade : MonoBehaviour
{
    public Image BlackScreen;
    public static ScreenFade Instance { get; private set; }

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

        BlackScreen = GetComponent<Image>();
    }

    public IEnumerator FadeOut(int speed)
    {
        yield return StartCoroutine(Fade(true, speed));
    }

    public IEnumerator FadeIn(int speed)
    {
        yield return StartCoroutine(Fade(false, speed));
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
