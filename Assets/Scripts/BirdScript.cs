using UnityEngine;

public class BirdScript : MonoBehaviour {
    public float StealAmount;
    [HideInInspector]
    public float speed;
    [HideInInspector]
    public bool isTriggered = false;
    public float lifeTime;

    private float lifeTimer = 0f;
    private HoneyCounter honeyCounter;
    private GameObject Hive;
    private bool headsRight = true;

    private Vector2 hiveDirection;

    private GameObject angerSymbol;
    private float initialAngerSymbolScale;
    private float pingPongSpeed = 2.0f;

    private bool hasCollided = false;

    private void Awake() {
        if (transform.position.x > Global.screenMaxWorldPoint.x) {
            Vector3 localScale = transform.localScale;
            localScale.x *= -1;
            transform.localScale = localScale;

            headsRight = false;
        }
        Hive = GameObject.FindGameObjectWithTag("Hive");
        angerSymbol = transform.GetChild(1).gameObject;
        initialAngerSymbolScale = angerSymbol.transform.localScale.x;
    }

    private void Start() {
        honeyCounter = FindObjectOfType<HoneyCounter>();
    }

    private void Update() {
        lifeTimer += Time.deltaTime;

        if (isTriggered) {
            angerSymbol.transform.localScale = new Vector3(
                    Mathf.PingPong(Time.time * pingPongSpeed, 0.5f) + initialAngerSymbolScale,
                    Mathf.PingPong(Time.time * pingPongSpeed, 0.5f) + initialAngerSymbolScale,
                    transform.localScale.y);

            transform.position = Vector2.MoveTowards(transform.position, Hive.transform.position, speed * Time.deltaTime);
            hiveDirection = Hive.transform.position - transform.position;
            transform.right = (headsRight ? 1 : -1) * hiveDirection;
        } else {
            transform.position += (headsRight ? Vector3.right : Vector3.left) * Time.deltaTime * speed;
        }

        if (lifeTimer >= lifeTime) {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Hive") && isTriggered && !hasCollided) {
            hasCollided = true;
            honeyCounter.HoneyAmount -= StealAmount;
            Handheld.Vibrate();
            Destroy(gameObject);
        } else if (other.CompareTag("Blade") && !isTriggered) {
            isTriggered = true;

            angerSymbol.SetActive(true);
            speed *= 1.5f;
        }
    }

    private void OnTriggerStay2D(Collider2D other) {
        OnTriggerEnter2D(other);
    }
}
