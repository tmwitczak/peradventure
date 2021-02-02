using System.Collections.Generic;
using UnityEngine.Assertions;
using UnityEngine;

public class SmokeBehaviour : MonoBehaviour {
    public List<ParticleSystem> smokeParticleSystems;
    public float smokeCooldown = 2.0f;

    private List<float> elapsedTime;
    private float smokeTimer;

    private float EMITTING_TIME = 6.0f;

    private void Start() {
        elapsedTime = new List<float>(smokeParticleSystems.Count);
        for (int i = 0; i < smokeParticleSystems.Count; ++i) {
            elapsedTime.Add(0.0f);
        }
    }

    private void Update() {
        smokeTimer += Time.deltaTime;
        if (smokeTimer >= smokeCooldown) {
            activateSmoke((int)Random.Range(0, 4));
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
        clearSmoke();
    }
}
