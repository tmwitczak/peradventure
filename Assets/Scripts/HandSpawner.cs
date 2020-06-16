using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class HandSpawner : MonoBehaviour
{
    public GameObject Hand;
    private Vector3 screenMin;
    private Vector3 screenMax;
    private float spawnTimer = 0.0f;
    private float spawnCooldown = 1.0f;
    private List<float> spawnX = new List<float>();
    private List<float> spawnY = new List<float>();
    private int tmp = 0;

    // Start is called before the first frame update
    private void Start()
    {
        screenMin = Camera.main.ScreenToWorldPoint(new Vector2(0, 0));
        screenMax = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        for (float i = screenMin.x - 5.0f; i < screenMax.x + 5.0f; i+=0.5f)
        {
            spawnX.Add(i);
        }
        for (float i = screenMin.y - 2.0f; i < screenMax.y + 2.0f; i += 0.5f)
        {
            spawnY.Add(i);
        }
        spawnX = spawnX.Where(value => value <= screenMin.x || value >= screenMax.x).ToList();
        spawnY = spawnY.Where(value => value <= screenMin.y || value >= screenMax.y).ToList();
    }

    // Update is called once per frame
    private void Update()
    {
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnCooldown)
        {
            SpawnHand();
            spawnTimer = 0.0f;
        }
    }

    private void SpawnHand()
    {
        tmp += 1000;
        Random.InitState(tmp);
        var spawnPosition = new Vector2(spawnX[Random.Range(0, spawnX.Count)], spawnY[Random.Range(0, spawnY.Count)]);
        Instantiate(Hand, spawnPosition, Quaternion.identity);
    }
}