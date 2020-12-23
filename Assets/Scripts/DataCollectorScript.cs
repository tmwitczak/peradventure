﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataCollectorScript : MonoBehaviour
{
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
        BeesScript = FindObjectOfType<BeesScript>();
    }


    [ContextMenu("Clear save data")]
    private void ClearData()
    {
        if (File.Exists(SaveSystem.path))
        {
            File.Delete(SaveSystem.path);
        }
    }

    public void SaveData()
    {
        hiveLevel = HiveLevel.hiveLevel;
        amountOfBees = BeesScript.amountOfBees;
        honeyAmount = HiveLevel.honeyAmount;
        levelMaxValue = HiveLevel.levelMaxValue;
        SaveSystem.SaveData(this);
    }

    public bool LoadData()
    {
        if (SaveSystem.LoadGameData() == null)
        {
            return false;
        }

        GameData gameData = SaveSystem.LoadGameData();
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            levelsUnlocked = gameData.levelsUnlocked;
        }else
        {
            BeesScript.amountOfBees = gameData.amountOfBees;
            HiveLevel.hiveLevel = gameData.hiveLevel;
            HiveLevel.levelMaxValue = gameData.levelMaxValue;
            HiveLevel.honeyAmount = gameData.honeyAmount;
            levelsUnlocked = gameData.levelsUnlocked;
        }
        //Debug.Log("Ilosc pszczol: " + gameData.amountOfBees + '\n' + "Level ula: " + gameData.hiveLevel + '\n' + "Ilosc miodu: " + gameData.honeyAmount + '\n' + "Odblokowane poziomy: " + gameData.levelsUnlocked + '\n' + "SliderMaxVal: " + gameData.levelMaxValue);

        return true;
    }
}
