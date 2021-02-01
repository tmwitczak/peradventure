using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BirdSpawnerScript : MonoBehaviour {
    public GameObject Bird;
    public float BirdSpeed;
    public float SpawnCooldown;
    public int BirdsToSpawn;

    private float spawnTimer = 0.0f;
    private List<float> spawnX = new List<float>();
    private List<float> spawnY = new List<float>();
    private int randomNumber = 0;

    private void Awake() {
        for (float i = Global.screenMinWorldPoint.x - 5.0f;
                i < Global.screenMaxWorldPoint.x + 5.0f;
                i += 0.5f) {
            spawnX.Add(i);
        }
        for (float i = Global.screenMinWorldPoint.y;
                i < Global.screenMaxWorldPoint.y;
                i += 0.5f) {
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
        spawnX = spawnX.Where(value =>
                value <= Global.screenMinWorldPoint.x ||
                value >= Global.screenMaxWorldPoint.x).ToList();
        spawnY = spawnY.Where(value =>
                value >= Global.screenMinWorldPoint.y ||
                value <= Global.screenMaxWorldPoint.y).ToList();

        var spawnPosition = new Vector2(
                spawnX[Random.Range(0, spawnX.Count)],
                spawnY[Random.Range(0, spawnY.Count)]);

        var bird = Instantiate(Bird, spawnPosition, Quaternion.identity);
        bird.GetComponent<BirdScript>().speed = BirdSpeed;
    }
}
