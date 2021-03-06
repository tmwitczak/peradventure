using UnityEngine;

public class BeesScript : MonoBehaviour {
    private ParticleSystem bees;
    [SerializeField] ParticleSystem center;
    private HoneyCounter honeyCounter;

    private void Awake() {
        bees = GetComponent<ParticleSystem>();
    }

    private void Start() {
        honeyCounter = FindObjectOfType<HoneyCounter>();
    }

    private void Update() {
        var emissionRateOverTime = bees.emission;
        emissionRateOverTime.rateOverTime = Global.amountOfBees;

        var velocity = bees.velocityOverLifetime;
        velocity.speedModifier = honeyCounter.SmokeFactor;

        var centerEmission = center.emission;
        centerEmission.rateOverTime = honeyCounter.SmokeFactor * 10.0f;
    }
}
