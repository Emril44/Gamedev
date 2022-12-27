using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioController : MonoBehaviour
{
    public enum BGM
    {
        Monochrome,
        Denial,
        Anger,
        Bargain
    }
    private BGM currentBGM = BGM.Monochrome;
    [SerializeField] private AudioSource[] bgmSources;
    private Coroutine[] volumeCoroutines;
    [Range(0.0001f, 10)]
    [SerializeField] private float distortionEffectTime;
    [Range(0, 1)]
    [SerializeField] private float distortionEffectDistortion;
    [SerializeField] private AudioMixer mixer;

    public static AudioController Instance { get; private set; }
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
            DontDestroyOnLoad(this);
        }
        volumeCoroutines = new Coroutine[bgmSources.Length];
        UpdateMusicVolume();
        SceneManager.sceneLoaded += (Scene scene, LoadSceneMode mode) =>
        {
            ChangeBGM(BGM.Monochrome);
        };
        bgmSources[(int)BGM.Monochrome].Play();
    }

    public void UpdateMusicVolume()
    {
        SetMixerVolume("MusicVolume", Mathf.Max(PlayerPrefs.GetFloat("Volume", 1), 0.0001f));
    }

    private void SetMixerVolume(string propertyName, float volume)
    {
        mixer.SetFloat(propertyName, Mathf.Log10(volume) * 20);
    }

    public IEnumerator SetDistorted(bool distorted)
    {
        float time = 0;
        if (distorted)
        {
            while (time < distortionEffectTime)
            {
                yield return new WaitForFixedUpdate();
                time += Time.fixedDeltaTime;
                mixer.SetFloat("Distortion", Mathf.Lerp(0, distortionEffectDistortion, time / distortionEffectTime));
            }
            mixer.SetFloat("Distortion", 1);
        }
        else
        {
            while (time < distortionEffectTime)
            {
                yield return new WaitForFixedUpdate();
                time += Time.fixedDeltaTime;
                mixer.SetFloat("Distortion", Mathf.Lerp(distortionEffectDistortion, 0, time / distortionEffectTime));
            }
            mixer.SetFloat("Distortion", 0);
        }
    }

    public void ChangeBGM(BGM bgm)
    {
        if (bgm == currentBGM)
        {
            return;
        }

        if (volumeCoroutines[(int)bgm] != null)
        {
            StopCoroutine(volumeCoroutines[(int)bgm]);
        }
        if (volumeCoroutines[(int)currentBGM] != null)
        {
            StopCoroutine(volumeCoroutines[(int)currentBGM]);
        }
        volumeCoroutines[(int)currentBGM] = StartCoroutine(SetVolumeSmoothly(currentBGM, 0));
        volumeCoroutines[(int)bgm] = StartCoroutine(SetVolumeSmoothly(bgm, 1));
        currentBGM = bgm;
    }

    private IEnumerator SetVolumeSmoothly(BGM bgm, float target)
    {
        AudioSource audioSource = bgmSources[(int)bgm];
        float startVolume = audioSource.volume;
        float t = 0;

        if (!audioSource.isPlaying) audioSource.Play();

        // fade in original source with new clip and fade out new source with old clip
        while (t < 0.98f)
        {
            t = Mathf.Lerp(t, 1f, 0.01f);
            audioSource.volume = Mathf.Lerp(startVolume, target, t);
            yield return new WaitForSecondsRealtime(0.02f); // so that crossfade doesn't halt on game pause (timeScale 0)
        }
        audioSource.volume = target;
        if (target == 0) audioSource.Stop();
        volumeCoroutines[(int)bgm] = null;
    }
}
