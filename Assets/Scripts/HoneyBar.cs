using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Object = System.Object;

public class HoneyBar : MonoBehaviour
{
    [SerializeField] GameObject EndGameMenu;
    public Slider slider;
    public float Speed;
    private BeesAmountScript beesAmountScript;
    private int amountOfBees;
    public static float endHoneyAmount = 0.0f;

    private void Start()
    {
        beesAmountScript = FindObjectOfType<BeesAmountScript>();
    }

    private void Update()
    {
        amountOfBees = beesAmountScript.ActualNumOfBees;
        slider.value += Time.deltaTime * amountOfBees * Speed;
        if (slider.value == slider.maxValue || TimerScript.TimeLeft == 0.0f)
        {
            endHoneyAmount = slider.value;
            EndGameMenu.SetActive(true);
            HiveLevel.resultsActive = true;
        }
    }
    public void setMaxHoneyAmount(float value)
    {
        slider.maxValue = value;
    }

    public void setHoneyAmount(float value)
    {
        slider.value = value;
    }
    public float getHoneyAmount()
    {
        return slider.value;
    }
}
