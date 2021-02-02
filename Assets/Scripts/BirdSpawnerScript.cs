using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BirdSpawnerScript : MonoBehaviour {
    public GameObject birdPrefab;
    public float birdSpeed;
    public float spawnCooldown;
    public int birdsToSpawn;

    private float spawnTimer;
    private List<float> spawnX = new List<float>();
    private List<float> spawnY = new List<float>();

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
        if (spawnTimer >= spawnCooldown) {
            spawnTimer = 0.0f;

            for (int i = 0; i < birdsToSpawn; i++) {
                spawnBird();
            }
        }
    }

    private void spawnBird() {
        spawnX = spawnX.Where(value =>
                value <= Global.screenMinWorldPoint.x ||
                value >= Global.screenMaxWorldPoint.x).ToList();
        spawnY = spawnY.Where(value =>
                value >= Global.screenMinWorldPoint.y ||
                value <= Global.screenMaxWorldPoint.y).ToList();

        var spawnPosition = new Vector2(
                spawnX[Random.Range(0, spawnX.Count)],
                spawnY[Random.Range(0, spawnY.Count)]);

        var bird = Instantiate(birdPrefab, spawnPosition, Quaternion.identity);
        bird.GetComponent<BirdScript>().speed = birdSpeed;
    }

    private void destroyAllBirds() {
        var birdClones = Resources.FindObjectsOfTypeAll<GameObject>().Where(
                obj => obj.name == "Bird(Clone)");
        foreach (var bird in birdClones) {
            Destroy(bird);
        }
    }

    public void reset() {
        spawnTimer = 0f;
        destroyAllBirds();
    }
}
