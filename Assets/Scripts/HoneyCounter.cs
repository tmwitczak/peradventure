using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HoneyCounter : MonoBehaviour
{
    public float Speed;
    public float MinSmokedSpeed = 0.3f;
    public float SmokeFactor = 1f;
    public BeesScript beesScript;
    public DataCollectorScript dataCollector;
    
    private float initialSmokeFactor;
    private int amountOfBees;
    private Text text;
    private float honeyAmount;
    private float levelFactor = 1.0f;
    
    public float endHoneyAmount { get; set; }

    private void Start()
    {
        honeyAmount = 0f;
        text = GetComponent<Text>();
        initialSmokeFactor = SmokeFactor;
    }

    private void Update()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "Level1":
                if (dataCollector.levelsUnlocked <= 1)
                {
                    levelFactor = 1.1f;
                }
                else levelFactor = 0.6f;
                break;
            case "Level2":
                levelFactor = 0.75f;
                break;
            case "Level3":
                levelFactor = 0.85f;
                break;
            case "Level4":
                levelFactor = 1.0f;
                break;
        }
        amountOfBees = beesScript.getAmountOfBees();
        honeyAmount += Time.deltaTime * ((amountOfBees / 10f) * SmokeFactor * levelFactor) * Speed;
        text.text = (Mathf.Round(16.54f * honeyAmount)) + "";
    }

    public void setHoneyAmount(float value)
    {
        honeyAmount = value <= 0f ? 0f : value;
    }
    public float getHoneyAmount()
    {
        return honeyAmount;
    }

    public float getSmokeFactor()
    {
        return SmokeFactor;
    }

    public void setSmokeFactor(float value)
    {
        if (value >= MinSmokedSpeed && value <= initialSmokeFactor)
        {
            SmokeFactor = value;
        }
        else if (value < MinSmokedSpeed) SmokeFactor = MinSmokedSpeed;
        else SmokeFactor = initialSmokeFactor;
    }
}
