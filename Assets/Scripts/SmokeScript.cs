using System;
using System.Collections.Generic;
using UnityEngine;

public class SmokeScript : MonoBehaviour {
    List<ParticleSystem.Particle> enter = new List<ParticleSystem.Particle>();
    private new ParticleSystem particleSystem;
    private float smokeEffect = 0.003f;
    private int particlesTriggered = 0;
    public HoneyCounter honeyCounter;

    void Start() {
        particleSystem = GetComponent<ParticleSystem>();
    }

    private void Update() {
        honeyCounter.SmokeFactor +=
            Convert.ToSingle(particlesTriggered == 0) * smokeEffect / 100.0f;
    }

    private void OnParticleCollision(GameObject other) {
        honeyCounter.SmokeFactor +=
            Convert.ToSingle(other.CompareTag("Blade")) * smokeEffect * 2.0f;
    }

    private void OnParticleTrigger() {
        particlesTriggered = particleSystem.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);
        honeyCounter.SmokeFactor -= particlesTriggered * smokeEffect;
    }
}
