using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    public static bool isPaused = false;
    [SerializeField] GameObject PauseMenu;
    [SerializeField] GameObject Blade;
    [SerializeField] IEnumerable<GameObject> TrailClones;

    void Start()
    {
        Blade = GameObject.FindGameObjectWithTag("Blade");
    }
    // Update is called once per frame
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
        PauseMenu.SetActive(false);
        Time.timeScale = 1.0f;
        isPaused = false;
        Blade.SetActive(true);
    }

    public void Pause()
    {
        TrailClones = Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.name == "Trail(Clone)");
        PauseMenu.SetActive(true);
        Time.timeScale = 0.0f;
        isPaused = true;
        Blade.SetActive(false);
        foreach(var obj in TrailClones)
        {
            Destroy(obj);
        }
    }
}
