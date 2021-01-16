using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeScript : MonoBehaviour
{
    public HoneyCounter honeyCounter;
    private float smokeEffect = 0.003f;
    private ParticleSystem particleSystem;
    List<ParticleSystem.Particle> enter = new List<ParticleSystem.Particle>();
    private int particlesTriggered = 0;

    // Start is called before the first frame update
    void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        honeyCounter.SmokeFactor += 
            Convert.ToSingle(particlesTriggered == 0) * smokeEffect / 100.0f;
    }

    private void OnParticleCollision(GameObject other)
    {
        honeyCounter.SmokeFactor +=
            Convert.ToSingle(other.CompareTag("Blade")) * smokeEffect * 2.0f;
    }

    private void OnParticleTrigger()
    {
        particlesTriggered = particleSystem.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);
        honeyCounter.SmokeFactor -= particlesTriggered * smokeEffect;
    }
}
