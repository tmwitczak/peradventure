using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR;

public class LevelManager : MonoBehaviour
{
    public GameObject Smoke;
    public HandSpawner HandSpawner;
    public GameObject BirdSpawner;
    public DataCollectorScript DataCollector;
    public HoneyCounter HoneyCounter; 
    public GameObject EndLevelHelper;
    public GameObject EndGameMenu;
    public StopwatchScript Stopwatch;

    private IEnumerable<GameObject> HandClones;
    private IEnumerable<GameObject> BirdClones;
    private bool settingParams = true;

    private void Update()
    {
        if(settingParams)
        {
            Debug.Log(DataCollector.levelsUnlocked);
            setLevelParameters(DataCollector.levelsUnlocked);
            settingParams = false;
        }
    }

    public void LoadLevel()
    {
        HandSpawner.PrespawnHands();
        EndLevelHelper.SetActive(true);
        EndGameMenu.SetActive(false);
        resetLevelParameters();
        Smoke.GetComponent<SmokeBehaviour>().ClearSmoke();
        Stopwatch.resetStopwatch();

        HoneyCounter.HoneyAmount = 0.0f;

    }

    public void resetLevelParameters()
    {
        settingParams = true;
    }

    void setLevelParameters(int levelNumber)
    {
        switch (levelNumber)
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

    public void DestroyHands()
    {
        HandSpawner.isSpawning = false;
        HandClones = Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.name == "Hand(Clone)");
        foreach (var obj in HandClones)
        {
            Destroy(obj);
        }
    }

    public void DestroyBirds()
    {
        BirdSpawner.GetComponent<BirdSpawnerScript>().isSpawning = false;
        BirdClones = Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.name == "Bird(Clone)");
        foreach (var obj in BirdClones)
        {
            Destroy(obj);
        }
    }
}
