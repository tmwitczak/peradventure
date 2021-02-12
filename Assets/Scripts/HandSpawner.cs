using System.Collections.Generic;
using System.Linq;
using UnityEngine;
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
    private Vector3 lastHandSpawnedInitialPosition;
    private Vector3 screenMin;
    private Vector3 screenMax;
    private List<float> spawnX;
    private List<float> spawnY;
    private int currentHand = 0;
    private float spawnTimer;
    private double spawnAngle = 15.0;
    private bool isSpawning;

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

        lastHandSpawnedInitialPosition = new Vector3(0.0f, 0.0f);
        isSpawning = true;
    }

    private void Update() {
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= SpawnCooldown && isSpawning) {
            spawnTimer = 0.0f;

            for (int i = 0; i < HandsToSpawn; ++i) {
                SpawnHand();
            }
        }
    }

    private void SpawnHand() {
        if(currentHand < hands.Count() && findUnusedHand())
        {
            bool hasSpawned = false;
            while (!hasSpawned && currentHand < hands.Count())
            {
                var previousHand = currentHand <= 0 ? new Vector3(0.0f, 0.0f) : lastHandSpawnedInitialPosition;
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

                var angle = calculateAngle(previousHand, nextHand);
                if (handsChecked == activeHands.Count() &&
                (currentHand == 0 || angle >= spawnAngle ))
                {
                    lastHandSpawnedInitialPosition = nextHand;
                    hands[currentHand++].SetActive(true);
                    hasSpawned = true;
                }
                else
                {
                    currentHand++;
                }
            }
        } else if (currentHand >= hands.Count - 1 && hands.Count > 0)
        {
            currentHand = 0;
        } else
        {
            isSpawning = false;
        }
    }

    private bool findUnusedHand()
    {
        for(; currentHand < hands.Count(); currentHand++)
        {
            if (!hands[currentHand].GetComponent<HandScript>().wasUsed)
            {
                return true;
            }
        }
        return false;
    }

    private double calculateAngle(Vector3 hand, Vector3 nextHand)
    {
        var d1 = Math.Sqrt(Math.Pow(hand.x, 2) + Math.Pow(hand.y, 2));
        var d2 = Math.Sqrt(Math.Pow(nextHand.x, 2) + Math.Pow(nextHand.y, 2));
        var d3 = Math.Sqrt(Math.Pow(hand.x - nextHand.x, 2) + Math.Pow(hand.y - nextHand.y, 2));
        return (Math.Acos((-Math.Pow(d3, 2) + Math.Pow(d2, 2) + Math.Pow(d1, 2)) / (2 * d1 * d2)) / Math.PI) * 180;
    }

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
