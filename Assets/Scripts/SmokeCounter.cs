using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SmokeCounter : MonoBehaviour
{
    private HoneyCounter honeyCounter;
    private Text text;
    private float smokeAmount = 0f;
    private string initialText;
    // Start is called before the first frame update
    void Start()
    {
        honeyCounter = FindObjectOfType<HoneyCounter>();
        text = GetComponent<Text>();
        initialText = text.text;
    }

    // Update is called once per frame
    void Update()
    {
        text.text = initialText + ComputeSmokeScreen() + " %";
    }

    float ComputeSmokeScreen()
    {
        smokeAmount = honeyCounter.getSmokeFactor() * 2f;
        return (Mathf.Round(smokeAmount * 10f) / 2f) * 10f;
    }
}
