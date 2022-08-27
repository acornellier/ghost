using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;
using Object = UnityEngine.Object;

public class SavedState
{
    public string scene;
    public string nextSpawn;
    public Dictionary<string, bool> bools = new();
}

public class SavedStateManager
{
    [Inject] LevelLoader _levelLoader;

    SavedState _savedState;

    const string _key = "SavedState";
    const string _hardModeKey = "HardMode";
    const string _hardModeUnlockedKey = "HardModeUnlocked";

    public SavedState SavedState
    {
        get
        {
            if (_savedState == null)
                Initialize();
            return _savedState;
        }
        private set => _savedState = value;
    }

    static bool? _isHardMode;

    public static bool IsHardMode
    {
        get
        {
            _isHardMode ??= Convert.ToBoolean(PlayerPrefs.GetInt(_hardModeKey));
            return _isHardMode.Value;
        }
    }

    void Initialize()
    {
        var jsonString = PlayerPrefs.GetString(_key);
        if (jsonString != "")
        {
            SavedState = JsonConvert.DeserializeObject<SavedState>(jsonString);
            return;
        }

        SavedState = new SavedState();
        Save();
    }

    public void Save()
    {
        var jsonString = JsonConvert.SerializeObject(SavedState);
        PlayerPrefs.SetString(_key, jsonString);
        PlayerPrefs.Save();
    }

    public void Reset()
    {
        PlayerPrefs.DeleteKey(_key);
        Initialize();
    }

    public bool IsBoolSet(string key)
    {
        return SavedState.bools.TryGetValue(key, out var value) && value;
    }

    public void SetBool(string key, bool value = true)
    {
        SavedState.bools[key] = value;
    }

    public static void UnlockHardmode()
    {
        PlayerPrefs.SetInt(_hardModeUnlockedKey, Convert.ToInt32(true));
    }

    public static bool IsHardModeUnlocked()
    {
        return Convert.ToBoolean(PlayerPrefs.GetInt(_hardModeUnlockedKey));
    }

    public void ToggleHardMode()
    {
        _isHardMode = !_isHardMode;
        PlayerPrefs.SetInt("HardMode", Convert.ToInt32(_isHardMode));

        if (SceneManager.GetActiveScene().buildIndex != 0)
            _levelLoader.ReloadScene();
    }
}