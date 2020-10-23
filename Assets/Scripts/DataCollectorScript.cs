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
    public float levelMaxValue;
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
        levelMaxValue = HiveLevel.levelMaxValue;
        SaveSystem.SaveData(this);

        //Debug.Log("Ilosc pszczol: " + amountOfBees + '\n' + "Level ula: " + hiveLevel + '\n' + "Ilosc miodu: " + honeyAmount);

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
        HiveLevel.levelMaxValue = gameData.levelMaxValue;
        HiveLevel.honeyAmount = gameData.honeyAmount;
        levelsUnlocked = gameData.levelsUnlocked;

        //Debug.Log("Ilosc pszczol: " + gameData.amountOfBees + '\n' + "Level ula: " + gameData.hiveLevel + '\n' + "Ilosc miodu: " + gameData.honeyAmount + '\n' + "Odblokowane poziomy: " + gameData.levelsUnlocked + '\n' + "SliderMaxVal: " + gameData.levelMaxValue);

        return true;
    }
}
