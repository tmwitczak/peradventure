using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataCollectorScript : MonoBehaviour
{
    private HiveLevel HiveLevel;
    
    public BeesScript BeesScript;
    [HideInInspector]
    public int amountOfBees;
    [HideInInspector]
    public float honeyAmount;
    [HideInInspector]
    public int hiveLevel;
    [HideInInspector] public int levelsUnlocked;

    private void Start()
    {
        HiveLevel = FindObjectOfType<HiveLevel>();
        BeesScript = FindObjectOfType<BeesScript>();
        levelsUnlocked = 1;
    }

    public void SaveData()
    {
        hiveLevel = HiveLevel.hiveLevel;
        amountOfBees = BeesScript.amountOfBees;
        honeyAmount = HiveLevel.honeyCounter.endHoneyAmount;
        SaveSystem.SaveData(this);
    }

    public void LoadData()
    {
        GameData gameData = SaveSystem.LoadGameData();
        BeesScript.amountOfBees = gameData.amountOfBees;
        HiveLevel.hiveLevel = gameData.hiveLevel;
        levelsUnlocked = gameData.levelsUnlocked;
    }
}
