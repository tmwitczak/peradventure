using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class SmokeBehaviour : MonoBehaviour {
    public List<ParticleSystem> smokeParticleSystems;
    public float smokeCooldown = 2.0f;
    public HoneyCounter honeyCounter;

    private List<SmokeScript> smokeGenerators;
    private List<float> elapsedTime;
    private float smokeTimer;

    private float EMITTING_TIME = 6.0f;

    private List<int> randomSmokeActivations;
    private IEnumerator<int> randomSmokeActivation;

    private void Awake() {
        smokeGenerators = new List<SmokeScript>();
        for (int i = 0; i < smokeParticleSystems.Count; ++i) {
            smokeGenerators.Add(
                    smokeParticleSystems[i].gameObject.GetComponent<SmokeScript>());
        }
    }

    private void Start() {
        elapsedTime = new List<float>(smokeParticleSystems.Count);
        for (int i = 0; i < smokeParticleSystems.Count; ++i) {
            elapsedTime.Add(0.0f);
        }
    }

    private void Update() {
        updateSmokeCoverageFactor();

        smokeTimer += Time.deltaTime;
        if (smokeTimer >= smokeCooldown) {
            randomSmokeActivation.MoveNext();
            activateSmoke(randomSmokeActivation.Current);
            smokeTimer = 0.0f;
        }

        for (int i = 0; i < smokeParticleSystems.Count; ++i) {
            if (smokeParticleSystems[i].isStopped) {
                continue;
            }

            elapsedTime[i] += Time.deltaTime;
            if (elapsedTime[i] > EMITTING_TIME) {
                smokeParticleSystems[i].Stop();
            }
        }
    }

    private int particlesTriggered {
        get {
            int count = 0;
            foreach (var generator in smokeGenerators) {
                count += generator.particlesTriggered;
            }
            return count;
        }
    }

    private float coverage {
        get {
            float sigma = 80f; // Controls the steepness of the curve
            float a = particlesTriggered / sigma;
            return Mathf.Exp(-0.5f * a * a);
        }
    }

    private void updateSmokeCoverageFactor() {
        float coveredDistancePerSecond = 0.9f;
        honeyCounter.SmokeFactor = Global.lerp(
                honeyCounter.SmokeFactor, coverage, coveredDistancePerSecond, Time.deltaTime);
    }

    private void activateSmoke(int i) {
        Assert.IsTrue(i >= 0 && i < smokeParticleSystems.Count);

        smokeParticleSystems[i].Play();
        elapsedTime[i] = 0.0f;
    }

    public void clearSmoke() {
        foreach (var system in smokeParticleSystems) {
            system.Stop();
            system.Clear();
        }
    }

    public void reset() {
        smokeTimer = 0f;
        precalculateRandomness();
        clearSmoke();
    }

    private int totalSmokeSpawnCount {
        get => Mathf.FloorToInt(StopwatchScript.MaxTime / smokeCooldown);
    }

    public void precalculateRandomness() {
        randomSmokeActivations = new List<int>();
        for (int i = 0; i < totalSmokeSpawnCount; ++i) {
            randomSmokeActivations.Add((int)Random.Range(0, 4));
        }

        randomSmokeActivation = randomSmokeActivations.GetEnumerator();
    }
}
