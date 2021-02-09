using UnityEngine;

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
    [SerializeField] private Timekeeper timekeeper;

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
        overlay.GetComponent<Overlay>().fadeIn = true;
        if (!firstStart) {
            GetComponent<Animator>().SetTrigger("FadeIn");
            overlay.GetComponent<Animator>().SetTrigger("FadeIn");
        }

        timekeeper.slowdownTimescale();

        blade.SetActive(false);
    }


    public void Resume() {
        if (!isPaused) {
            return;
        }

        isPaused = false;

        overlay.GetComponent<Overlay>().fadeIn = false;

        GetComponent<Animator>().SetTrigger("FadeOut");
        overlay.GetComponent<Animator>().SetTrigger("FadeOut");

        timekeeper.speedupTimescale();

        blade.SetActive(true);
    }

    public void Disable() {
        // gameObject.SetActive(false);
    }
}
