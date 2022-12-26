using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioController : MonoBehaviour
{
    private AudioSource source;
    private float volume = 1;
    private bool crossFading = false;
    [SerializeField] private AudioClip defaultClip; // monochrome and menu clip
    [SerializeField] private float distortionEffectTime;
    [Range(0, 1)]
    [SerializeField] private float distortionEffectDistortion;
    private AudioDistortionFilter distortion;
    private AudioSource fadeOutSource;

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
        source = GetComponent<AudioSource>();
        UpdateVolume();
        SceneManager.sceneLoaded += (Scene scene, LoadSceneMode mode) =>
        {
            ChangeBGM(defaultClip);
        };
    }

    public IEnumerator SetDistorted(bool distorted)
    {
        float time = 0;
        if (distorted)
        {
            distortion = gameObject.AddComponent<AudioDistortionFilter>();
            distortion.distortionLevel = 0;
            while (time < distortionEffectTime)
            {
                yield return new WaitForFixedUpdate();
                time += Time.fixedDeltaTime;
                distortion.distortionLevel = Mathf.Lerp(0, distortionEffectDistortion, time / distortionEffectTime);
            }
            distortion.distortionLevel = 1;
        }
        else
        {
            while (time < distortionEffectTime)
            {
                yield return new WaitForFixedUpdate();
                time += Time.fixedDeltaTime;
                distortion.distortionLevel = Mathf.Lerp(distortionEffectDistortion, 0, time / distortionEffectTime);
            }
            distortion.distortionLevel = 0;
            Destroy(distortion);
        }
    }

    public void UpdateVolume()
    {
        volume = PlayerPrefs.GetFloat("Volume", 1);
        if (!crossFading) source.volume = volume;
    }

    public void ChangeBGM(AudioClip newBGM)
    {
        if(source.clip.name == newBGM.name)
        {
            return;
        }

        StartCoroutine(Crossfade(newBGM));
    }

    private IEnumerator Crossfade(AudioClip newTrack)
    {
        crossFading = true;
        // add new audiosource & grab data from original audiosource
        AudioSource fadeOutSource = gameObject.AddComponent<AudioSource>();
        fadeOutSource.clip = source.clip;
        fadeOutSource.time = source.time;
        fadeOutSource.volume= source.volume;
        fadeOutSource.outputAudioMixerGroup = source.outputAudioMixerGroup;

        // play it
        fadeOutSource.Play();

        // set original audiosource data, update with new clip
        source.volume = 0f;
        source.clip = newTrack;
        float t = 0;
        float v = volume == 0 ? 0 : fadeOutSource.volume / volume; // percentage of the volume to be reached
        source.Play();

        // fade in original source with new clip and fade out new source with old clip
        while(t < 0.98f)
        {
            t = Mathf.Lerp(t, 1f, Time.deltaTime * 0.5f);
            fadeOutSource.volume = Mathf.Lerp(v, 0f, t) * volume; // take global volume into account with the percentage
            source.volume = Mathf.Lerp(0f, v, t) * volume;
            yield return null;
        }

        source.volume = volume;

        // destroy faded source
        Destroy(fadeOutSource);
        crossFading = false;
        yield break;
    }
}
