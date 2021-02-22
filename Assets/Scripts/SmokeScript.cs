using System;
using System.Collections.Generic;
using UnityEngine;

public class SmokeScript : MonoBehaviour {
    List<ParticleSystem.Particle> inside = new List<ParticleSystem.Particle>();
    private new ParticleSystem particleSystem;
    private float smokeEffect = 0.005f;
    private float recoverySpeed = 10f;
    private float bladeRecovery = 1.5f;
    private int particlesTriggered = 0;
    public HoneyCounter honeyCounter;

    private void Awake() {
        particleSystem = GetComponent<ParticleSystem>();
    }

    private void Update() {
        honeyCounter.SmokeFactor += (smokeEffect / recoverySpeed) * particleMultiplier();
    }

    private void OnParticleCollision(GameObject other) {
        honeyCounter.SmokeFactor +=
            Convert.ToSingle(other.CompareTag("Blade")) * smokeEffect * bladeRecovery;
    }

    private void OnParticleTrigger() {
        particlesTriggered = particleSystem.GetTriggerParticles(ParticleSystemTriggerEventType.Inside, inside);
        honeyCounter.SmokeFactor -= smokeEffect * (particlesTriggered / 100f);
    }

    private float particleMultiplier()
    {
        if(particlesTriggered > 0)
        {
            return 1 / Mathf.Pow(particlesTriggered, 2);
        }
        return 1.0f;
    }
}
