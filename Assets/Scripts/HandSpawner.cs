using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

using Random = UnityEngine.Random;

public class HandSpawner : MonoBehaviour {
    public GameObject Hand;
    public float HandSpeed;
    public float SpawnCooldown;
    public int HandsToSpawn;
    [SerializeField] private int handsToPrespawn = 100;
    public float innerPadding;
    public float outerPadding;

    private List<GameObject> hands = new List<GameObject>();
    private int currentHand = 0;
    private GameObject handParent;

    private Vector3 screenMin;
    private Vector3 screenMax;
    private float spawnTimer = 0.0f;
    private List<float> spawnX = new List<float>();
    private List<float> spawnY = new List<float>();
    private int randomNumber = 0;

    private void Awake() {
        screenMin = Camera.main.ScreenToWorldPoint(new Vector2(0, 0)) + new Vector3(-2.0f, -2.0f);
        screenMax = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height))
            + new Vector3(2.0f, 2.0f);

        for (float i = screenMin.x - outerPadding; i < screenMax.x + outerPadding; i += 0.01f) {
            spawnX.Add(i);
        }
        for (float i = screenMin.y - outerPadding; i < screenMax.y + outerPadding; i += 0.01f) {
            spawnY.Add(i);
        }

        PrespawnHands();
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
        Assert.IsTrue(currentHand < hands.Count);

        hands[currentHand++].SetActive(true);
    }

    public void PrespawnHands() {
        DestroyHands();

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

    public void DestroyHands() {
        foreach (var hand in hands) {
            Destroy(hand);
        }
        currentHand = 0;
        hands.Clear();
        Destroy(handParent);
    }
}
