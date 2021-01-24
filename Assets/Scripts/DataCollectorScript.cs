using System.IO;
using UnityEngine;

public class DataCollectorScript : MonoBehaviour {
    public BeesScript beesScript;

    public float honeyAmount;
    public float levelMaxValue;
    public int amountOfBees;
    public int hiveLevel;
    public int levelsUnlocked;

    [ContextMenu("Clear save data")]
    private void ClearData() {
        if (File.Exists(SaveSystem.path)) {
            File.Delete(SaveSystem.path);
        }
    }

    [ContextMenu("Write save data")]
    public void SaveData() {
        amountOfBees = beesScript.getAmountOfBees();
        hiveLevel = HiveLevel.hiveLevel;
        honeyAmount = HiveLevel.honeyAmount;
        levelMaxValue = HiveLevel.levelMaxValue;

        SaveSystem.SaveData(this);
    }

    [ContextMenu("Read save data")]
    public bool LoadData() {
        GameData gameData = SaveSystem.LoadGameData();
        if (gameData == null) {
            return false;
        }

        HiveLevel.hiveLevel = gameData.hiveLevel;
        HiveLevel.honeyAmount = gameData.honeyAmount;
        HiveLevel.levelMaxValue = gameData.levelMaxValue;
        beesScript.setAmountOfBees(gameData.amountOfBees);

        amountOfBees = beesScript.getAmountOfBees();
        hiveLevel = gameData.hiveLevel;
        honeyAmount = gameData.honeyAmount;
        levelMaxValue = gameData.levelMaxValue;
        levelsUnlocked = gameData.levelsUnlocked;

        return true;
    }
}
