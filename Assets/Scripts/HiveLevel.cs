using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HiveLevel : MonoBehaviour
{
    public GameObject EndLevelHelper;
    public Slider slider;
    public DataCollectorScript dataCollector;
    private BeesScript beesScript;
    [SerializeField] 
    Text levelNumber;
    [HideInInspector]
    public HoneyCounter honeyCounter;
    public Image beeLevelUp;

    public static int hiveLevel;
    public static float levelMaxValue = 0.0f;
    public static float honeyAmount = 0.0f;

    private float previousLevelMaxValue = 0.0f;
    private float fillSpeed = 1.5f;
    private float startFillTime = 0.0f;
    private float honeyOverflow = 0.0f;
    private float honeyForSlider = 0.0f;

    private Text endText;
    private string[] endTextsList = { "Sublime!", "Brilliant!", "Great Job!", "Almost got it :("};

    private bool endStart = true;
    private bool overflowed = false;
    private bool beeAmountUp = false;
    private bool finishedLevel = true;
    public static bool resultsActive = false;

    private GameObject continueButton;
    private GameObject restartButton;

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
        previousLevelMaxValue = levelMaxValue;

        levelNumber.text = hiveLevel.ToString();
        honeyForSlider = honeyAmount;

        beeLevelUp.canvasRenderer.SetAlpha(0.0f);
        endText = gameObject.transform.Find("Text").GetComponent<Text>();
        continueButton = transform.GetChild(5).gameObject;
        restartButton = transform.GetChild(6).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        float honeyCovered = (Time.time - startFillTime);
        float honeySmoothing = honeyCovered / fillSpeed;
        if (resultsActive && !beeAmountUp)
        {
            EndLevelHelper.SetActive(false);
            if (endStart)
            {
                StartCoroutine(waitForMenu(0.5f));
            }
            else
            {
                if (honeySmoothing < 1.0f)
                {
                    slider.value = Mathf.Lerp(honeyForSlider, !overflowed ? (honeyCounter.endHoneyAmount + honeyForSlider) : honeyOverflow, honeySmoothing);
                }
            }

            if (slider.value >= previousLevelMaxValue)
            {
                slider.maxValue = levelMaxValue;
                levelNumber.text = hiveLevel.ToString();

                slider.value = 0.0f;
                honeyForSlider = 0.0f;

                overflowed = true;
                beeAmountUp = true;
            }
        }
        else if(beeAmountUp)
        {
            StartCoroutine(showBeeAmountUp());
        }
    }

    IEnumerator waitForMenu(float seconds)
    {
        if (honeyCounter.endHoneyAmount >= 110.0f)
        {
            endText.text = endTextsList[0];
        }
        else if (110.0f > honeyCounter.endHoneyAmount && honeyCounter.endHoneyAmount >= 80.0f)
        {
            endText.text = endTextsList[1];
        }
        else if (80.0f > honeyCounter.endHoneyAmount && honeyCounter.endHoneyAmount >= 60.0f)
        {
            endText.text = endTextsList[2];
        }
        else
        {
            endText.text = endTextsList[3];
            finishedLevel = false;
            restartButton.SetActive(true);
            continueButton.SetActive(false);
        }
        endStart = false;
        yield return new WaitForSecondsRealtime(seconds);
        startFillTime = Time.time;
        honeyAmount += honeyCounter.endHoneyAmount;

        if (honeyAmount >= levelMaxValue)
        {
            honeyOverflow = honeyAmount - levelMaxValue;
            LevelUp();
        }

        Save();
    }

    IEnumerator showBeeAmountUp()
    {
        beeLevelUp.CrossFadeAlpha(1.0f, 0.1f, false);
        yield return new WaitForSecondsRealtime(0.5f);
        beeLevelUp.CrossFadeAlpha(0.0f, 0.1f, false);
        beeAmountUp = false;
    }

    private void LevelUp()
    {
        hiveLevel++;
        beesScript.amountOfBees += 20;
        levelMaxValue *= 2.0f;
    }

    private void Save()
    {
        int levelToUnlock = SceneManager.GetActiveScene().buildIndex + 1;
        // SceneManager.sceneCountInBuildSettings - 1 >= levelToUnlock
        // leveltounlock <= 6 is temporary
        if (dataCollector.levelsUnlocked < levelToUnlock && levelToUnlock <= 6 && finishedLevel)
        {
            dataCollector.levelsUnlocked = levelToUnlock;
        }
        dataCollector.SaveData();
    }
}
