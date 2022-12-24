using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public AudioSource source;

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
        float v = fadeOutSource.volume;
        source.Play();

        // fade in original source with new clip and fade out new source with old clip
        while(t < 0.98f)
        {
            t = Mathf.Lerp(t, 1f, Time.deltaTime * 0.5f);
            fadeOutSource.volume = Mathf.Lerp(v, 0f, t);
            source.volume = Mathf.Lerp(0f, v, t);
            yield return null;
        }

        source.volume = v;

        // destroy faded source
        Destroy(fadeOutSource);
        yield break;
    }
}
