using System;
using System.Collections;
using System.Collections.Generic;
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

    [System.Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;
        [Range(0f, 1f)]
        public float volume;
        public float minDistance;
        public float maxDistance;
    }

    private BGM currentBGM = BGM.Monochrome;
    [SerializeField] private AudioSource[] bgmSources;
    private Coroutine[] volumeCoroutines;
    [Range(0.0001f, 10)]
    [SerializeField] private float distortionEffectTime;
    [Range(0, 1)]
    [SerializeField] private float distortionEffectDistortion;
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private AudioMixerGroup SFXGroup;
    [SerializeField] private Sound[] sounds;
    private Dictionary<string, Sound> namesToSounds;

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
        SceneManager.sceneLoaded += (Scene scene, LoadSceneMode mode) =>
        {
            ChangeBGM(BGM.Monochrome);
        };
        bgmSources[(int)BGM.Monochrome].Play();
        namesToSounds = new Dictionary<string, Sound>();
        foreach (Sound sound in sounds)
        {
            namesToSounds.Add(sound.name, sound);
        }
    }

    private void Start()
    {
        UpdateMusicVolume();
        UpdateEffectsVolume();
    }

    public void PlaySFXAt(string name, Vector3 pos)
    {
        if (name == "CollectSpark")
        if (!namesToSounds.ContainsKey(name))
        {
            Debug.LogWarning("Sound " + name + " not found, no sound will be played");
            return;
        }
        Sound sound = namesToSounds[name];
        GameObject gameObject = new GameObject("One shot audio");
        gameObject.transform.position = pos;
        AudioSource source = gameObject.AddComponent<AudioSource>();
        source.clip = sound.clip;
        source.spatialBlend = 1f;
        source.volume = sound.volume;
        source.outputAudioMixerGroup = SFXGroup;
        source.minDistance = sound.minDistance;
        source.maxDistance = sound.maxDistance;
        source.Play();
        Destroy(gameObject, sound.clip.length);
    }

    public void PlaySFXGlobally(string name)
    {
        if (!namesToSounds.ContainsKey(name))
        {
            Debug.LogWarning("Sound " + name + " not found, no sound will be played");
            return;
        }
        Sound sound = namesToSounds[name];
        AudioSource source = gameObject.AddComponent<AudioSource>();
        source.clip = sound.clip;
        source.volume = sound.volume;
        source.outputAudioMixerGroup = SFXGroup;
        source.Play();

        Destroy(source, sound.clip.length);
    }

    public void UpdateMusicVolume()
    {
        SetMixerVolume("MusicVolume", Mathf.Max(PlayerPrefs.GetFloat("VolumeMusic", 1), 0.0001f));
    }
    public void UpdateEffectsVolume()
    {
        SetMixerVolume("SFXVolume", Mathf.Max(PlayerPrefs.GetFloat("VolumeEffects", 1), 0.0001f));
    }

    public Sound GetSound(string name)
    {
        if (!namesToSounds.ContainsKey(name))
        {
            Debug.LogWarning("Sound " + name + " not found, no sound will be returned");
            return null;
        }
        return namesToSounds[name];
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
        volumeCoroutines[(int)bgm] = StartCoroutine(SetVolumeSmoothly(bgm, 0.75f));
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
