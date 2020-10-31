using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BirdSpawnerScript : MonoBehaviour
{
    public GameObject Bird;
    public float BirdSpeed;
    public float SpawnCooldown;
    public int BirdsToSpawn;

    private Vector3 screenMin;
    private Vector3 screenMax;
    private float spawnTimer = 0.0f;
    private List<float> spawnX = new List<float>();
    private List<float> spawnY = new List<float>();
    private int seed = 0;
    private int randomNumber = 0;
    // Start is called before the first frame update
    void Start()
    {
        screenMin = Camera.main.ScreenToWorldPoint(new Vector2(0, 0));
        screenMax = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        for (float i = screenMin.x - 5.0f; i < screenMax.x + 5.0f; i += 0.5f)
        {
            spawnX.Add(i);
        }
        for (float i = screenMin.y; i < screenMax.y; i += 0.5f)
        {
            spawnY.Add(i);
        }
    }

    // Update is called once per frame
    void Update()
    {
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= SpawnCooldown)
        {
            switch (BirdsToSpawn)
            {
                case 12:
                    randomNumber = Random.Range(1, 3);
                    for (int i = 0; i < randomNumber; i++)
                    {
                        SpawnBird();
                    }
                    break;
                case 13:
                    randomNumber = Random.Range(1, 4);
                    for (int i = 0; i < randomNumber; i++)
                    {
                        SpawnBird();
                    }
                    break;
                default:
                    for (int i = 0; i < BirdsToSpawn; i++)
                    {
                        SpawnBird();
                    }
                    break;
            }
            spawnTimer = 0.0f;
        }
    }

    private void SpawnBird()
    {
        seed += 100;
        Random.InitState(seed);
        int rand = Random.Range(0, 50);
        spawnX = spawnX.Where(value => value <= screenMin.x || value >= screenMax.x).ToList();
        spawnY = spawnY.Where(value => value >= screenMin.y || value <= screenMax.y).ToList();
        var spawnPosition = new Vector2(spawnX[Random.Range(0, spawnX.Count)], spawnY[Random.Range(0, spawnY.Count)]);
        var bird = Instantiate(Bird, spawnPosition, Quaternion.identity);
        bird.GetComponent<BirdScript>().Speed = BirdSpeed;
    }
}
