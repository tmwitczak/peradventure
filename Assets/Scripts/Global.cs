using System.IO;
using UnityEngine;

public class Global : MonoBehaviour {
    private static GameData gameData;
    [SerializeField] private GameData lookupData;

    public static float honeyAmount {
        get => gameData._honeyAmount;
        set => gameData._honeyAmount = value;
    }
    public static float levelMaxValue {
        get => gameData._levelMaxValue;
        set => gameData._levelMaxValue = value;
    }
    public static int amountOfBees {
        get => gameData._amountOfBees;
        set => gameData._amountOfBees = value;
    }
    public static int hiveLevel {
        get => gameData._hiveLevel;
        set => gameData._hiveLevel = value;
    }
    public static int currentGameplayLevel {
        get => gameData._currentGameplayLevel;
        set => gameData._currentGameplayLevel = value;
    }
    public static Vector2 screenMinWorldPoint {
        get => Camera.main.ScreenToWorldPoint(new Vector2(0, 0));
    }
    public static Vector2 screenMaxWorldPoint {
        get => Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
    }
    public static float fmod(float a, float b) {
        return a - b * Mathf.Floor(a / b);
    }
    public static int mod(int a, int b) {
        return (int)fmod(a, b);
    }

    private void Awake() {
        LoadData();
        lookupData = gameData;
    }

    [ContextMenu("Lookup save data")]
    private void ContextMenuLookupData() {
        lookupData = gameData;
    }

    [ContextMenu("Clear save data")]
    private void ContextMenuClearData() {
        ClearData();
    }

    [ContextMenu("Write save data")]
    private void ContextMenuSaveData() {
        SaveData();
    }

    [ContextMenu("Read save data")]
    private void ContextMenuLoadData() {
        LoadData();
    }

    public static void ClearData() {
        if (File.Exists(SaveSystem.path)) {
            File.Delete(SaveSystem.path);
        }
    }

    public static void SaveData() {
        SaveSystem.SaveData(gameData);
    }

    public static bool LoadData() {
        gameData = SaveSystem.LoadGameData();

        if (gameData == null) {
            gameData = new GameData();
            return false;
        }

        return true;
    }
}
