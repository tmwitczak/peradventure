using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int levelsUnlocked;
    public int hiveLevel;
    public float honeyAmount;
    public int beesAmount;

    public GameData(DataCollectorScript dataCollector)
    {
        levelsUnlocked = dataCollector.levelsUnlocked;
        hiveLevel = dataCollector.hiveLevel;
        honeyAmount = dataCollector.honeyAmount;
        beesAmount = dataCollector.beesAmount;
    }
}
