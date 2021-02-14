using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using System;
using Random = UnityEngine.Random;

public class HandSpawner : MonoBehaviour {
    public GameObject Hand;
    public float HandSpeed;
    public float SpawnCooldown;
    public int HandsToSpawn;
    [SerializeField] private int handsToPrespawn = 100;
    public float innerPadding;
    public float outerPadding;
    [HideInInspector] public List<GameObject> activeHands = new List<GameObject>();
    [HideInInspector] public List<GameObject> hands = new List<GameObject>();

    private GameObject handParent;
    private Vector3 screenMin;
    private Vector3 screenMax;
    private List<float> spawnX;
    private List<float> spawnY;
    private int _currentHand = 0;
    private float spawnTimer;
    private float _spawnAngle = 15f;

    private int currentHand {
        get => _currentHand;
        set => _currentHand = Global.mod(value, handsToPrespawn);
    }
    private float spawnAngle {
        get => _spawnAngle;
        set => _spawnAngle = Global.fmod(value, Mathf.PI * Mathf.Rad2Deg);
    }

    private void Awake() {
        screenMin = Camera.main.ScreenToWorldPoint(new Vector2(0, 0)) + new Vector3(-2.0f, -2.0f);
        screenMax = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height))
            + new Vector3(2.0f, 2.0f);

        spawnX = new List<float>();
        for (float i = screenMin.x - outerPadding; i < screenMax.x + outerPadding; i += 0.01f) {
            spawnX.Add(i);
        }

        spawnY = new List<float>();
        for (float i = screenMin.y - outerPadding; i < screenMax.y + outerPadding; i += 0.01f) {
            spawnY.Add(i);
        }
    }

    private void Update() {
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= SpawnCooldown) {
            spawnTimer = 0.0f;

            for (int i = 0; i < HandsToSpawn; ++i) {
                SpawnHand();
            }
        }
    }

    private void SpawnHand() {
        Assert.IsTrue(currentHand >= 0 && currentHand < hands.Count);

        // Skip spawning of this hand if there isn't enough of the prespawned hands
        Assert.IsFalse(unusedHandsCount == 0,
                "Hand couldn't be spawned! Increase the prespawn limit!");
        if (unusedHandsCount == 0) {
            return;
        }

        // Search for a fitting hand (angular condition)
        int unfitHands = 0;
        while (unfitHands < unusedHandsCount && moveToNextUnusedHand())
        {
            var nextHand = hands[currentHand].GetComponent<HandScript>().initialPosition;
            int handsChecked = 0;
            foreach (var hand in activeHands)
            {
                var activeHandAngle = calculateAngle(hand.transform.position, nextHand);
                if (activeHandAngle >= spawnAngle)
                {
                    handsChecked++;
                } else
                {
                    break;
                }
            }

            if (handsChecked == activeHands.Count())
            {
                break;
            }
            else {
                ++unfitHands;
            }
        }

        // Skip spawning of this hand if the active ones are too densely packed
        Assert.IsTrue(unfitHands < unusedHandsCount,
                "Hand couldn't be spawned due to the close proximity of the active hands!");
        if (!(unfitHands < unusedHandsCount)) {
            return;
        }

        // Activate the chosen prespawned hand
        hands[currentHand].SetActive(true);
        hands[currentHand].GetComponent<HandScript>().wasUsed = true;
    }

    private bool moveToNextUnusedHand()
    {
        for(int startSearch = currentHand++; currentHand != startSearch; currentHand++)
        {
            if (!hands[currentHand].GetComponent<HandScript>().wasUsed)
            {
                return true;
            }
        }
        return false;
    }

    private int unusedHandsCount {
        get {
            int unusedHands = 0;
            foreach (var hand in hands) {
                if (!hand.GetComponent<HandScript>().wasUsed) {
                    ++unusedHands;
                }
            }
            return unusedHands;
        }
    }

    private float calculateAngle(Vector3 hand, Vector3 nextHand)
        => Vector3.Angle(hand, nextHand);

    public void prespawnHands() {
        destroyAllHands();
        handParent = new GameObject("Hands");

        for (int i = 0; i < handsToPrespawn; ++i) {
            int mode = Random.Range(0, 3);

            List<float> spawnX = new List<float>(this.spawnX);
            List<float> spawnY = new List<float>(this.spawnY);

            if (mode == 1) // Left and right edges
            {
                spawnX = spawnX.Where(value =>
                    value <= screenMin.x - innerPadding ||
                    value >= screenMax.x + innerPadding).ToList();
            } else if (mode == 2) // Top and bottom edges
              {
                spawnY = spawnY.Where(value =>
                    value <= screenMin.y - innerPadding ||
                    value >= screenMax.y + innerPadding).ToList();
            } else // Corners
              {
                spawnX = spawnX.Where(value =>
                        value <= screenMin.x - innerPadding ||
                        value >= screenMax.x + innerPadding).ToList();
                spawnY = spawnY.Where(value =>
                        value <= screenMin.y - innerPadding ||
                        value >= screenMax.y + innerPadding).ToList();
            }

            var spawnPosition = new Vector2(
                    spawnX[Random.Range(0, spawnX.Count)],
                    spawnY[Random.Range(0, spawnY.Count)]);

            var hand = Instantiate(Hand, spawnPosition, Quaternion.identity,
                    handParent.GetComponent<Transform>());
            hand.SetActive(false);
            hand.GetComponent<HandScript>().speed = HandSpeed;

            hands.Add(hand);
        }
    }

    public void destroyAllHands() {
        foreach (var hand in hands) {
            Destroy(hand);
        }
        currentHand = 0;
        hands.Clear();
        activeHands.Clear();
        Destroy(handParent);
    }

    public void reset() {
        spawnTimer = 0f;
        destroyAllHands();
        prespawnHands();
    }
}
