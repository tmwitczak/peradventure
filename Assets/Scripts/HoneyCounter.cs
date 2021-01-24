using UnityEngine;
using UnityEngine.UI;

public class HoneyCounter : MonoBehaviour {
    public BeesScript beesScript;
    public DataCollectorScript dataCollector;
    public float MinSmokedSpeed = 0.3f;
    public float Speed;
    public float _smokeFactor = 1f;

    private Text text;
    private float _honeyAmount;
    private float initialSmokeFactor;
    private int amountOfBees;

    public float endHoneyAmount { get; set; }

    private void Start() {
        HoneyAmount = 0f;
        text = GetComponent<Text>();
        initialSmokeFactor = SmokeFactor;
    }

    private void Update() {
        amountOfBees = beesScript.getAmountOfBees();
        HoneyAmount += Time.deltaTime * ((amountOfBees / 10f) * SmokeFactor) * Speed;
        text.text = (Mathf.Round(16.54f * HoneyAmount)) + "";
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
