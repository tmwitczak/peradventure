using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Object = System.Object;

public class HoneyCounter : MonoBehaviour
{
    public float Speed;
    public float MinSmokedSpeed = 0.3f;
    public float SmokeFactor = 1f;
    private float initialSmokeFactor;
    private BeesAmountScript beesAmountScript;
    private int amountOfBees;
    private Text text;
    private float honeyAmount;
    private string initialText;
    
    public float endHoneyAmount { get; set; }
    private void Start()
    {
        beesAmountScript = FindObjectOfType<BeesAmountScript>();
        honeyAmount = 0f;
        text = GetComponent<Text>();
        initialText = text.text;
        initialSmokeFactor = SmokeFactor;
    }

    private void Update()
    {
        //Debug.Log(SmokeFactor);
        amountOfBees = beesAmountScript.ActualNumOfBees;
        honeyAmount += Time.deltaTime * (amountOfBees * SmokeFactor) * Speed;
        text.text = initialText + (Mathf.Round(honeyAmount) / 10.0f) + " l";
    }

    public void setHoneyAmount(float value)
    {
        honeyAmount = value;
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
