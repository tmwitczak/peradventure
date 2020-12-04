using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

public class BladeScript : MonoBehaviour
{
    public GameObject Trail;
    private GameObject currentTrail;
    private GameObject particleSystem;

    [SerializeField] GameObject starParticles;
    [SerializeField] CircleCollider2D circleCollider;
    [SerializeField] CircleCollider2D circleCollider1;

    private Rigidbody2D rigidbody;
    private Camera camera;
    private HoneyCounter honeyCounter;

    private Vector2 previousPos;

    public float minCutVelocity = .001f;

    private bool isCutting = false;

    // Start is called before the first frame update
    void Start()
    {
        honeyCounter = GameObject.FindGameObjectWithTag("HoneyCounter").GetComponent<HoneyCounter>();
        rigidbody = GetComponent<Rigidbody2D>();
        camera = Camera.main;
        particleSystem = starParticles;
        switchColliders(false);
    }

    // Update is called once per frame
    void Update()
    {
        #region Mouse Inputs
        if (Input.GetMouseButtonDown(0))
        {
            StartCut();
        }
        else if (Input.GetMouseButtonUp(0) && currentTrail != null)
        {
            StopCutting(); 
        }
        #endregion

        #region Mobile Inputs

        if (Input.touchCount > 0)
        {
            if (Input.touches[0].phase == TouchPhase.Began)
            {
                StartCut();
            }
            else if (Input.touches[0].phase == TouchPhase.Ended && currentTrail != null)
            {
                StopCutting();
            }
        }

        #endregion

        if (isCutting)
        {
            UpdateCut();
        }

        if (currentTrail != null)
        {
            currentTrail.transform.position = previousPos;
        }
    }

    void UpdateCut()
    {
        Vector2 newPos = camera.ScreenToWorldPoint(Input.mousePosition);
        rigidbody.position = newPos;

        float velocity = (newPos - previousPos).magnitude / Time.deltaTime;
        if (velocity > minCutVelocity)
        {
            switchColliders(true);
        }
        else {
            switchColliders(false);
        }

        previousPos = newPos;
    }
    
    void StartCut()
    {
        isCutting = true;
        previousPos = camera.ScreenToWorldPoint(Input.mousePosition);
        currentTrail = Instantiate(Trail);
        switchColliders(false);
    }

    void StopCutting()
    {
        isCutting = false;
        Destroy(currentTrail, 1f);
        currentTrail = null;
        switchColliders(false);
    }

    void emitParticles(GameObject hand)
    {
        particleSystem = Instantiate(starParticles);
        particleSystem.transform.position = 
            new Vector3(Mathf.Lerp(transform.position.x, hand.transform.position.x, 0.2f), Mathf.Lerp(transform.position.y, hand.transform.position.y, 0.2f));
        particleSystem.GetComponent<ParticleSystem>().Play();
        Destroy(particleSystem, 1.0f);
    }

    void switchColliders(bool onOff)
    {
        if(!onOff)
        {
            circleCollider.enabled = false;
            circleCollider1.enabled = false;
        } else
        {
            circleCollider.enabled = true;
            circleCollider1.enabled = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Hand"))
        {
            if (other.GetComponent<HandScript>().moveBack)
            {
                honeyCounter.setHoneyAmount(honeyCounter.getHoneyAmount() + other.GetComponent<HandScript>().StealAmount);
            }
            Destroy(other.gameObject);
            emitParticles(other.gameObject);
        } else if (other.CompareTag("Bird"))
        {
            other.gameObject.GetComponent<BirdScript>().isTriggered = true;
        }
    }
}
