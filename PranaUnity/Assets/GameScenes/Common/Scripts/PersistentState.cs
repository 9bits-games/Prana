using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class PersistentState {

    public static void Sync() {
        PlayerPrefs.Save();
    }

    public static void ResetSate() {
        PlayerPrefs.DeleteAll();
    }

    private static string KEY_itemsCollectedForLevel(string LevelName) {
        return string.Format("level.{0}.items.collected", LevelName);
    }

    public static int GetItemsCollectedForLevel(string LevelName) {
        string key = KEY_itemsCollectedForLevel(LevelName);
        return PlayerPrefs.GetInt(key, 0);
    }

    public static void SetItemsCollectedForLevel(string LevelName, int number, bool overrideStored = false) {
        string key = KEY_itemsCollectedForLevel(LevelName);

        int stored = 0;
        if (!overrideStored) stored = GetItemsCollectedForLevel(LevelName);
        PlayerPrefs.SetInt(key, Mathf.Max(number, stored));
    }
}
