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

    private Vector3 screenMax;

    private GameObject angerSymbol;
    private float initialAngerSymbolScale;
    private float pingPongSpeed = 2.0f;
    // private GameObject badWords;

    private bool hasCollided = false;
    private void Start()
    {
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
        // badWords = transform.GetChild(0).gameObject;
        angerSymbol = transform.GetChild(1).gameObject;
        initialAngerSymbolScale = angerSymbol.transform.localScale.x;
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
            angerSymbol.SetActive(true);
            angerSymbol.transform.localScale = new Vector3(Mathf.PingPong(Time.time * pingPongSpeed, 0.5f) + initialAngerSymbolScale, Mathf.PingPong(Time.time * pingPongSpeed, 0.5f) + initialAngerSymbolScale, transform.localScale.y); ;
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
        if (collision.CompareTag("Hive") && isTriggered && !hasCollided)
        {
            hasCollided = true;
            if(honeyCounter.getHoneyAmount() - StealAmount > 0f)
            {
                honeyCounter.setHoneyAmount(honeyCounter.getHoneyAmount() - StealAmount);
            }else
            {
                honeyCounter.setHoneyAmount(0f);
            }
            Handheld.Vibrate();
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Hive") && isTriggered && !hasCollided)
        {
            hasCollided = true;
            if (honeyCounter.getHoneyAmount() - StealAmount > 0f)
            {
                honeyCounter.setHoneyAmount(honeyCounter.getHoneyAmount() - StealAmount);
            }
            else
            {
                honeyCounter.setHoneyAmount(0f);
            }
            Handheld.Vibrate();
            Destroy(gameObject);
        }
    }
}
