using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicPlayer : MonoBehaviour
{
    [NonSerialized] public AudioClip defaultMusic;

    AudioSource _source;

    void Awake()
    {
        _source = GetComponent<AudioSource>();
        defaultMusic = _source.clip;
    }

    public void PlayMusic(AudioClip clip, float fadeOutTime = 0f)
    {
        StartCoroutine(CO_PlayMusic(clip, fadeOutTime));
    }

    public void PlayDefaultIfNotAlreadyPlaying()
    {
        if (_source.clip != defaultMusic)
            StartCoroutine(CO_PlayMusic(defaultMusic));
    }

    IEnumerator CO_PlayMusic(AudioClip clip, float fadeOutTime = 0f)
    {
        var t = 0f;
        while (_source.volume > 0)
        {
            t += Time.deltaTime;
            _source.volume = Mathf.Clamp01(1 - t / fadeOutTime);
            yield return null;
        }

        _source.clip = clip;
        _source.volume = 1;
        _source.Play();
    }
}