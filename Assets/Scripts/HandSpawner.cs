using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.XR;
using Random = UnityEngine.Random;

public class HandSpawner : MonoBehaviour {
    public GameObject Hand;
    public float HandSpeed;
    public float SpawnCooldown;
    public int HandsToSpawn;
    [SerializeField] private int handsToPrespawn = 100;
    public float innerPadding;
    public float outerPadding;
    public List<GameObject> activeHands = new List<GameObject>();

    private List<GameObject> hands = new List<GameObject>();
    private int currentHand = 0;
    private GameObject handParent;
    private Vector2 handSize;
    private Vector3 lastHandSpawnedInitialPosition;

    private Vector3 screenMin;
    private Vector3 screenMax;
    private float spawnTimer;
    private List<float> spawnX;
    private List<float> spawnY;

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

        handSize = new Vector2(Hand.GetComponent<BoxCollider2D>().size.x, Hand.GetComponent<BoxCollider2D>().size.y);
        lastHandSpawnedInitialPosition = new Vector3(0.0f, 0.0f);
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
        if(currentHand < hands.Count)
        {
            bool hasSpawned = false;
            while (!hasSpawned)
            {
                var previousHand = currentHand <= 0 ? new Vector3(0.0f, 0.0f) : lastHandSpawnedInitialPosition;
                var nextHand = hands[currentHand].GetComponent<HandScript>().initialPosition;
                int handsChecked = 0;
                foreach (var hand in activeHands)
                {
                    var handPosition = hand.GetComponent<HandScript>().initialPosition;
                    if (
                    nextHand.x < handPosition.x - handSize.x ||
                    nextHand.x > handPosition.x + handSize.x ||
                    nextHand.y < handPosition.y - handSize.y ||
                    nextHand.y > handPosition.y + handSize.y)
                    {
                        handsChecked++;
                    }
                }
                if (handsChecked == activeHands.Count() &&
                (currentHand == 0 ||
                nextHand.x < previousHand.x - handSize.x ||
                nextHand.x > previousHand.x + handSize.x ||
                nextHand.y < previousHand.y - handSize.y ||
                nextHand.y > previousHand.y + handSize.y))
                {
                    lastHandSpawnedInitialPosition = nextHand;
                    activeHands.Add(hands[currentHand]);
                    hands[currentHand++].SetActive(true);
                    hasSpawned = true;
                }
                else
                {
                    currentHand++;
                }
            }
        } else if (currentHand > hands.Count && hands.Count > 0)
        {
            currentHand = 0;
        }

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
        Destroy(handParent);
    }

    public void reset() {
        spawnTimer = 0f;
        destroyAllHands();
        prespawnHands();
    }
}
