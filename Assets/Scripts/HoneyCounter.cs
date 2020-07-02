using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Object = System.Object;

public class HoneyCounter : MonoBehaviour
{
    public float Speed;
    public float SmokeFactor = 1f;
    private BeesAmountScript beesAmountScript;
    private int amountOfBees;
    private Text text;
    private float honeyAmount;
    private string initialText;
    
    public float endHoneyAmount { get; set; }
    private void Start()
    {
        beesAmountScript = FindObjectOfType<BeesAmountScript>();
        honeyAmount = 0.0f;
        text = GetComponent<Text>();
        initialText = text.text;
    }

    private void Update()
    {
        amountOfBees = beesAmountScript.ActualNumOfBees;
        honeyAmount += Time.deltaTime * (amountOfBees * SmokeFactor) * Speed;
        text.text = initialText + Mathf.Round(honeyAmount);
    }

    public void setHoneyAmount(float value)
    {
        honeyAmount = value;
    }
    public float getHoneyAmount()
    {
        return honeyAmount;
    }
}
