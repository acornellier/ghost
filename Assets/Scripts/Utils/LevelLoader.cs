using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip transitionClip;

    [SerializeField] Image transitionImage;
    [SerializeField] float transitionTime = 1f;

    public bool isLoaded;

    void Start()
    {
        StartCoroutine(StartLevel());
    }

    IEnumerator StartLevel()
    {
        yield return StartCoroutine(CO_StartScene());
        isLoaded = true;
    }

    public void LoadScene(string scene)
    {
        StartCoroutine(CO_EndLevel(scene));
    }

    IEnumerator CO_EndLevel(string scene)
    {
        audioSource.PlayOneShot(transitionClip);
        yield return CO_EndScene();
        SceneManager.LoadScene(scene);
    }

    IEnumerator CO_StartScene()
    {
        var color = transitionImage.color;
        color.a = 1;

        var t = 0f;
        while (t < transitionTime)
        {
            t += Time.deltaTime;
            color.a = 1 - Mathf.Clamp01(t / transitionTime);
            transitionImage.color = color;
            yield return null;
        }
    }

    IEnumerator CO_EndScene()
    {
        var color = transitionImage.color;
        color.a = 0;

        var t = 0f;
        while (t < transitionTime)
        {
            t += Time.deltaTime;
            color.a = Mathf.Clamp01(t / transitionTime);
            transitionImage.color = color;
            yield return null;
        }
    }
}