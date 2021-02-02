using System.Linq;
using UnityEngine;

public class LevelManager : MonoBehaviour {
    public EndgameMenu endgameMenu;
    public GameObject birdSpawner;
    public GameObject endLevelHelper;
    public GameObject mainMenu;
    public GameObject smoke;
    public HandSpawner handSpawner;
    public HoneyCounter honeyCounter;
    public StopwatchScript stopwatch;

    private void Awake() {
        mainMenu.SetActive(true);
    }

    private void Start() {
        loadLevel(Global.currentGameplayLevel);
    }

    public void loadLevel(int level) {
        Random.InitState(level); // Make the game deterministic at every level
        resetScene();
        setLevelParameters(level);
    }

    public void resetScene() {
        Debug.Log("Scene reset");

        // Hands
        handSpawner.destroyAllHands();
        handSpawner.prespawnHands();

        // Birds
        var birdClones = Resources.FindObjectsOfTypeAll<GameObject>().Where(
                obj => obj.name == "Bird(Clone)");
        foreach (var bird in birdClones) {
            Destroy(bird);
        }

        // Smoke
        smoke.GetComponent<SmokeBehaviour>().clearSmoke();

        // Stopwatch
        stopwatch.resetStopwatch();

        // Honey counter
        honeyCounter.reset();

        // End level helper
        endLevelHelper.SetActive(false);
        endLevelHelper.SetActive(true);

        // Endgame menu
        endgameMenu.Reset();
    }

    private void setLevelParameters(int levelNumber) {
        switch (levelNumber) {
            case 1:
                smoke.SetActive(false);
                handSpawner.gameObject.SetActive(true);
                handSpawner.HandSpeed = 2;
                handSpawner.SpawnCooldown = 3;
                handSpawner.HandsToSpawn = 1;
                birdSpawner.SetActive(false);
                birdSpawner.GetComponent<BirdSpawnerScript>().birdSpeed = 2;
                birdSpawner.GetComponent<BirdSpawnerScript>().spawnCooldown = 3;
                birdSpawner.GetComponent<BirdSpawnerScript>().birdsToSpawn = 1;
                break;
            case 2:
                smoke.SetActive(false);
                handSpawner.gameObject.SetActive(true);
                handSpawner.HandSpeed = 2.5f;
                handSpawner.SpawnCooldown = 2;
                handSpawner.HandsToSpawn = 1;
                birdSpawner.SetActive(true);
                birdSpawner.GetComponent<BirdSpawnerScript>().birdSpeed = 2;
                birdSpawner.GetComponent<BirdSpawnerScript>().spawnCooldown = 3;
                birdSpawner.GetComponent<BirdSpawnerScript>().birdsToSpawn = 1;
                break;
            case 3:
                smoke.SetActive(false);
                handSpawner.gameObject.SetActive(true);
                handSpawner.HandSpeed = 2.5f;
                handSpawner.SpawnCooldown = 2;
                handSpawner.HandsToSpawn = 1;
                birdSpawner.SetActive(true);
                birdSpawner.GetComponent<BirdSpawnerScript>().birdSpeed = 2;
                birdSpawner.GetComponent<BirdSpawnerScript>().spawnCooldown = 3;
                birdSpawner.GetComponent<BirdSpawnerScript>().birdsToSpawn = 1;
                break;
            case 4:
                smoke.SetActive(true);
                smoke.GetComponent<SmokeBehaviour>().smokeCooldown = 8.0f;
                handSpawner.gameObject.SetActive(true);
                handSpawner.HandSpeed = 3f;
                handSpawner.SpawnCooldown = 1.5f;
                handSpawner.HandsToSpawn = 1;
                birdSpawner.SetActive(true);
                birdSpawner.GetComponent<BirdSpawnerScript>().birdSpeed = 2;
                birdSpawner.GetComponent<BirdSpawnerScript>().spawnCooldown = 3;
                birdSpawner.GetComponent<BirdSpawnerScript>().birdsToSpawn = 1;
                break;
            default:
                smoke.SetActive(false);
                handSpawner.gameObject.SetActive(true);
                birdSpawner.SetActive(false);
                birdSpawner.GetComponent<BirdSpawnerScript>().birdSpeed = 2;
                birdSpawner.GetComponent<BirdSpawnerScript>().spawnCooldown = 3;
                birdSpawner.GetComponent<BirdSpawnerScript>().birdsToSpawn = 1;
                handSpawner.HandSpeed = 2f;
                handSpawner.SpawnCooldown = 2;
                handSpawner.HandsToSpawn = 1;
                break;
        }
    }
}
