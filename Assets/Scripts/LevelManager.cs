using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameObject Smoke;
    public HandSpawner HandSpawner;
    public GameObject BirdSpawner;
    public DataCollectorScript DataCollector;
    public HoneyCounter HoneyCounter;

    private bool settingParams = true;

    private void Update()
    {
        if(settingParams)
        {
            setLevelParameters(DataCollector.levelsUnlocked);
            settingParams = false;
            HoneyCounter.HoneyAmount = 0.0f;
        }
    }

    void setLevelParameters(int levelNumber)
    {
        switch(levelNumber)
        {
            case 1:
                Smoke.SetActive(false);
                HandSpawner.HandSpeed = 2;
                HandSpawner.SpawnCooldown = 3;
                HandSpawner.HandsToSpawn = 1;
                HandSpawner.handsToPrespawn = 100;
                BirdSpawner.SetActive(false);
                break;
            case 2:
                Smoke.SetActive(false);
                HandSpawner.HandSpeed = 2.5f;
                HandSpawner.SpawnCooldown = 2;
                HandSpawner.HandsToSpawn = 1;
                HandSpawner.handsToPrespawn = 100;
                BirdSpawner.SetActive(true);
                break;
            case 3:
                Smoke.SetActive(true);
                HandSpawner.HandSpeed = 3f;
                HandSpawner.SpawnCooldown = 2;
                HandSpawner.HandsToSpawn = 1;
                HandSpawner.handsToPrespawn = 100;
                BirdSpawner.SetActive(true);
                break;
            case 4:
                Smoke.SetActive(true);
                HandSpawner.HandSpeed = 3f;
                HandSpawner.SpawnCooldown = 1.5f;
                HandSpawner.HandsToSpawn = 1;
                HandSpawner.handsToPrespawn = 100;
                BirdSpawner.SetActive(true);
                break;
            default:
                Smoke.SetActive(false);
                BirdSpawner.SetActive(false);
                HandSpawner.HandSpeed = 2f;
                HandSpawner.SpawnCooldown = 2;
                HandSpawner.HandsToSpawn = 1;
                HandSpawner.handsToPrespawn = 100;
                break;
        }
    }

    public void resetLevelParameters()
    {
        settingParams = true;
    }
}
