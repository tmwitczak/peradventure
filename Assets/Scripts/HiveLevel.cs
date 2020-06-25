﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HiveLevel : MonoBehaviour
{
    public static bool resultsActive = false;
    public static int hiveLevel;
    [SerializeField] Text levelNumber;
    public Slider slider;

    private float fillSpeed = 1.5f;
    private float honeyAmount = 0.0f;
    private float startFillTime = 0.0f;

    private bool endStart = true;
    private bool filled = false;

    // Start is called before the first frame update
    void Start()
    {
        hiveLevel = 1;
        levelNumber.text = hiveLevel.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        float honeyCovered = (Time.time - startFillTime);
        float honeySmoothing = honeyCovered / fillSpeed;
        if (resultsActive && !filled)
        {
            if (endStart)
            {
                StartCoroutine(fillSlider());
            }
            else
            {
                slider.value = Mathf.Lerp(honeyAmount, HoneyBar.endHoneyAmount, honeySmoothing);
                if (slider.value == HoneyBar.endHoneyAmount)
                {
                    honeyAmount = HoneyBar.endHoneyAmount;
                    filled = true;
                }
            }
        }

        if(slider.value == slider.maxValue)
        {
            LevelUp();
            slider.value = 0.0f;
        }
    }

    IEnumerator fillSlider()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        endStart = false;
        startFillTime = Time.time;
    }

    private void LevelUp()
    {
        hiveLevel++;
        slider.maxValue *= 2.0f;
    }
}
