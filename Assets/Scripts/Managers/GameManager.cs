using System;
using UnityEngine;
using Zenject;

public enum GameState
{
    Playing,
    Paused,
}

public class GameManager : IInitializable
{
    public GameState State { get; private set; } = GameState.Playing;
    public event Action<bool> OnGamePauseChange;

    public void Initialize()
    {
        Time.timeScale = 1;
    }

    public void SetState(GameState newState)
    {
        if (newState == GameState.Paused)
        {
            Time.timeScale = 0;
            OnGamePauseChange?.Invoke(true);
        }
        else if (State == GameState.Paused)
        {
            Time.timeScale = 1;
            OnGamePauseChange?.Invoke(false);
        }

        State = newState;
    }
}