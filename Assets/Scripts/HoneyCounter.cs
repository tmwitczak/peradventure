using UnityEngine;
using UnityEngine.UI;

public class HoneyCounter : MonoBehaviour {
    public BeesScript beesScript;
    public float MinSmokedSpeed = 0.3f;
    public float Speed;
    public float _smokeFactor = 1f;

    private Text text;
    private float _honeyAmount;
    private float initialSmokeFactor;

    public float endHoneyAmount { get; set; }

    private void Awake() {
        text = GetComponent<Text>();
        reset();
    }

    private void Start() {
        initialSmokeFactor = SmokeFactor;
    }

    private void Update() {
        HoneyAmount += Time.deltaTime * ((Global.amountOfBees / 10f) * SmokeFactor) * Speed;
        text.text = (Mathf.Round(16.54f * HoneyAmount)) + "";
    }

    public void reset() {
        HoneyAmount = 0f;
    }

    public float HoneyAmount {
        get => _honeyAmount;
        set => _honeyAmount = Mathf.Max(0f, value);
    }

    public float SmokeFactor {
        get => _smokeFactor;
        set => _smokeFactor = Mathf.Clamp(value, MinSmokedSpeed, initialSmokeFactor);
    }
}
