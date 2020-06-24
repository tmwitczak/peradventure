using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Object = System.Object;

public class HoneyBar : MonoBehaviour
{
    public Slider slider;
    public float Speed;
    private BeesAmountScript beesAmountScript;
    private int amountOfBees;

    private void Start()
    {
        beesAmountScript = FindObjectOfType<BeesAmountScript>();
    }

    private void Update()
    {
        amountOfBees = beesAmountScript.ActualNumOfBees;
        slider.value += Time.deltaTime * amountOfBees * Speed;
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
