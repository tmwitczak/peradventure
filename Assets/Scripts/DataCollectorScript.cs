using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataCollectorScript : MonoBehaviour
{
    private HiveLevel HiveLvl;
    private BeesScript BeesScript;

    [HideInInspector]
    public int amountOfBees;
    [HideInInspector]
    public float honeyAmount;
    [HideInInspector]
    public int hiveLevel;
    [HideInInspector] 
    public int levelsUnlocked;

    private void Start()
    {
        HiveLvl = FindObjectOfType<HiveLevel>();
        BeesScript = FindObjectOfType<BeesScript>();
        levelsUnlocked = 1;
    }

    public void SaveData()
    {
        hiveLevel = HiveLevel.hiveLevel;
        amountOfBees = BeesScript.amountOfBees;
        honeyAmount = HiveLevel.honeyAmount;
        SaveSystem.SaveData(this);
    }

    public bool LoadData()
    {
        if (SaveSystem.LoadGameData() == null)
        {
            return false;
        }

        GameData gameData = SaveSystem.LoadGameData();
        BeesScript.amountOfBees = gameData.amountOfBees;
        HiveLevel.hiveLevel = gameData.hiveLevel;
        HiveLevel.honeyAmount = gameData.honeyAmount;
        levelsUnlocked = gameData.levelsUnlocked;

        return true;
    }
}
