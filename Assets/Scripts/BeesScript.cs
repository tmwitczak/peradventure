using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeesScript : MonoBehaviour
{
    public int amountOfBees = 10;
    private ParticleSystem bees;
    [SerializeField] ParticleSystem center;
    private HoneyCounter honeyCounter;
    // Start is called before the first frame update
    void Start()
    {
        honeyCounter = FindObjectOfType<HoneyCounter>();
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
    }
}
