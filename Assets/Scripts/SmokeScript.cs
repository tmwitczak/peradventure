using System;
using System.Collections.Generic;
using UnityEngine;

public class SmokeScript : MonoBehaviour {
    List<ParticleSystem.Particle> inside = new List<ParticleSystem.Particle>();
    private new ParticleSystem particleSystem;
    public int particlesTriggered = 0;

    private void Awake() {
        particleSystem = GetComponent<ParticleSystem>();
    }

    private void OnParticleTrigger() {
        particlesTriggered = particleSystem.GetTriggerParticles(
                ParticleSystemTriggerEventType.Inside, inside);
    }
}
