using System;
using System.Collections.Generic;
using UnityEngine;

public class SmokeScript : MonoBehaviour {
    List<ParticleSystem.Particle> enter = new List<ParticleSystem.Particle>();
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
        particlesTriggered = particleSystem.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);
        honeyCounter.SmokeFactor -= particlesTriggered * smokeEffect;
    }

    private float particleMultiplier()
    {
        float particleMultiplier = 0.0f;
        switch (particlesTriggered)
        {
            case 0:
                particleMultiplier = 1.0f;
                break;
            case 1:
                particleMultiplier = 0.2f;
                break;
            case 2:
                particleMultiplier = 0.1f;
                break;
            default:
                particleMultiplier = 0.0f;
                break;
        }
        return particleMultiplier;
    }
}
