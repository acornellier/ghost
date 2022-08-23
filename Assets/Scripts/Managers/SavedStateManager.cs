using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using Zenject;

public class SavedState
{
    public string scene;
    public Dictionary<string, bool> bools = new();
}

public class SavedStateManager : IInitializable
{
    public SavedState SavedState { get; private set; }

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

    public void Update(SavedState savedState)
    {
        SavedState = savedState;
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
    }
}