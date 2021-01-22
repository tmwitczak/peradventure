using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeesScript : MonoBehaviour
{
    [SerializeField]
    private int amountOfBees = 30;
    private ParticleSystem bees;
    [SerializeField] ParticleSystem center;
    private HoneyCounter honeyCounter;
    private DataCollectorScript dataCollector;

    // Start is called before the first frame update
    void Start()
    {
        honeyCounter = FindObjectOfType<HoneyCounter>();
        dataCollector = FindObjectOfType<DataCollectorScript>();
        bees = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        var emissionRateOverTime = bees.emission;
        emissionRateOverTime.rateOverTime = amountOfBees;

        var velocity = bees.velocityOverLifetime;
        velocity.speedModifier = honeyCounter.SmokeFactor;

        var centerEmission = center.emission;
        centerEmission.rateOverTime = honeyCounter.SmokeFactor * 10.0f;

        honeyCounter.Speed = honeyCounter.SmokeFactor;
    }

    public int getAmountOfBees()
    {
        return amountOfBees;
    }

    public void setAmountOfBees(int val)
    {
        amountOfBees = val;
    }

    public void addAmountOfBees(int val)
    {
        amountOfBees += val;
    }
}
