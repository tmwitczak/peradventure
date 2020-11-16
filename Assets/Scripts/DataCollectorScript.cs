using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    }

    public void SaveData()
    {
        hiveLevel = HiveLevel.hiveLevel;
        amountOfBees = BeesScript.amountOfBees;
        honeyAmount = HiveLevel.honeyAmount;
        levelMaxValue = HiveLevel.levelMaxValue;
        SaveSystem.SaveData(this);
        Debug.Log("zapisano pszczol" + BeesScript.amountOfBees + "tyle przypisano:" + amountOfBees);
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
        }
        //Debug.Log("Ilosc pszczol: " + gameData.amountOfBees + '\n' + "Level ula: " + gameData.hiveLevel + '\n' + "Ilosc miodu: " + gameData.honeyAmount + '\n' + "Odblokowane poziomy: " + gameData.levelsUnlocked + '\n' + "SliderMaxVal: " + gameData.levelMaxValue);

        return true;
    }
}
