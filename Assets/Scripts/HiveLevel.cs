﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HiveLevel : MonoBehaviour
{
    public GameObject EndLevelHelper;
    public static bool resultsActive = false;
    public static int hiveLevel;
    [SerializeField] Text levelNumber;
    public Slider slider;
    private BeesScript beesScript;
    [HideInInspector]
    public HoneyCounter honeyCounter;
    public DataCollectorScript dataCollector;

    public static float honeyAmount = 0.0f;
    public static float levelMaxValue = 0.0f;

    private float fillSpeed = 1.5f;
    private float startFillTime = 0.0f;

    private bool endStart = true;
    private bool filled = false;

    // Start is called before the first frame update
    void Start()
    {
        honeyCounter = FindObjectOfType<HoneyCounter>();
        beesScript = FindObjectOfType<BeesScript>();
        
        if (!dataCollector.LoadData())
        {
            hiveLevel = 1;
            levelMaxValue = slider.maxValue;
        }

        slider.maxValue = levelMaxValue;
        levelNumber.text = hiveLevel.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        float honeyCovered = (Time.time - startFillTime);
        float honeySmoothing = honeyCovered / fillSpeed;
        if (resultsActive && !filled)
        {
            EndLevelHelper.SetActive(false);
            if (endStart)
            {
                StartCoroutine(waitForMenu(0.5f));
            }
            else
            {
                slider.value = Mathf.Lerp(honeyAmount, honeyCounter.endHoneyAmount + honeyAmount, honeySmoothing);
                if (honeySmoothing >= 1.0f)
                {
                    honeyAmount += honeyCounter.endHoneyAmount;
                    filled = true;
                }
            }

            if (slider.value >= levelMaxValue)
            {
                LevelUp();
                slider.value = 0.0f;
            }

            if(filled)
            {
                int levelToUnlock = SceneManager.GetActiveScene().buildIndex + 1;
                if (!(dataCollector.levelsUnlocked == levelToUnlock) && SceneManager.sceneCountInBuildSettings - 1 >= levelToUnlock)
                {
                    dataCollector.levelsUnlocked = levelToUnlock;
                }
                dataCollector.SaveData();
            }
        }
    }

    IEnumerator waitForMenu(float seconds)
    {
        yield return new WaitForSecondsRealtime(seconds);
        endStart = false;
        startFillTime = Time.time;
    }

    private void LevelUp()
    {
        hiveLevel++;
        beesScript.amountOfBees += 20;
        levelNumber.text = hiveLevel.ToString();
        levelMaxValue *= 2.0f;
    }
}
