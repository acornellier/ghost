using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class SavedState
{
    public string scene;
    public string nextSpawn;
    public Dictionary<string, bool> bools = new();
}

public class SavedStateManager
{
    SavedState _savedState;

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

    const string _key = "SavedState";

    public void Initialize()
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
}