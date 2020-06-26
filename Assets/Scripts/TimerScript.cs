using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour
{
    public static float TimeLeft = 30.0f;

    private Text text;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (TimeLeft > 0.0f)
        {
            TimeLeft -= Time.deltaTime;
            text.text = "Time left: " + Mathf.Round(TimeLeft);
        }
        else TimeLeft = 0.0f;
    }
}
