using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicPlayer : MonoBehaviour
{
    AudioSource _source;
    AudioClip _defaultMusic;

    void Awake()
    {
        _source = GetComponent<AudioSource>();
        _defaultMusic = _source.clip;
    }

    public void PlayMusic(AudioClip clip, float fadeOutTime = 0f)
    {
        if (_source.clip != clip)
            StartCoroutine(CO_PlayMusic(clip, fadeOutTime));
    }

    public void PlayDefaultMusic(float fadeOutTime = 0f)
    {
        if (_source.clip != _defaultMusic)
            StartCoroutine(CO_PlayMusic(_defaultMusic, fadeOutTime));
    }

    IEnumerator CO_PlayMusic(AudioClip clip, float fadeOutTime = 0f)
    {
        if (fadeOutTime > 0)
        {
            var t = 0f;
            while (_source.volume > 0)
            {
                t += Time.deltaTime;
                _source.volume = Mathf.Clamp01(1 - t / fadeOutTime);
                yield return null;
            }
        }

        _source.clip = clip;
        _source.volume = 1;
        _source.Play();
    }
}