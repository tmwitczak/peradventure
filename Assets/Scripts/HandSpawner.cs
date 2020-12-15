using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class HandSpawner : MonoBehaviour
{
    public GameObject Hand;
    public float HandSpeed;
    public float SpawnCooldown;
    public int HandsToSpawn;

    private Vector3 screenMin;
    private Vector3 screenMax;
    private float spawnTimer = 0.0f;
    private List<float> spawnX = new List<float>();
    private List<float> spawnY = new List<float>();
    private int seed = 0;
    private int randomNumber = 0;

    // Start is called before the first frame update
    private void Start()
    {
        screenMin = Camera.main.ScreenToWorldPoint(new Vector2(0, 0)) + new Vector3(-2.0f, -2.0f);
        screenMax = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height)) + new Vector3(2.0f, 2.0f);
        for (float i = screenMin.x - 4.0f; i < screenMax.x + 4.0f; i += 0.75f)
        {
            spawnX.Add(i);
        }
        for (float i = screenMin.y - 1.0f; i < screenMax.y + 1.0f; i += 0.75f)
        {
            spawnY.Add(i);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= SpawnCooldown)
        {
            switch (HandsToSpawn)
            {
                case 12:
                    randomNumber = Random.Range(1, 3);
                    for (int i = 0; i < randomNumber; i++)
                    {
                        SpawnHand();
                    }
                    break;
                case 13:
                    randomNumber = Random.Range(1, 4);
                    for (int i = 0; i < randomNumber; i++)
                    {
                        SpawnHand();
                    }
                    break;
                default:
                    for (int i = 0; i < HandsToSpawn; i++)
                    {
                        SpawnHand();
                    }
                    break;
            }
            spawnTimer = 0.0f;
        }
    }

    private void SpawnHand()
    {
        Random.InitState((int)System.DateTime.Now.Ticks);
        int rand = Random.Range(0, 11);
        if (rand % 3 == 1)
        {
            spawnX = spawnX.Where(value => value <= screenMin.x || value >= screenMax.x).ToList();
        }
        else if (rand % 3 == 2)
        {
            spawnY = spawnY.Where(value => value <= screenMin.y || value >= screenMax.y).ToList();
        }
        else
        {
            spawnX = spawnX.Where(value => value <= screenMin.x || value >= screenMax.x).ToList();
            spawnY = spawnY.Where(value => value <= screenMin.y || value >= screenMax.y).ToList();
        }
        var spawnPosition = new Vector2(spawnX[Random.Range(0, spawnX.Count)], spawnY[Random.Range(0, spawnY.Count)]);
        var hand = Instantiate(Hand, spawnPosition, Quaternion.identity);
        hand.GetComponent<HandScript>().Speed = HandSpeed;
    }
}