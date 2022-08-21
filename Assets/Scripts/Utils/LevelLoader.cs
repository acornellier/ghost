using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider2D))]
public class LevelLoader : MonoBehaviour
{
    [SerializeField] Material screenTransitionMaterial;
    [SerializeField] string progressProperty;
    [SerializeField] string flipProperty;
    [SerializeField] float transitionTime = 1f;

    public bool isLoaded;

    void Start()
    {
        screenTransitionMaterial.SetInt(flipProperty, 0);
        StartCoroutine(StartLevel());
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.GetComponent<Player>())
            return;

        EndLevel();
    }

    IEnumerator StartLevel()
    {
        yield return StartCoroutine(CO_TransitionScene());
        isLoaded = true;
    }

    public void EndLevel()
    {
        StartCoroutine(CO_EndLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    public void LoadScene(int buildIndex)
    {
        StartCoroutine(CO_EndLevel(buildIndex));
    }

    IEnumerator CO_EndLevel(int buildIndex)
    {
        screenTransitionMaterial.SetInt(flipProperty, 1);
        yield return CO_TransitionScene();
        SceneManager.LoadScene(buildIndex);
    }

    IEnumerator CO_TransitionScene()
    {
        var t = 0f;
        while (t < transitionTime)
        {
            t += Time.deltaTime;
            var value = Mathf.Clamp01(t / transitionTime);
            screenTransitionMaterial.SetFloat(progressProperty, value);
            yield return null;
        }
    }
}