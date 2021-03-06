using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelManager : MonoBehaviour {
    public BackgroundVariantGenerator backgroundVariantGenerator;
    public EndgameMenu endgameMenu;
    public GameObject birdSpawner;
    public GameObject endLevelHelper;
    public GameObject mainMenu;
    public GameObject smoke;
    public HandSpawner handSpawner;
    public HoneyCounter honeyCounter;
    public PreRenderBackground preRenderBackground;
    public StopwatchScript stopwatch;

    private List<LevelParameters> startupLevelParameters;

    private void Awake() {
        mainMenu.SetActive(true);
        setupLevelParameters();
    }

    private void Start() {
        Global.appVersion = Application.version;
        loadLevel(Global.currentGameplayLevel);
        backgroundVariantGenerator.setLighting(Global.currentGameplayLevel);
        preRenderBackground.setBackgroundTextures(Global.currentGameplayLevel);
        preRenderBackground.refresh();
    }

    public void loadLevel(int level) {
        Random.InitState(level); // Make the game deterministic at every level
        resetScene();
        setLevelParameters(level);
    }

    public void resetScene() {
        Debug.Log("Scene reset");

        // Hands
        handSpawner.reset();

        // Birds
        birdSpawner.GetComponent<BirdSpawnerScript>().reset();

        // Smoke
        smoke.GetComponent<SmokeBehaviour>().reset();

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

    public class LevelParameters {
        public class Description {
            public bool active;
            public int spawnCountAtOnce;
            public float velocity;
            public float interval;

            public Description(bool active, int spawnCountAtOnce, float velocity, float interval) {
                this.active = active;
                this.spawnCountAtOnce = spawnCountAtOnce;
                this.velocity = velocity;
                this.interval = interval;
            }
        }

        public Description hands;
        public Description birds;
        public Description smoke;

        public LevelParameters(Description hands, Description birds, Description smoke) {
            this.hands = hands;
            this.birds = birds;
            this.smoke = smoke;
        }
    }

    private void setupLevelParameters() {
        // TODO: Rewrite the level management as JSON-centric
        startupLevelParameters = new List<LevelParameters>();

        // Nothing
        startupLevelParameters.Add(new LevelParameters(
                    /* hands */ new LevelParameters.Description(false, 0, 0f, 0f),
                    /* birds */ new LevelParameters.Description(false, 0, 0f, 0f),
                    /* smoke */ new LevelParameters.Description(false, 0, 0f, 0f)));
        // Hands only
        startupLevelParameters.Add(new LevelParameters(
                    /* hands */ new LevelParameters.Description(true, 1, 1f, 3f),
                    /* birds */ new LevelParameters.Description(false, 0, 0f, 0f),
                    /* smoke */ new LevelParameters.Description(false, 0, 0f, 0f)));
        // Smoke only
        startupLevelParameters.Add(new LevelParameters(
                    /* hands */ new LevelParameters.Description(false, 0, 0f, 0f),
                    /* birds */ new LevelParameters.Description(false, 0, 0f, 0f),
                    /* smoke */ new LevelParameters.Description(true, 0, 0f, 4f)));
        // Hands and smoke
        startupLevelParameters.Add(new LevelParameters(
                    /* hands */ new LevelParameters.Description(true, 1, 2.5f, 2f),
                    /* birds */ new LevelParameters.Description(false, 0, 0f, 0f),
                    /* smoke */ new LevelParameters.Description(true, 0, 0f, 8f)));
        // Hands and birds
        startupLevelParameters.Add(new LevelParameters(
                    /* hands */ new LevelParameters.Description(true, 1, 2.5f, 2f),
                    /* birds */ new LevelParameters.Description(true, 1, 2f, 4f),
                    /* smoke */ new LevelParameters.Description(false, 0, 0f, 8f)));
        // Smoke and birds
        startupLevelParameters.Add(new LevelParameters(
                    /* hands */ new LevelParameters.Description(false, 0, 0f, 0f),
                    /* birds */ new LevelParameters.Description(true, 1, 2f, 4f),
                    /* smoke */ new LevelParameters.Description(true, 0, 0f, 8f)));
        // Multiple hands
        startupLevelParameters.Add(new LevelParameters(
                    /* hands */ new LevelParameters.Description(true, 2, 2.5f, 3f),
                    /* birds */ new LevelParameters.Description(false, 0, 0f, 0f),
                    /* smoke */ new LevelParameters.Description(false, 0, 0f, 0f)));
        // Hands, birds and smoke
        startupLevelParameters.Add(new LevelParameters(
                    /* hands */ new LevelParameters.Description(true, 3, 2f, 4f),
                    /* birds */ new LevelParameters.Description(true, 2, 3f, 6f),
                    /* smoke */ new LevelParameters.Description(true, 0, 0f, 15f)));
    }

    private void setLevelParameters(int levelNumber) {
        var parameters = startupLevelParameters.ElementAt((levelNumber - 1) % startupLevelParameters.Count);

        // Hands
        handSpawner.gameObject.SetActive(parameters.hands.active);
        if (handSpawner.gameObject.activeSelf) {
            handSpawner.HandsToSpawn = parameters.hands.spawnCountAtOnce;
            handSpawner.SpawnCooldown = parameters.hands.interval;
            handSpawner.HandSpeed = parameters.hands.velocity;
        }

        // Birds
        birdSpawner.SetActive(parameters.birds.active);
        if (birdSpawner.activeSelf) {
            birdSpawner.GetComponent<BirdSpawnerScript>().birdsToSpawn = parameters.birds.spawnCountAtOnce;
            birdSpawner.GetComponent<BirdSpawnerScript>().spawnCooldown = parameters.birds.interval;
            birdSpawner.GetComponent<BirdSpawnerScript>().birdSpeed = parameters.birds.velocity;
        }

        // Smoke
        smoke.SetActive(parameters.smoke.active);
        if (smoke.activeSelf) {
            smoke.GetComponent<SmokeBehaviour>().smokeCooldown = parameters.smoke.interval;
        }
    }
}
