using System.IO;
using UnityEngine;

public static class SaveSystem {
    public static string path = Application.persistentDataPath + "/gamedata.json";

    public static void SaveData(GameData gameData) {
        string jsonSaveData = JsonUtility.ToJson(gameData);
        File.WriteAllText(path, jsonSaveData);
    }

    public static GameData LoadGameData() {
        if (File.Exists(path)) {
            string jsonSaveData = File.ReadAllText(path);
            GameData gameData = JsonUtility.FromJson<GameData>(jsonSaveData);
            return gameData;
        } else {
            return null;
        }
    }
}
