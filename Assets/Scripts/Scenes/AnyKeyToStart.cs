using System.Collections;
using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using Zenject;

public class AnyKeyToStart : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    [SerializeField] UnityEvent unityEvent;

    [Inject] SavedStateManager _savedStateManager;

    Coroutine _fadeInCoroutine;

    public void Start()
    {
        StartCoroutine(CO_Start());
    }

    IEnumerator CO_Start()
    {
        text.alpha = 0;
        yield return new WaitForSeconds(1);

        InputSystem.onAnyButtonPress.CallOnce(
            (_) =>
            {
                // TODO: probably bad
                _savedStateManager.Reset();

                unityEvent.Invoke();

                if (_fadeInCoroutine != null)
                    StopCoroutine(_fadeInCoroutine);

                StartCoroutine(FadeOutText());
            }
        );

        _fadeInCoroutine = StartCoroutine(FadeInText());
    }

    IEnumerator FadeInText()
    {
        var t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime;
            text.alpha = Mathf.Lerp(0, 1, t);
            yield return null;
        }
    }

    IEnumerator FadeOutText()
    {
        var t = 0f;
        while (t < 0.5)
        {
            t += Time.deltaTime;
            text.alpha = Mathf.Lerp(1, 0, t * 2);
            yield return null;
        }
    }
}