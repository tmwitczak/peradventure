using UnityEngine;
using UnityEngine.UI;

public class StopwatchScript : MonoBehaviour {
    public static float MaxTime = 30.0f;
    public Transform clockHandTransform;
    public Transform clockHandStick;
    private Vector3 clockHandStickOriginalScale;
    [SerializeField] GameObject EndGameMenu;
    [SerializeField] GameObject Fill;

    public static float timer = 0.0f;
    private float timeDuration;
    private HoneyCounter honeyCounter;

    public bool isTimerFinished = false;

    private float fmod(float a, float b) {
        return a - b * Mathf.Floor(a / b);
    }

    private float polygonRadius(float angle, float sides, float scale) {
        float alpha = -((sides - 2) * Mathf.PI / (2 * sides));
        float beta = 2 * Mathf.PI / sides;
        float a = Mathf.Tan(alpha);
        float b = fmod(angle, beta);
        return (-scale * a) / (Mathf.Sin(b) - a * Mathf.Cos(b));
    }

    private void Start() {
        timeDuration = MaxTime;
        honeyCounter = FindObjectOfType<HoneyCounter>();
        clockHandStickOriginalScale = clockHandStick.localScale;
    }

    private void Update() {
        if (timer < 1f) {
            timer += Time.deltaTime / timeDuration;
            float timeNormalized = timer % 1f;
            clockHandTransform.eulerAngles = new Vector3(0, 0, -timeNormalized * 360f);
            clockHandStick.localScale = new Vector3(
                    clockHandStickOriginalScale.x,
                    polygonRadius(timeNormalized * 2 * Mathf.PI, 6, clockHandStickOriginalScale.y),
                    clockHandStickOriginalScale.z);
            Fill.GetComponent<Image>().fillAmount = timeNormalized;
        } else {
            timer = 1f;
        }

        if (timer == 1f && !isTimerFinished) {
            honeyCounter.endHoneyAmount = honeyCounter.HoneyAmount;
            EndGameMenu.SetActive(true);
            EndgameMenu.resultsActive = true;
            EndgameMenu.endStart = true;
            Fill.GetComponent<Image>().fillAmount = 1.0f;
            isTimerFinished = true;
        }
    }

    public void resetStopwatch() {
        timer = 0.0f;
        Fill.GetComponent<Image>().fillAmount = 0.0f;
        isTimerFinished = false;
    }
}
