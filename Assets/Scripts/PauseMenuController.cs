using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuController : MonoBehaviour
{
    bool firstStart = true;
    public static bool isPaused = false;
    [SerializeField] GameObject PauseMenu;
    [SerializeField] GameObject Blade;
    [SerializeField] GameObject Overlay;
    [SerializeField] Animator BackgroundOverlay;
    [SerializeField] IEnumerable<GameObject> TrailClones;
    public Animator animator;
    public BeesScript beesScript;

    private HandSpawner handSpawner;
    private BirdSpawnerScript birdSpawner;

    void Start()
    {
        Blade = GameObject.FindGameObjectWithTag("Blade");
        handSpawner = FindObjectOfType<HandSpawner>();
        birdSpawner = FindObjectOfType<BirdSpawnerScript>();

        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            // firstStart = false;
            // animator.GetCurrentAnimatorClipInfo(0)[0].clip.SampleAnimation(
            //         animator.gameObject, animator.GetCurrentAnimatorClipInfo(0)[0].clip.length);
            Pause();
            // Time.timeScale = 0.0f;
        }
        iTween.Init(gameObject);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        animator.SetTrigger("FadeOut");
        BackgroundOverlay.SetTrigger("FadeOut");
        // Time.timeScale = 1.0f;
        isPaused = false;
        Blade.SetActive(true);
        handSpawner.isSpawning = true;
        birdSpawner.isSpawning = true;
        iTween.Stop(gameObject);
        iTween.ValueTo(gameObject, iTween.Hash(
            "from", 0.0f,
            "to", 1.0f,
            "time", 0.5f,
            "ignoretimescale", true,
            "onupdatetarget", gameObject,
            "onupdate", "tweenOnUpdateCallBack",
            "easetype", iTween.EaseType.easeOutQuad
            )
        );
    }

    public void Disable()
    {
        PauseMenu.SetActive(false);
    }

    public void Pause()
    {
        Overlay.SetActive(true);
        TrailClones = Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.name == "Trail(Clone)");
        PauseMenu.SetActive(true);
        // Time.timeScale = 0.0f;
        iTween.Stop(gameObject);
        iTween.ValueTo(gameObject, iTween.Hash(
            "from", Time.timeScale,
            "to", 0.0f,
            "time", 0.5f,
            "ignoretimescale", true,
            "onupdatetarget", gameObject,
            "onupdate", "tweenOnUpdateCallBack",
            "easetype", iTween.EaseType.easeOutQuad
            )
        );
        isPaused = true;
        Blade.SetActive(false);
        handSpawner.isSpawning = false;
        birdSpawner.isSpawning = false;

        foreach (var obj in TrailClones)
        {
            Destroy(obj);
        }

        GameObject.Find("BeesCount").GetComponent<Text>().text = Global.amountOfBees.ToString();
    }

    void tweenOnUpdateCallBack(float newValue)
    {
        Time.timeScale = newValue;
    }
}
