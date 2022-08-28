using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class DebugShortcuts : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            if (Screen.fullScreen)
                Screen.SetResolution(1920, 1080, false);
            else
                Screen.fullScreen = true;
        }
    }
}