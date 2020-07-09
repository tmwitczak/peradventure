using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeesScript : MonoBehaviour
{
    public int amountOfBees = 10;
    
    private ParticleSystem bees;
    // Start is called before the first frame update
    void Start()
    {
        bees = GetComponent<ParticleSystem>();
        bees.maxParticles = amountOfBees;
        var emissionRateOverTime = bees.emission.rateOverTime;
        emissionRateOverTime.constantMax = amountOfBees;
    }
}
