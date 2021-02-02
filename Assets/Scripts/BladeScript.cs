using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BladeScript : MonoBehaviour {
    public GameObject Trail;
    private GameObject currentTrail;
    private GameObject particleSystem;

    [SerializeField] GameObject starParticles;
    [SerializeField] CircleCollider2D circleCollider;
    [SerializeField] CircleCollider2D circleCollider1;

    private Rigidbody2D rigidbody;

    private Vector2 previousPos;

    public float minCutVelocity = .001f;

    private bool isCutting = false;

    private void Awake() {
        rigidbody = GetComponent<Rigidbody2D>();
        particleSystem = starParticles;
        switchColliders(false);
    }

    private void Update() {
        #region Mouse Inputs
        if (Input.GetMouseButtonDown(0)) {
            StartCut();
        } else if (Input.GetMouseButtonUp(0) && currentTrail != null) {
            StopCutting();
        }
        #endregion

        #region Mobile Inputs
        if (Input.touchCount > 0) {
            if (Input.touches[0].phase == TouchPhase.Began) {
                StartCut();
            } else if (Input.touches[0].phase == TouchPhase.Ended && currentTrail != null) {
                StopCutting();
            }
        }
        #endregion

        if (isCutting) {
            UpdateCut();
        }

        if (currentTrail != null) {
            currentTrail.transform.position = previousPos;
        }
    }

    private void UpdateCut() {
        Vector2 newPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        rigidbody.position = newPos;

        float velocity = (newPos - previousPos).magnitude / Time.deltaTime;
        switchColliders(velocity > minCutVelocity);

        previousPos = newPos;
    }

    private void StartCut() {
        isCutting = true;
        previousPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        currentTrail = Instantiate(Trail);
        switchColliders(false);
    }

    public void destroyAllTrails() {
        IEnumerable<GameObject> trails =
            Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.name == "Trail(Clone)");
        foreach (var trail in trails) {
            Destroy(trail);
        }
    }

    private void StopCutting() {
        isCutting = false;
        Destroy(currentTrail, 1f);
        currentTrail = null;
        switchColliders(false);
    }

    private void emitParticles(GameObject hand) {
        particleSystem = Instantiate(starParticles);
        particleSystem.transform.position = new Vector3(
                    Mathf.Lerp(transform.position.x, hand.transform.position.x, 0.2f),
                    Mathf.Lerp(transform.position.y, hand.transform.position.y, 0.2f));
        particleSystem.GetComponent<ParticleSystem>().Play();
        Destroy(particleSystem, 1.0f);
    }

    private void switchColliders(bool onOff) {
        circleCollider.enabled = circleCollider1.enabled = onOff;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Hand")) {
            other.GetComponent<HandScript>().giveBack();
            Destroy(other.gameObject);
            emitParticles(other.gameObject);
        }
    }

    private void OnEnable() {
        destroyAllTrails();
    }

    private void OnDisable() {
        destroyAllTrails();
    }
}
