using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BirdSpawnerScript : MonoBehaviour {
    public GameObject Bird;
    public float BirdSpeed;
    public float SpawnCooldown;
    public int BirdsToSpawn;

    private Vector3 screenMin;
    private Vector3 screenMax;
    private float spawnTimer = 0.0f;
    private List<float> spawnX = new List<float>();
    private List<float> spawnY = new List<float>();
    private int randomNumber = 0;

    private void Start() {
        screenMin = Camera.main.ScreenToWorldPoint(new Vector2(0, 0));
        screenMax = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        for (float i = screenMin.x - 5.0f; i < screenMax.x + 5.0f; i += 0.5f) {
            spawnX.Add(i);
        }
        for (float i = screenMin.y; i < screenMax.y; i += 0.5f) {
            spawnY.Add(i);
        }
    }

    private void Update() {
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= SpawnCooldown) {
            spawnTimer = 0.0f;

            for (int i = 0; i < BirdsToSpawn; i++) {
                SpawnBird();
            }
        }
    }

    private void SpawnBird() {
        int rand = Random.Range(0, 50);
        spawnX = spawnX.Where(value => value <= screenMin.x || value >= screenMax.x).ToList();
        spawnY = spawnY.Where(value => value >= screenMin.y || value <= screenMax.y).ToList();
        var spawnPosition = new Vector2(spawnX[Random.Range(0, spawnX.Count)], spawnY[Random.Range(0, spawnY.Count)]);
        var bird = Instantiate(Bird, spawnPosition, Quaternion.identity);
        bird.GetComponent<BirdScript>().Speed = BirdSpeed;
    }
}
