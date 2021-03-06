using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Random = UnityEngine.Random;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class HandScript : MonoBehaviour {
    public GameObject hive;
    public bool moveBack;
    public bool wasUsed;
    public float speed;
    public float stealAmount;
    public Vector3 initialPosition;
    static public HandSpawner handSpawner;
    public Animator animator;

    private float stolenHoney;
    private List<Hashtable> forwardMovementProperties, backwardMovementProperties;
    [SerializeField] private GameObject honey;
    private HoneyCounter honeyCounter;
    private float destructionTimer = 0.0f;
    private float lifetimeAfterTheft = 5.0f;
    private bool stealAnimationTriggered;

    private void Awake() {
        honeyCounter = GameObject.FindGameObjectWithTag("HoneyCounter").GetComponent<HoneyCounter>();
        if (handSpawner == null) {
            handSpawner = GameObject.FindGameObjectWithTag("HandSpawner").GetComponent<HandSpawner>();
        }

        initialPosition = transform.position;
        moveBack = false;
        wasUsed = false;
        stealAnimationTriggered = false;

        transform.rotation = rotationTowardsHive();

        honey.transform.localScale = Vector3.zero;

        // iTween setup
        {
            iTween.Init(gameObject);

            forwardMovementProperties = new List<Hashtable>();
            backwardMovementProperties = new List<Hashtable>();

            float time = (new Vector2(transform.position.x, transform.position.y).magnitude) / speed;

            // Movement properties: initial position -> hive
            forwardMovementProperties.Add(iTween.Hash(
                        "position", hive.transform.position,
                        "time", 1.0f * time,
                        "easeType", iTween.EaseType.easeInSine));
            forwardMovementProperties.Add(iTween.Hash(
                        "position", hive.transform.position,
                        "time", 1.2f * time,
                        "easeType", iTween.EaseType.easeInQuad));
            forwardMovementProperties.Add(iTween.Hash(
                        "position", hive.transform.position,
                        "time", 1.4f * time,
                        "easeType", iTween.EaseType.easeInCubic));
            // TODO: These bouncy comments below look cool, but need more work
            // forwardMovementProperties.Add(iTween.Hash(
            //         "position", hive.transform.position,
            //         "time", 1.8f * time,
            //         "easeType", iTween.EaseType.easeInBounce));
            // forwardMovementProperties.Add(iTween.Hash(
            //         "position", hive.transform.position,
            //         "time", 2.6f * time,
            //         "easeType", iTween.EaseType.easeInOutBounce));

            // Movement properties: hive -> initial position
            backwardMovementProperties.Add(iTween.Hash(
                    "position", initialPosition,
                    "time", 1.0f * time,
                    "easeType", iTween.EaseType.easeOutSine));
            backwardMovementProperties.Add(iTween.Hash(
                    "position", initialPosition,
                    "time", 1.2f * time,
                    "easeType", iTween.EaseType.easeOutQuad));
            backwardMovementProperties.Add(iTween.Hash(
                    "position", initialPosition,
                    "time", 1.4f * time,
                    "easeType", iTween.EaseType.easeOutCubic));
        }

        iTween.MoveTo(gameObject,
                forwardMovementProperties[Random.Range(0, forwardMovementProperties.Count)]);
    }

    private void Update() {
        destructionTimer += Convert.ToSingle(moveBack) * Time.deltaTime;
        gameObject.SetActive(destructionTimer < lifetimeAfterTheft);

        // Trigger a grabbing animation when a hand is close enough to the hive
        if (!stealAnimationTriggered &&
                Vector3.Distance(hive.transform.position, transform.position) <= 4.75f) {
            animator.SetTrigger("Steal");
            stealAnimationTriggered = true;

            iTween.Stop(honey);
            iTween.ScaleTo(honey, iTween.Hash(
                        "scale", Vector3.one,
                        "time", 0.8f,
                        "delay", 0.15f,
                        "easeType", iTween.EaseType.easeOutElastic));
        }
    }

    private Quaternion rotationTowardsHive() {
        Vector3 direction = (hive.transform.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        return Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Hive")) {
            honey.SetActive(true);
            steal();

            moveBack = true;
            iTween.Stop(gameObject);
            iTween.MoveTo(gameObject,
                    backwardMovementProperties[Random.Range(0, backwardMovementProperties.Count)]);
        }
    }

    public void steal() {
        stolenHoney = Mathf.Min(honeyCounter.HoneyAmount, stealAmount);
        honeyCounter.HoneyAmount -= Convert.ToSingle(!moveBack) * stolenHoney;
    }

    public void giveBack() {
        honeyCounter.HoneyAmount += Convert.ToSingle(moveBack) * stolenHoney;
        stolenHoney = 0f;
    }

    private void OnEnable() {
        stolenHoney = 0f;

        if (!handSpawner.activeHands.Contains(gameObject)) {
            handSpawner.activeHands.Add(gameObject);
        }
    }

    private void OnDisable() {
        handSpawner.activeHands.Remove(gameObject);
    }
}
