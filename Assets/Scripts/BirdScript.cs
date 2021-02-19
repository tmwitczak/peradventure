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

    private Vector3 transformRightTarget;
    private float angularSpeed = -2f * Mathf.PI * Mathf.Rad2Deg;
    private float angularAcceleration = -3f * Mathf.PI * Mathf.Rad2Deg;

    private Vector2 hiveDirection;

    [SerializeField] private GameObject angerSymbol;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject model;
    private Vector3 initialAngerSymbolScale;
    private float pingPongSpeed = 2.0f;

    private bool hasCollided = false;

    private void Awake() {
        iTween.Init(gameObject);

        if (transform.position.x > Global.screenMaxWorldPoint.x) {
            Vector3 localScale = transform.localScale;
            localScale.x *= -1;
            transform.localScale = localScale;

            headsRight = false;
        }
        Hive = GameObject.FindGameObjectWithTag("Hive");
        initialAngerSymbolScale = angerSymbol.transform.localScale;
    }

    private void Start() {
        honeyCounter = FindObjectOfType<HoneyCounter>();
    }

    private void Update() {
        lifeTimer += Time.deltaTime;

        if (isTriggered) {
            angerSymbol.transform.localScale = new Vector3(
                    (0.5f + Mathf.PingPong(Time.time * pingPongSpeed, 1f)) * initialAngerSymbolScale.x,
                    transform.localScale.y,
                    (0.5f + Mathf.PingPong(Time.time * pingPongSpeed, 1f)) * initialAngerSymbolScale.z);

            transform.position = Vector2.MoveTowards(transform.position, Hive.transform.position, speed * Time.deltaTime);

            angularSpeed += angularAcceleration * Time.deltaTime;
            model.transform.Rotate(0f, 0f, angularSpeed * Time.deltaTime);
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
            animator.speed *= 1.25f;
            animator.SetLayerWeight(animator.GetLayerIndex("Attack"), 1f);
            animator.SetTrigger("Prepare");

            hiveDirection = Hive.transform.position - transform.position;
            transformRightTarget = (headsRight ? 1 : -1) * hiveDirection;

            iTween.Stop(gameObject);
            iTween.ValueTo(gameObject, iTween.Hash(
                "from", 0f,
                "to", 1f,
                "time", 0.5f,
                "ignoretimescale", false,
                "onupdate", "tweenOnUpdateTransformRightCallback",
                "easetype", iTween.EaseType.easeInOutCubic
                )
            );
        }
    }

    private void OnTriggerStay2D(Collider2D other) {
        OnTriggerEnter2D(other);
    }

    private void tweenOnUpdateTransformRightCallback(float value) {
        transform.right = Vector3.Lerp(transform.right, transformRightTarget, value);
    }
}
