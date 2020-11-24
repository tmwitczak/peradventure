using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class HandScript : MonoBehaviour
{
    public float Speed;
    public float StealAmount;
    public GameObject Hive;
    private GameObject honey;

    public bool moveBack;

    private Vector3 initialPos;

    private HoneyCounter HoneyCounter;

    private float destructionTimer = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        moveBack = false;
        initialPos = transform.position;
        honey = gameObject.transform.GetChild(0).gameObject;
        HoneyCounter = GameObject.FindGameObjectWithTag("HoneyCounter").GetComponent<HoneyCounter>();
    }

    // Update is called once per frame
    void Update()
    {
        float step = Speed * Time.deltaTime;
        if (!moveBack)
        {
            transform.position = Vector2.MoveTowards(transform.position, Hive.transform.position, step);
            Vector3 direction = (Hive.transform.position - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion lookRotation = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, step * 6);
        }
        else
        {
            honey.SetActive(true);
            transform.position = Vector2.MoveTowards(transform.position, initialPos, step);
            destructionTimer += Time.deltaTime;
            if (destructionTimer >= 1.0f)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Hive"))
        {
            moveBack = true;
            if (HoneyCounter.getHoneyAmount() - StealAmount <= 0.0f)
            {
                HoneyCounter.setHoneyAmount(0.0f);
            }else HoneyCounter.setHoneyAmount(HoneyCounter.getHoneyAmount() - StealAmount);
        }
    }
}
