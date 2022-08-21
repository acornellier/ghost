using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MenuManager : IInitializable
{
    [Inject] GameManager _gameManager;

    PlayerInputActions _playerControls;

    readonly Stack<GameObject> _menuStack = new();

    public void Initialize()
    {
        _playerControls = new PlayerInputActions();
        _playerControls.UI.GoBack.performed += _ => OnPauseInput();
    }

    void OnPauseInput()
    {
        GoBackOrResume();
    }

    public void CloseAll()
    {
        while (_menuStack.TryPop(out var menu))
        {
            menu.SetActive(false);
        }
    }

    public void OpenMenu(GameObject menu)
    {
        _playerControls.UI.GoBack.Enable();

        if (_menuStack.TryPeek(out var oldMenu))
            oldMenu.SetActive(false);

        _menuStack.Push(menu);
        menu.SetActive(true);
    }

    public void GoBackOrResume()
    {
        if (_menuStack.Count <= 0)
            return;

        GoBack();
        if (_menuStack.Count <= 0)
        {
            _gameManager.SetState(GameState.Playing);
            _playerControls.UI.GoBack.Disable();
        }
    }

    public void GoBack()
    {
        if (_menuStack.TryPop(out var oldMenu))
            oldMenu.SetActive(false);

        if (_menuStack.TryPeek(out var newMenu))
            newMenu.SetActive(true);
    }
}