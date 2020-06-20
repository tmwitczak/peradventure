using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeBehaviour : MonoBehaviour
{
    [SerializeField] ParticleSystem SmokeLeft;
    [SerializeField] ParticleSystem SmokeRight;
    [SerializeField] ParticleSystem SmokeUp;
    [SerializeField] ParticleSystem SmokeDown;

    private float smokeTimer = 0.0f;
    private float smokeCooldown = 2.0f;
    private bool[] startPlaying;
    private float[] startTime;

    private float EMITTING_TIME = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        startPlaying = new bool[4];
        startTime = new float[4];

        for (int i = 0; i < 4; i++)
        {
            startTime[i] = 0.0f;
            startPlaying[i] = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        smokeTimer += Time.deltaTime;
        if (smokeTimer >= smokeCooldown)
        {
            ActivateSmoke((int)Random.Range(0, 4));
            smokeTimer = 0.0f;
            smokeCooldown = Random.Range(1, 10);
        }

        if (startPlaying[0])
        {
            if(SmokeLeft.isStopped)
            {
                SmokeLeft.Play();
            }
            float timePlaying = (Time.time - startTime[0]);
            if (timePlaying > EMITTING_TIME && SmokeLeft.isPlaying)
            {
                SmokeLeft.Stop();
                startPlaying[0] = false;
                startTime[0] = 0.0f;
            }
        }

        if (startPlaying[1])
        {
            if (SmokeRight.isStopped)
            {
                SmokeRight.Play();
            }
            float timePlaying = (Time.time - startTime[1]);
            if (timePlaying > EMITTING_TIME && SmokeRight.isPlaying)
            {
                SmokeRight.Stop();
                startPlaying[1] = false;
                startTime[1] = 0.0f;
            }
        }

        if (startPlaying[2])
        {
            if (SmokeUp.isStopped)
            {
                SmokeUp.Play();
            }
            float timePlaying = (Time.time - startTime[2]);
            if (timePlaying > EMITTING_TIME && SmokeUp.isPlaying)
            {
                SmokeUp.Stop();
                startPlaying[2] = false;
                startTime[2] = 0.0f;
            }
        }

        if (startPlaying[3])
        {
            if (SmokeDown.isStopped)
            {
                SmokeDown.Play();
            }
            float timePlaying = (Time.time - startTime[3]);
            if (timePlaying > EMITTING_TIME && SmokeDown.isPlaying)
            {
                SmokeDown.Stop();
                startPlaying[3] = false;
                startTime[3] = 0.0f;
            }
        }
    }

    void ActivateSmoke(int i)
    {
        Debug.Log(i);
        switch (i)
        {
            case 0:
                startTime[i] = Time.time;
                startPlaying[i] = true;
                break;
            case 1:
                startTime[i] = Time.time;
                startPlaying[i] = true;
                break;
            case 2:
                startTime[i] = Time.time;
                startPlaying[i] = true;
                break;
            case 3:
                startTime[i] = Time.time;
                startPlaying[i] = true;
                break;
        }
    }
}
