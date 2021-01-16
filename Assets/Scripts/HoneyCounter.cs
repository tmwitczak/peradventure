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
    public float _smokeFactor = 1f;
    public BeesScript beesScript;
    public DataCollectorScript dataCollector;
    
    private float initialSmokeFactor;
    private int amountOfBees;
    private Text text;
    private float _honeyAmount;
    private float levelFactor = 1.0f;
    
    public float endHoneyAmount { get; set; }

    private void Start()
    {
        HoneyAmount = 0f;
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
        HoneyAmount += Time.deltaTime * ((amountOfBees / 10f) * SmokeFactor * levelFactor) * Speed;
        text.text = (Mathf.Round(16.54f * HoneyAmount)) + "";
    }

    public float HoneyAmount
    {
        get => _honeyAmount;
        set => _honeyAmount = Mathf.Max(0f, value);
    }

    public float SmokeFactor
    {
        get => _smokeFactor;
        set => _smokeFactor = Mathf.Clamp(value, MinSmokedSpeed, initialSmokeFactor);
    }
}
