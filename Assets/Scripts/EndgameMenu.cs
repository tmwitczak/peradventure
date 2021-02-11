using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EndgameMenu : MonoBehaviour {
    [SerializeField] private BackgroundVariantGenerator backgroundVariantGenerator;
    [SerializeField] private GameObject birdSpawner;
    [SerializeField] private GameObject blade;
    [SerializeField] private GameObject handSpawner;
    [SerializeField] private GameObject hiveScript;
    [SerializeField] private HoneyCounter honeyCounter;
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private PreRenderBackground preRenderBackground;
    [SerializeField] private Timekeeper timekeeper;

    public BeesScript beesScript;
    public GameObject EndLevelHelper;
    public Image beeLevelUp;

    private float fillSpeed = 1.5f;
    private float honeyOverflow = 0.0f;

    private Text endText;
    private string[] endTextsList = { "sublime!", "brilliant!", "great job!", "almost!" };

    private bool overflowed = false;
    public bool beeAmountUp = false;
    public static bool endStart = true;
    public static bool levelFailed = false;
    public static bool resultsActive = false;

    public GameObject continueButton;
    public GameObject restartButton;

    [SerializeField] GameObject overlay;

    public void Reset() {
        beeAmountUp = endStart = levelFailed = resultsActive = false;
    }

    private void Awake() {
        iTween.Init(gameObject);
    }

    private void OnEnable() {
        beeLevelUp.canvasRenderer.SetAlpha(0.0f);
        endText = gameObject.transform.Find("Text").GetComponent<Text>();

        // Test for lighting change and smooth background transitions
        preRenderBackground.setBackgroundTextures(Global.currentGameplayLevel);
        preRenderBackground.runTransition(Global.currentGameplayLevel, Global.currentGameplayLevel + 1);
        preRenderBackground.refresh();

        overlay.SetActive(true);
        overlay.GetComponent<Animator>().SetTrigger("FadeIn");
        GetComponent<Animator>().SetTrigger("FadeIn");

        timekeeper.slowdownTimescale();

        Input.backButtonLeavesApp = true;
    }

    public void Resume() {
        GetComponent<Animator>().SetTrigger("FadeOut");
        overlay.GetComponent<Animator>().SetTrigger("FadeOut");

        blade.SetActive(true);
        hiveScript.SetActive(true);
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
        levelManager.loadLevel(Global.currentGameplayLevel);

        timekeeper.speedupTimescale();

        Input.backButtonLeavesApp = false;
    }

    public void Disable() {
        gameObject.SetActive(false);
    }

    private void Update() {
        if (beeAmountUp) {
            StartCoroutine(showBeeAmountUp());
        }

        if (resultsActive) {
            if (endStart) {
                StartCoroutine(waitForMenu(0.0f));
            }
            if (Global.honeyAmount >= Global.levelMaxValue) {
                honeyOverflow = Global.honeyAmount - Global.levelMaxValue;
                LevelUp();
                beeAmountUp = true;
                Global.SaveData();
            }
        }
    }

    private IEnumerator waitForMenu(float seconds) {
        if (honeyCounter.endHoneyAmount >= 110.0f) {
            endText.text = endTextsList[0];
        } else if (110.0f > honeyCounter.endHoneyAmount && honeyCounter.endHoneyAmount >= 80.0f) {
            endText.text = endTextsList[1];
        } else if (80.0f > honeyCounter.endHoneyAmount && honeyCounter.endHoneyAmount >= 40.0f) {
            endText.text = endTextsList[2];
        } else {
            endText.text = endTextsList[3];
        }
        endStart = false;

        handSpawner.SetActive(false);
        birdSpawner.SetActive(false);
        hiveScript.SetActive(false);
        blade.SetActive(false);

        yield return new WaitForSecondsRealtime(seconds);

        iTween.Stop(gameObject);
        iTween.ValueTo(gameObject, iTween.Hash(
            "from", Global.honeyAmount,
            "to", Global.honeyAmount + honeyCounter.endHoneyAmount,
            "time", 3.0f,
            "ignoretimescale", true,
            // "onupdatetarget", gameObject,
            "onupdate", "tweenOnUpdateCallBack",
            // "oncompletetarget", gameObject,
            "oncomplete", "tweenOnCompleteCallback",
            "easetype", iTween.EaseType.easeOutQuad
            )
        );
        Save();
    }

    private void tweenOnUpdateCallBack(float newValue) {
        Global.honeyAmount = newValue;
    }

    private void tweenOnCompleteCallback() {
        Debug.Log("fully filled");
        Global.SaveData();
    }

    private IEnumerator showBeeAmountUp() {
        beeLevelUp.CrossFadeAlpha(1.0f, 0.1f, false);
        yield return new WaitForSecondsRealtime(1.0f);
        beeLevelUp.CrossFadeAlpha(0.0f, 0.1f, false);
        beeAmountUp = false;
    }

    private void LevelUp() {
        Global.hiveLevel++;
        Global.amountOfBees += 20;
        Global.levelMaxValue *= 2.0f;
    }

    private void Save() {
        Global.currentGameplayLevel += 1;
        Global.SaveData();
    }
}
