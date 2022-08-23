using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    void Update()
    {
        // TODO: DELETE THIS BEFORE PUBLISHING
        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        else if (Input.GetKeyDown(KeyCode.P))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        else if (Input.GetKeyDown(KeyCode.N))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void PlayMusic(AudioClip clip, float fadeOutTime = 0f)
    {
        StartCoroutine(CO_PlayMusic(clip, fadeOutTime));
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