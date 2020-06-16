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

    public GameObject Hive;

    private bool moveBack;

    private Vector3 initialPos;

    private float destructionTimer = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        moveBack = false;
        initialPos = transform.position;
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
        }
    }
}
