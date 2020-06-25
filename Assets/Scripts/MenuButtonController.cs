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
        QuitToMenu
    }

    [SerializeField] GameObject MainMenu;
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
                case ButtonType.Start:
                    if (animEnded)
                    {
                        LoadLevel("Level1");
                        TimerScript.TimeLeft = 30.0f;
                        Time.timeScale = 1.0f;
                        SetToFalse();
                    }
                    break;

                case ButtonType.Options:
                    if (animEnded && !OptionsMenu.activeInHierarchy)
                    {
                        MainMenu.SetActive(false);
                        OptionsMenu.SetActive(true);
                        SetToFalse();
                    }
                    break;

                case ButtonType.Quit:
                    if (animEnded)
                    {
                        SetToFalse();
                        Application.Quit();
                    }
                    break;

                case ButtonType.Back:
                    if (animEnded && OptionsMenu.activeInHierarchy)
                    {
                        MainMenu.SetActive(true);
                        OptionsMenu.SetActive(false);
                        SetToFalse();
                    }
                    break;
                case ButtonType.QuitToMenu:
                    if (animEnded)
                    {
                        LoadMainMenu();
                        SetToFalse();
                    }
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
    }

    private void LoadLevel(int sceneNum)
    {
        SceneManager.LoadScene(sceneNum);
    }

    private void LoadLevel(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    private void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void SetToFalse()
    {
        animEnded = false;
        buttonPressed = false;
    }
}
