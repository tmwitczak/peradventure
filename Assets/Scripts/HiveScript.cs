using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class HiveScript : MonoBehaviour {
    [SerializeField] private GameObject branch;
    [SerializeField] private GameObject hive;
    [SerializeField] private float amountMagnitude = 10f;

    private void Awake() {
        iTween.Init(branch);
    }

    private Vector3 moveTowardsZero(Vector3 vector) {
        float speed = 0.05f;
        return Vector3.MoveTowards(
                vector,
                Vector3.zero,
                speed * Time.unscaledDeltaTime);
    }
    private Quaternion rotateTowardsZero(Quaternion quaternion) {
        float speed = 1.0f;
        return Quaternion.Lerp(quaternion, Quaternion.identity, Time.unscaledDeltaTime * speed);
    }


    private void LateUpdate() {
        // Reset the animated properties
        // – iTween sometimes doesn't return them to their initial value
        branch.transform.localRotation = rotateTowardsZero(branch.transform.localRotation);
        branch.transform.localPosition = moveTowardsZero(branch.transform.localPosition);
        hive.transform.localRotation = rotateTowardsZero(hive.transform.localRotation);
    }

    private IEnumerator OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Hand") ||
                (other.CompareTag("Bird") && other.GetComponent<BirdScript>().isTriggered)) {
            iTween.Stop(branch);
            iTween.Stop(hive);

            yield return new WaitForSecondsRealtime(0.02f);

            iTween.PunchRotation(branch, iTween.Hash(
                "amount", new Vector3(
                    -0.5f * amountMagnitude,
                    0f,
                    (Random.Range(0f, 1f) < 0.5f ? -1 : 1) * amountMagnitude),
                "time", Random.Range(2f, 3f),
                "ignoretimescale", true
                )
            );
            iTween.PunchPosition(branch, iTween.Hash(
                "amount", new Vector3(
                    0f,
                    -0.025f * amountMagnitude,
                    0f),
                "time", Random.Range(2f, 3f),
                "ignoretimescale", true
                )
            );
            iTween.PunchRotation(hive, iTween.Hash(
                "amount", new Vector3(
                    0f,
                    (Random.Range(0f, 1f) < 0.5f ? -1 : 1) * amountMagnitude,
                    (Random.Range(0f, 1f) < 0.5f ? -1 : 1) * amountMagnitude),
                "time", Random.Range(2, 3f),
                "ignoretimescale", true
                )
            );
        }
    }
}
