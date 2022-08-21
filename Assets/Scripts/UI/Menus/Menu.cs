using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class Menu : MonoBehaviour
{
    [SerializeField] GameObject defaultSelected;

    [Inject] protected MenuManager menuManager;

    void OnEnable()
    {
        if (defaultSelected)
            EventSystem.current.SetSelectedGameObject(defaultSelected);
    }

    public void GoBackOrResume()
    {
        menuManager.GoBackOrResume();
    }

    public void GoBack()
    {
        menuManager.GoBack();
    }

    public void OpenMenu(GameObject menu)
    {
        menuManager.OpenMenu(menu);
    }
}