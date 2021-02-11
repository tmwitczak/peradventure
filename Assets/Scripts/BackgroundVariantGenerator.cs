using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class BackgroundVariantGenerator : MonoBehaviour {
    [SerializeField] private Transform sun;
    [SerializeField] public List<Quaternion> rotations;
    [SerializeField] public List<Texture2D> backgroundTextures;
    [SerializeField] public int seed;

    private Vector3 _euler;

    public List<int> backgroundPerLevel;
    private Random.State rngState;

    public Vector3 euler {
        get {
            _euler = sun.localRotation.eulerAngles;
            return _euler;
        }
        set {
            _euler.x = (value.x + 180f) % 180f;
            _euler.y = value.y;
            _euler.z = 0f;

            sun.localEulerAngles = _euler;
        }
    }

    public int rotationAtLevel(int level) {
        int index = level - 1;

        // Save global RNG state
        Random.State otherRngState = Random.state;

        // Initialize
        if (backgroundPerLevel == null || backgroundPerLevel.Count == 0) {
            backgroundPerLevel = new List<int>();

            Random.InitState(seed);
            rngState = Random.state;
        }

        // Update the list when needed
        Random.state = rngState;
        while (backgroundPerLevel.Count <= index) {
            backgroundPerLevel.Add(Random.Range(0, rotations.Count));

            // Make sure each element is followed by a different one
            if (backgroundPerLevel.Count > 1 &&
                    backgroundPerLevel.ElementAt(backgroundPerLevel.Count - 2)
                    == backgroundPerLevel.ElementAt(backgroundPerLevel.Count - 1)) {
                backgroundPerLevel.RemoveAt(backgroundPerLevel.Count - 1);
            }
        }
        rngState = Random.state;

        // Restore global RNG state
        Random.state = otherRngState;

        return backgroundPerLevel.ElementAt(index);
    }

    private void Awake() {
        backgroundPerLevel = new List<int>();
    }

    public void add() {
        generate();
        rotations.Add(sun.localRotation);
        removeDuplicates();
    }

    public void generate() {
        sun.localRotation = Random.rotation;
        euler = sun.localRotation.eulerAngles;
        sun.localEulerAngles = euler;
    }

    public void save(int index) {
        rotations[index] = sun.localRotation;
    }

    public void remove(int index) {
        rotations.RemoveAt(index);
    }

    public void removeDuplicates() {
        rotations = rotations.Distinct().ToList();
    }

    public void setLighting(int levelNumber) => setRotation(levelNumber);

    public void setRotation(int level) {
        sun.localRotation = rotations.ElementAt(rotationAtLevel(level));
    }
    public void setRotationType(int index) {
        sun.localRotation = rotations.ElementAt(index);
    }

    public void lerpRotation(int a, int b, float factor) {
        sun.localRotation = Quaternion.Lerp(
                rotations.ElementAt(rotationAtLevel(a)),
                rotations.ElementAt(rotationAtLevel(b)),
                factor);
    }
}
