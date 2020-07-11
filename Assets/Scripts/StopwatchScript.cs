using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StopwatchScript : MonoBehaviour
{
    public static float MaxTime = 30.0f;
    public Transform clockHandTransform;
    [SerializeField] GameObject EndGameMenu;
    [SerializeField] GameObject Fill;
    
    private float timer = 0.0f;
    private float timeDuration;
    private HoneyCounter honeyCounter;

    // Start is called before the first frame update
    void Start()
    {
        timeDuration = MaxTime;
        honeyCounter = FindObjectOfType<HoneyCounter>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timer == 1f)
        {
            honeyCounter.endHoneyAmount = honeyCounter.getHoneyAmount();
            EndGameMenu.SetActive(true);
            HiveLevel.resultsActive = true;
            Fill.GetComponent<Image>().fillAmount = 1.0f;
            Destroy(GameObject.Find("HandSpawner"));
            Destroy(GameObject.Find("HoneyCounter"));
        }
        if (timer < 1f)
        {
            timer += Time.deltaTime / timeDuration;
            float timeNormalized = timer % 1f;
            clockHandTransform.eulerAngles = new Vector3(0, 0, -timeNormalized * 360f);
            Fill.GetComponent<Image>().fillAmount = timeNormalized;
        }
        else timer = 1f;
    }
}
