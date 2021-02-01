using UnityEngine;
using UnityEngine.UI;

public class PauseMenuController : MonoBehaviour {
    bool firstStart = true;
    public static bool isPaused = false;
    private float timerLimit = 1f - 0.5f / 30f;

    [SerializeField] private BeesScript beesScript;
    [SerializeField] private BirdSpawnerScript birdSpawner;
    [SerializeField] private GameObject blade;
    [SerializeField] private GameObject overlay;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private HandSpawner handSpawner;

    private float originalTimescale;

    private void Awake() {
        iTween.Init(gameObject);
        originalTimescale = Time.timeScale;
    }

    private void Start() {
        // firstStart = false;

        Pause(true);

        // iTween.Stop(gameObject);
        // Time.timeScale = 0.0f;

        // Animator animator = GetComponent<Animator>();
        // animator.GetCurrentAnimatorClipInfo(0)[0].clip.SampleAnimation(
        //         animator.gameObject, animator.GetCurrentAnimatorClipInfo(0)[0].clip.length);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (isPaused) {
                Resume();
            } else {
                Pause();
            }
        }
    }

    public void Pause(bool firstStart = false) {
        if (isPaused || StopwatchScript.timer > timerLimit) {
            return;
        }

        isPaused = true;

        // gameObject.SetActive(true);
        overlay.SetActive(true);
        if (!firstStart) {
            GetComponent<Animator>().SetTrigger("FadeIn");
            overlay.GetComponent<Animator>().SetTrigger("FadeIn");
        }

        changeTimescale(originalTimescale, 0.0f);

        blade.SetActive(false);
    }


    public void Resume() {
        if (!isPaused) {
            return;
        }

        isPaused = false;

        GetComponent<Animator>().SetTrigger("FadeOut");
        overlay.GetComponent<Animator>().SetTrigger("FadeOut");

        changeTimescale(0.0f, originalTimescale);

        blade.SetActive(true);
    }

    public void Disable() {
        // gameObject.SetActive(false);
    }

    private void changeTimescale(float from, float to) {
        iTween.Stop(gameObject);
        iTween.ValueTo(gameObject, iTween.Hash(
            "from", from,
            "to", to,
            "time", 0.5f,
            "ignoretimescale", true,
            // "onupdatetarget", gameObject,
            "onupdate", "tweenOnUpdateTimescaleCallback",
            "easetype", iTween.EaseType.easeOutQuad
            )
        );
    }

    private void tweenOnUpdateTimescaleCallback(float value) {
        Time.timeScale = value;
    }
}
