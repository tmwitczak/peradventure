﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class BladeScript : MonoBehaviour
{
    public TrailRenderer Trail;
    public float minCutVelocity;

    private Rigidbody2D rigidbody;
    [SerializeField] CircleCollider2D circleCollider;
    [SerializeField] CircleCollider2D circleCollider1;
    private Camera camera;
    private TrailRenderer currentTrail;
    private Vector2 previousPos;
    private HoneyCounter honeyCounter;

    private float timer = 0.0f;
    private bool isCutting = false;
    // Start is called before the first frame update
    void Start()
    {
        honeyCounter = GameObject.FindGameObjectWithTag("HoneyCounter").GetComponent<HoneyCounter>();
        rigidbody = GetComponent<Rigidbody2D>();
        camera = Camera.main;
        circleCollider.enabled = false;
        circleCollider1.enabled = false;
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
            if(timer > 0.0f)
            {
                Trail.Clear();
                currentTrail.Clear();
                timer -= Time.deltaTime;
            }
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
        timer = 0.05f;
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
            honeyCounter.setHoneyAmount(honeyCounter.getHoneyAmount() + other.GetComponent<HandScript>().StealAmount);
            Destroy(other.gameObject);
        } else if (other.CompareTag("Bird"))
        {
            other.gameObject.GetComponent<BirdScript>().isTriggered = true;
        }
    }
}
