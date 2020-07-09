using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeesAmountScript : MonoBehaviour
{
    private Text text;
    private List<GameObject> bees;

    public int InitialNumOfBees
    {
        get => initialNumOfBees;
        set => initialNumOfBees = value;
    }

    private int initialNumOfBees;
    public int ActualNumOfBees
    {
        get { return actualNumOfBees; }
    }
    private int actualNumOfBees;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
        initialNumOfBees = GameObject.FindGameObjectsWithTag("Bee").Length;
    }

    // Update is called once per frame
    void Update()
    {
        actualNumOfBees = GameObject.FindGameObjectsWithTag("Bee").Length;
        text.text = actualNumOfBees + "/" + initialNumOfBees;
    }
}
