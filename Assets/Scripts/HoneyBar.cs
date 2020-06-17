using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HoneyBar : MonoBehaviour
{
    public Slider slider;
    public float Speed;

    private void Update()
    {
        slider.value += Time.deltaTime * Speed;
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
