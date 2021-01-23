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
    public BeesScript beesScript;
    private LevelManager levelManager;
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
    private string[] endTextsList = { "sublime!", "brilliant!", "great job!", "almost\ngot it :("};

    private bool overflowed = false;
    public static bool finishedLevel = true;
    public bool beeAmountUp = false;
    public static bool resultsActive = false;
    public static bool endStart = true;

    public GameObject continueButton;
    public GameObject restartButton;

    [SerializeField] GameObject Overlay;
    [SerializeField] Animator BackgroundOverlay;

    // Start is called before the first frame update
    void OnEnable()
    {
        honeyCounter = FindObjectOfType<HoneyCounter>();
        levelManager = FindObjectOfType<LevelManager>();

        slider.maxValue = levelMaxValue;
        previousLevelMaxValue = levelMaxValue;

        levelNumber.text = hiveLevel.ToString();
        honeyForSlider = honeyAmount;

        beeLevelUp.canvasRenderer.SetAlpha(0.0f);
        endText = gameObject.transform.Find("Text").GetComponent<Text>();

        // Test for lighting change and smooth background transitions
        Vector3 a = GameObject.Find("Sun").GetComponent<Transform>().eulerAngles;
        a.x += 30.0f;
        GameObject.Find("Sun").GetComponent<Transform>().eulerAngles = a;
        GameObject.Find("PreRenderBackgroundController").GetComponent<PreRenderBackground>().refresh();

        Overlay.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        float honeyCovered = (Time.time - startFillTime);
        float honeySmoothing = honeyCovered / fillSpeed;
        if (resultsActive && !beeAmountUp)
        {
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
        EndLevelHelper.SetActive(false);
        if (honeyCounter.endHoneyAmount >= 110.0f)
        {
            endText.text = endTextsList[0];
        }
        else if (110.0f > honeyCounter.endHoneyAmount && honeyCounter.endHoneyAmount >= 80.0f)
        {
            endText.text = endTextsList[1];
        }
        else if (80.0f > honeyCounter.endHoneyAmount && honeyCounter.endHoneyAmount >= 40.0f)
        {
            endText.text = endTextsList[2];
        }
        else
        {
            endText.text = endTextsList[3];
            finishedLevel = false;
            restartButton.SetActive(true);
            continueButton.SetActive(false);
            endText.gameObject.SetActive(true);
        }
        endStart = false;
        Debug.Log(endStart);

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
        beesScript.addAmountOfBees(20);
        levelMaxValue *= 2.0f;
    }

    private void Save()
    {
        if (finishedLevel)
        {
            dataCollector.levelsUnlocked += 1;
        }
        dataCollector.SaveData();
    }
}
