using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HoneyCounter : MonoBehaviour
{
    public float Speed;
    public float MinSmokedSpeed = 0.3f;
    public float SmokeFactor = 1f;
    public BeesScript beesScript;
    
    private float initialSmokeFactor;
    private int amountOfBees;
    private Text text;
    private float honeyAmount;
    private string initialText;
    
    public float endHoneyAmount { get; set; }
    private void Start()
    {
        honeyAmount = 0f;
        text = GetComponent<Text>();
        initialText = text.text;
        initialSmokeFactor = SmokeFactor;
        amountOfBees = beesScript.amountOfBees;
    }

    private void Update()
    {
        Debug.Log(SmokeFactor);
        honeyAmount += Time.deltaTime * ((amountOfBees / 10f) * SmokeFactor) * Speed;
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
