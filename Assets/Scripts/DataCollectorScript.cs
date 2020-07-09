using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataCollectorScript : MonoBehaviour
{
    private HiveLevel HiveLevel;
    private BeesAmountScript BeesAmountScript;
    [HideInInspector]
    public int beesAmount;
    [HideInInspector]
    public float honeyAmount;
    [HideInInspector]
    public int hiveLevel;

    [HideInInspector] public int levelsUnlocked;

    private void Start()
    {
        BeesAmountScript = FindObjectOfType<BeesAmountScript>();
        HiveLevel = FindObjectOfType<HiveLevel>();
        levelsUnlocked = 1;
    }

    // Update is called once per frame
    void Update()
    {
        beesAmount = BeesAmountScript.InitialNumOfBees;
        //honeyAmount = HoneyCounter.getHoneyAmount();
        hiveLevel = HiveLevel.hiveLevel;
    }

    public void SaveData()
    {
        levelsUnlocked++;
        SaveSystem.SaveData(this);
    }

    public void LoadData()
    {
        GameData gameData = SaveSystem.LoadGameData();
        HiveLevel.hiveLevel = gameData.hiveLevel;
        BeesAmountScript.InitialNumOfBees = gameData.beesAmount;
        levelsUnlocked = gameData.levelsUnlocked;
    }
}
