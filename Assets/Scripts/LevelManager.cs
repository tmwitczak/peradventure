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
        handSpawner.PrespawnHands();

        // Birds
        var birdClones = Resources.FindObjectsOfTypeAll<GameObject>().Where(
                obj => obj.name == "Bird(Clone)");
        foreach (var bird in birdClones) {
            Destroy(bird);
        }

        // Smoke
        smoke.GetComponent<SmokeBehaviour>().ClearSmoke();

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
                break;
            case 2:
                smoke.SetActive(false);
                handSpawner.gameObject.SetActive(true);
                handSpawner.HandSpeed = 2.5f;
                handSpawner.SpawnCooldown = 2;
                handSpawner.HandsToSpawn = 1;
                birdSpawner.SetActive(true);
                break;
            case 3:
                smoke.SetActive(false);
                handSpawner.gameObject.SetActive(true);
                handSpawner.HandSpeed = 2.5f;
                handSpawner.SpawnCooldown = 2;
                handSpawner.HandsToSpawn = 1;
                birdSpawner.SetActive(true);
                break;
            case 4:
                smoke.SetActive(true);
                handSpawner.gameObject.SetActive(true);
                handSpawner.HandSpeed = 3f;
                handSpawner.SpawnCooldown = 1.5f;
                handSpawner.HandsToSpawn = 1;
                birdSpawner.SetActive(true);
                break;
            default:
                smoke.SetActive(false);
                handSpawner.gameObject.SetActive(true);
                birdSpawner.SetActive(false);
                handSpawner.HandSpeed = 2f;
                handSpawner.SpawnCooldown = 2;
                handSpawner.HandsToSpawn = 1;
                break;
        }
    }
}
