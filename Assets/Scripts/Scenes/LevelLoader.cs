using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip transitionClip;

    [SerializeField] Image transitionImage;
    [SerializeField] float transitionTime = 1f;

    [Inject] SavedStateManager _savedStateManager;

    public void StartScene()
    {
        StartCoroutine(CO_StartScene());
    }

    public void ReloadScene()
    {
        StartCoroutine(CO_LoadScene(SceneManager.GetActiveScene().name));
    }

    public void LoadScene(string scene, string nextSpawn = null)
    {
        _savedStateManager.SavedState.nextSpawn = nextSpawn;
        _savedStateManager.SavedState.scene = scene;
        _savedStateManager.Save();
        audioSource.PlayOneShot(transitionClip);

        StartCoroutine(CO_LoadScene(scene));
    }

    IEnumerator CO_StartScene()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
            yield break;

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

    IEnumerator CO_LoadScene(string scene)
    {
        Time.timeScale = 1;

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

        SceneManager.LoadScene(scene);
    }
}