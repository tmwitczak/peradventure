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
    // Start is called before the first frame update
    void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Blade"))
        {
            honeyCounter.setSmokeFactor(honeyCounter.getSmokeFactor() + smokeEffect);
        }
    }

    private void OnParticleTrigger()
    {
        int numEnter = particleSystem.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);
        for (int i = 0; i < numEnter; i++)
        {
            honeyCounter.setSmokeFactor(honeyCounter.getSmokeFactor() - smokeEffect);
        }
    }
}
