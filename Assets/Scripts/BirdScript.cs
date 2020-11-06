using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BirdScript : MonoBehaviour
{
    public float StealAmount;
    [HideInInspector]
    public float Speed;
    [HideInInspector]
    public bool isTriggered = false;
    public float lifeTime;

    private float lifeTimer = 0f;
    private HoneyCounter honeyCounter;
    private GameObject Hive;
    private bool headsRight = true;
    private float initialSpeed;

    private Vector2 hiveDirection;

    private Vector3 screenMin;
    private Vector3 screenMax;
    private void Start()
    {
        screenMin = Camera.main.ScreenToWorldPoint(new Vector2(0, 0));
        screenMax = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        if(transform.position.x > screenMax.x)
        {
            Vector3 localScale = transform.localScale;
            localScale.x *= -1;
            transform.localScale = localScale;
            headsRight = false;
        }
        Hive = GameObject.FindGameObjectWithTag("Hive");
        honeyCounter = FindObjectOfType<HoneyCounter>();
        initialSpeed = Speed;
    }
    private void Update()
    {
        lifeTimer += Time.deltaTime;
        if(headsRight && !isTriggered)
        {
            transform.position += Vector3.right * Time.deltaTime * Speed;
        } else if(!headsRight && !isTriggered)
        {
            transform.position += Vector3.left * Time.deltaTime * Speed;
        } else
        {
            Speed = initialSpeed * 1.5f;
            transform.position = Vector2.MoveTowards(transform.position, Hive.transform.position, Speed * Time.deltaTime);
            
            hiveDirection = Hive.transform.position - transform.position;
            transform.right = (headsRight ? 1 : -1) * hiveDirection;
        }
        if(lifeTimer >= lifeTime)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Hive") && isTriggered)
        {
            if(honeyCounter.getHoneyAmount() - StealAmount > 0f)
            {
                honeyCounter.setHoneyAmount(honeyCounter.getHoneyAmount() - StealAmount);
            }else
            {
                honeyCounter.setHoneyAmount(0f);
            }
            Destroy(gameObject);
        }
    }
}
