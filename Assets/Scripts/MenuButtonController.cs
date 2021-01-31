using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuButtonController : MonoBehaviour {
    private enum ButtonType {
        None,
        Start,
        Options,
        Quit,
        Back,
        Resume,
        QuitToMenu,
        Continue,
        Level,
        Restart
    }

    [SerializeField] GameObject MainMenu;
    [SerializeField] GameObject LevelMenu;
    [SerializeField] GameObject OptionsMenu;
    [SerializeField] ButtonType type;
    [SerializeField] Animator animator;
    public AudioSource audioSource;
    private bool buttonPressed = false;
    private bool animEnded = false;
    private LevelManager levelManager;

    private void Start() {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        levelManager = FindObjectOfType<LevelManager>();
    }

    private void handleButtonPress() {
        switch (type) {
            //case ButtonType.Start:
            //    MainMenu.SetActive(false);
            //    LevelMenu.SetActive(true);
            //    AnimationEnd();
            //    break;

            //case ButtonType.Options:
            //    if (!OptionsMenu.activeSelf)
            //    {
            //        MainMenu.SetActive(false);
            //        OptionsMenu.SetActive(true);
            //        SetToFalse();
            //    }
            //    break;

            //case ButtonType.Quit:
            //    AnimationEnd();
            //    Application.Quit();
            //    break;

            //case ButtonType.Back:
            //    if (!MainMenu.activeSelf)
            //    {
            //        MainMenu.SetActive(true);
            //        if(OptionsMenu.activeSelf)
            //        {
            //            OptionsMenu.SetActive(false);
            //        }
            //        if(LevelMenu.activeSelf)
            //        {
            //            LevelMenu.SetActive(false);
            //        }
            //        AnimationEnd();
            //    }
            //    SetToFalse();
            //    break;

            //case ButtonType.QuitToMenu:
            //    LoadMainMenu();
            //    AnimationEnd();
            //    break;

            //case ButtonType.Level:
            //    string buttonName = gameObject.name;
            //    buttonName = buttonName.Replace(" ", String.Empty);
            //    LoadLevel(buttonName);
            //    StopwatchScript.MaxTime = 30.0f;
            //    Time.timeScale = 1.0f;
            //    break;

            case ButtonType.Continue:
            case ButtonType.Restart:
                levelManager.loadLevel(Global.currentGameplayLevel);
                break;
        }
    }

    public void PressButton() {
        animator.SetBool("press", true);
        buttonPressed = true;
        animEnded = false;

        handleButtonPress();
    }

    private void SetToFalse() {
        animEnded = false;
        buttonPressed = false;
    }
}
