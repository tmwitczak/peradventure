using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HiveLevel : MonoBehaviour {
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private HoneyCounter honeyCounter;

    [HideInInspector]
    [SerializeField] Text levelNumber;
    public BeesScript beesScript;
    public GameObject EndLevelHelper;
    public Image beeLevelUp;
    public Slider slider;

    private float fillSpeed = 1.5f;
    private float honeyForSlider = 0.0f;
    private float honeyOverflow = 0.0f;
    private float previousLevelMaxValue = 0.0f;
    private float startFillTime = 0.0f;

    private Text endText;
    private string[] endTextsList = { "sublime!", "brilliant!", "great job!", "almost\ngot it :(" };

    private bool overflowed = false;
    public bool beeAmountUp = false;
    public static bool endStart = true;
    public static bool finishedLevel = true;
    public static bool resultsActive = false;

    public GameObject continueButton;
    public GameObject restartButton;

    [SerializeField] Animator BackgroundOverlay;
    [SerializeField] GameObject Overlay;

    void OnEnable() {
        slider.maxValue = Global.levelMaxValue;
        previousLevelMaxValue = Global.levelMaxValue;

        levelNumber.text = Global.hiveLevel.ToString();
        honeyForSlider = Global.honeyAmount;

        beeLevelUp.canvasRenderer.SetAlpha(0.0f);
        endText = gameObject.transform.Find("Text").GetComponent<Text>();

        // Test for lighting change and smooth background transitions
        Vector3 a = GameObject.Find("Sun").GetComponent<Transform>().eulerAngles;
        a.x += 30.0f;
        GameObject.Find("Sun").GetComponent<Transform>().eulerAngles = a;
        GameObject.Find("PreRenderBackgroundController").GetComponent<PreRenderBackground>().refresh();

        Overlay.SetActive(true);
    }

    void Update() {
        float honeyCovered = (Time.time - startFillTime);
        float honeySmoothing = honeyCovered / fillSpeed;
        if (resultsActive && !beeAmountUp) {
            if (endStart) {
                StartCoroutine(waitForMenu(0.5f));
            } else {
                if (honeySmoothing < 1.0f) {
                    slider.value = Mathf.Lerp(honeyForSlider,
                            !overflowed ? (honeyCounter.endHoneyAmount + honeyForSlider) : honeyOverflow, honeySmoothing);
                }
            }

            if (slider.value >= previousLevelMaxValue) {
                slider.maxValue = Global.levelMaxValue;
                levelNumber.text = Global.hiveLevel.ToString();

                slider.value = 0.0f;
                honeyForSlider = 0.0f;

                overflowed = true;
                beeAmountUp = true;
            }
        } else if (beeAmountUp) {
            StartCoroutine(showBeeAmountUp());
        }
    }

    IEnumerator waitForMenu(float seconds) {
        EndLevelHelper.SetActive(false);
        if (honeyCounter.endHoneyAmount >= 110.0f) {
            endText.text = endTextsList[0];
        } else if (110.0f > honeyCounter.endHoneyAmount && honeyCounter.endHoneyAmount >= 80.0f) {
            endText.text = endTextsList[1];
        } else if (80.0f > honeyCounter.endHoneyAmount && honeyCounter.endHoneyAmount >= 40.0f) {
            endText.text = endTextsList[2];
        } else {
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
        Global.honeyAmount += honeyCounter.endHoneyAmount;

        if (Global.honeyAmount >= Global.levelMaxValue) {
            honeyOverflow = Global.honeyAmount - Global.levelMaxValue;
            LevelUp();
        }
        Save();
    }

    IEnumerator showBeeAmountUp() {
        beeLevelUp.CrossFadeAlpha(1.0f, 0.1f, false);
        yield return new WaitForSecondsRealtime(0.5f);
        beeLevelUp.CrossFadeAlpha(0.0f, 0.1f, false);
        beeAmountUp = false;
    }

    private void LevelUp() {
        Global.hiveLevel++;
        Global.amountOfBees += 20;
        Global.levelMaxValue *= 2.0f;
    }

    private void Save() {
        if (finishedLevel) {
            Global.levelsUnlocked += 1;
        }
        Global.SaveData();
    }
}
