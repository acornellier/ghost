using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

// TODO: DELETE THIS BEFORE PUBLISHING
public class DebugShortcuts : MonoBehaviour
{
    [Inject] SavedStateManager _savedStateManager;

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.R))
        {
            _savedStateManager.Reset();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}