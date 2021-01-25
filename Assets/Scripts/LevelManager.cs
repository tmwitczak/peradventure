using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelManager : MonoBehaviour {
    public BeesScript BeesScript;
    public GameObject BirdSpawner;
    public GameObject EndGameMenu;
    public GameObject EndLevelHelper;
    public GameObject PBOverlay;
    public GameObject Smoke;
    public HandSpawner HandSpawner;
    public HiveLevel HiveLevel;
    public HoneyCounter HoneyCounter;
    public StopwatchScript Stopwatch;

    private IEnumerable<GameObject> BirdClones;
    private IEnumerable<GameObject> HandClones;
    private bool settingParams = true;

    private void Start() {
    }

    private void Update() {
        if (settingParams) {
            Debug.Log(Global.levelsUnlocked);
            setLevelParameters(Global.levelsUnlocked);
            settingParams = false;
        }
    }

    public void LoadLevel() {
        DestroyHands();
        DestroyBirds();
        HandSpawner.PrespawnHands();
        EndLevelHelper.SetActive(true);
        resetLevelParameters();
        Smoke.GetComponent<SmokeBehaviour>().ClearSmoke();
        Stopwatch.resetStopwatch();
        PBOverlay.SetActive(false);
        HoneyCounter.HoneyAmount = 0.0f;
        HandSpawner.isSpawning = true;
        BirdSpawner.GetComponent<BirdSpawnerScript>().isSpawning = true;
        HiveLevel.resultsActive = false;
        HiveLevel.beeAmountUp = false;
        HiveLevel.finishedLevel = false;
        HiveLevel.endStart = false;
        Debug.Log("load level");
    }

    public void resetLevelParameters() {
        settingParams = true;
    }

    void setLevelParameters(int levelNumber) {
        switch (levelNumber) {
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
                Smoke.SetActive(false);
                HandSpawner.HandSpeed = 2.5f;
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

    public void DestroyHands() {
        HandSpawner.isSpawning = false;
        HandSpawner.DestroyHands();
    }

    public void DestroyBirds() {
        BirdSpawner.GetComponent<BirdSpawnerScript>().isSpawning = false;
        BirdClones = Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.name == "Bird(Clone)");
        foreach (var obj in BirdClones) {
            Destroy(obj);
        }
    }
}
