using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    private DataCollectorScript dataCollector;

    public static float honeyAmount = 0.0f;
    private float fillSpeed = 1.5f;
    private float startFillTime = 0.0f;

    private bool endStart = true;
    private bool filled = false;

    // Start is called before the first frame update
    void Start()
    {
        honeyCounter = FindObjectOfType<HoneyCounter>();
        beesScript = FindObjectOfType<BeesScript>();
        dataCollector = FindObjectOfType<DataCollectorScript>();

        if (!dataCollector.LoadData())
        {
            hiveLevel = 1;
        }

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
                StartCoroutine(fillSlider(0.5f));
            }
            else
            {
                slider.value = Mathf.Lerp(honeyAmount, honeyCounter.endHoneyAmount + honeyAmount, honeySmoothing);
                if (slider.value == honeyCounter.endHoneyAmount + honeyAmount)
                {
                    honeyAmount += honeyCounter.endHoneyAmount;
                    filled = true;
                }
            }

            if (slider.value == slider.maxValue)
            {
                LevelUp();
                slider.value = 0.0f;
            }

            if(filled)
            {
                dataCollector.SaveData();
            }
        }
    }

    IEnumerator fillSlider(float seconds)
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
        slider.maxValue *= 5.0f;
    }
}
