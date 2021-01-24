[System.Serializable]
public class GameData {
    public float honeyAmount;
    public float levelMaxValue;
    public int amountOfBees;
    public int hiveLevel;
    public int levelsUnlocked;

    public GameData(DataCollectorScript dataCollector) {
        amountOfBees = dataCollector.amountOfBees;
        hiveLevel = dataCollector.hiveLevel;
        honeyAmount = dataCollector.honeyAmount;
        levelMaxValue = dataCollector.levelMaxValue;
        levelsUnlocked = dataCollector.levelsUnlocked;
    }
}
