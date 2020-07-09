using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladeScript : MonoBehaviour
{
    private bool isCutting = true;
    public GameObject Trail;
    public float minCutVelocity;

    private Rigidbody2D rigidbody;
    [SerializeField] CircleCollider2D circleCollider;
    [SerializeField] CircleCollider2D circleCollider1;
    private Camera camera;
    private GameObject currentTrail;
    private Vector2 previousPos;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        #region Mouse Inputs
        if (Input.GetMouseButtonDown(0))
        {
            StartCut();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            StopCutting();
        }
        #endregion

        #region Mobile Inputs

        if (Input.touchCount > 0)
        {
            if (Input.touches[0].phase == TouchPhase.Moved)
            {
                StartCut();
            }
            else if (Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled)
            {
                StopCutting();
            }
        }

        #endregion

        if (isCutting)
        {
            UpdateCut();
        }
    }

    void UpdateCut()
    {
        Vector2 newPos = camera.ScreenToWorldPoint(Input.mousePosition);
        rigidbody.position = newPos;
        float velocity = (newPos - previousPos).magnitude * Time.deltaTime;
        if (velocity > minCutVelocity)
        {
            circleCollider.enabled = true;
            circleCollider1.enabled = true;
        }
        else { 
            circleCollider.enabled = false; 
            circleCollider1.enabled = false; 
        }

        previousPos = newPos;
    }

    void StartCut()
    {
        isCutting = true;
        currentTrail = Instantiate(Trail, transform);
        circleCollider.enabled = false;
        circleCollider1.enabled = false;
    }

    void StopCutting()
    {
        isCutting = false;
        Destroy(currentTrail);
        circleCollider.enabled = false;
        circleCollider1.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Hand"))
        {
            Destroy(other.gameObject);
        }
    }
}
