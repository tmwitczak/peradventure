using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HiveLevel : MonoBehaviour {
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private HoneyCounter honeyCounter;

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

    public Animator animator;

    void OnEnable() {
        slider.maxValue = Global.levelMaxValue;
        previousLevelMaxValue = Global.levelMaxValue;

        honeyForSlider = Global.honeyAmount;

        beeLevelUp.canvasRenderer.SetAlpha(0.0f);
        endText = gameObject.transform.Find("Text").GetComponent<Text>();

        // Test for lighting change and smooth background transitions
        Vector3 a = GameObject.Find("Sun").GetComponent<Transform>().eulerAngles;
        a.x += 30.0f;
        GameObject.Find("Sun").GetComponent<Transform>().eulerAngles = a;
        GameObject.Find("PreRenderBackgroundController").GetComponent<PreRenderBackground>().refresh();

        Overlay.SetActive(true);

        iTween.Init(gameObject);
    }

    public void Resume()
    {
        animator.SetTrigger("FadeOut");
        BackgroundOverlay.SetTrigger("FadeOut");
        // iTween.Stop(gameObject);
        // iTween.ValueTo(gameObject, iTween.Hash(
        //     "from", 0.0f,
        //     "to", 1.0f,
        //     "time", 0.5f,
        //     "ignoretimescale", true,
        //     "onupdatetarget", gameObject,
        //     "onupdate", "tweenOnUpdateCallBack",
        //     "easetype", iTween.EaseType.easeOutQuad
        //     )
        // );
        levelManager.LoadLevel();
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }

    private void Update() {
        if (resultsActive && !beeAmountUp) {
            if (endStart) {
                StartCoroutine(waitForMenu(0.0f));
            } else {
            }

            if (Global.honeyAmount >= Global.levelMaxValue) {
                honeyOverflow = Global.honeyAmount - Global.levelMaxValue;
                LevelUp();
                beeAmountUp = true;
                Global.SaveData();
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

        iTween.Stop(gameObject);
        iTween.ValueTo(gameObject, iTween.Hash(
            "from", Global.honeyAmount,
            "to", Global.honeyAmount + honeyCounter.endHoneyAmount,
            "time", 3.0f,
            "ignoretimescale", true,
            "onupdatetarget", gameObject,
            "onupdate", "tweenOnUpdateCallBack",
            "oncompletetarget", gameObject,
            "oncomplete", "tweenOnCompleteCallback",
            "easetype", iTween.EaseType.easeOutQuad
            )
        );
        Save();
    }

    void tweenOnUpdateCallBack(float newValue) {
        Global.honeyAmount = newValue;
    }

    void tweenOnCompleteCallback() {
        Debug.Log("fully filled");
        Global.SaveData();
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
