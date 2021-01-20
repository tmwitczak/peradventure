using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuButtonController : MonoBehaviour
{
    private enum ButtonType
    {
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

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (buttonPressed)
        {
            switch (type)
            {
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

                case ButtonType.Continue:
                    LoadLevel(SceneManager.GetActiveScene().name);
                    break;

                //case ButtonType.Level:
                //    string buttonName = gameObject.name;
                //    buttonName = buttonName.Replace(" ", String.Empty);
                //    LoadLevel(buttonName);
                //    StopwatchScript.MaxTime = 30.0f;
                //    Time.timeScale = 1.0f;
                //    break;
                
                case ButtonType.Restart:
                    LoadLevel(SceneManager.GetActiveScene().name);
                    break;
            }
        }
    }

    public void PressButton()
    {
        animator.SetBool("press", true);
        buttonPressed = true;
        animEnded = false;
    }

    public void AnimationEnd()
    {
        animator.SetBool("press", false);
        animEnded = true;
        buttonPressed = false;
    }

    public void ActivateLevel()
    {
        var button = gameObject.GetComponentInChildren<Button>();
        var lockImage = button.transform.Find("Image").gameObject;
        var buttonText = button.GetComponentInChildren<Text>();
        var buttonName = gameObject.name;
        var buttonBgColor = button.GetComponent<Image>();

        button.interactable = true;
        lockImage.SetActive(false);
        for (int i = 0; i < buttonName.Length; i++)
        {
            if (Char.IsDigit(buttonName[i]))
            {
                buttonText.text += buttonName[i];
            }
        }
        buttonBgColor.color = Color.white;
    }

    //private void LoadLevel(int sceneNum)
    //{
    //    SceneManager.LoadScene(sceneNum);
    //}

    private void LoadLevel(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    //private bool LoadNextScene()
    //{
    //    if(Application.CanStreamedLevelBeLoaded(SceneManager.GetActiveScene().buildIndex + 1))
    //    {
    //        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    //    } else
    //    {
    //        return false;
    //    }
    //    return true;
    //}

    //private void LoadMainMenu()
    //{
    //    SceneManager.LoadScene("MainMenu");
    //}

    private void SetToFalse()
    {
        animEnded = false;
        buttonPressed = false;
    }
}
