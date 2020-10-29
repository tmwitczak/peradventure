using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelMenuScript : MonoBehaviour
{
    public DataCollectorScript dataCollector;
    public GameObject[] levels;
    // Start is called before the first frame update
    void Start()
    {
        if (!dataCollector.LoadData())
        {
            dataCollector.levelsUnlocked = 1;
        } else
        {
            for(int i = 0; i<dataCollector.levelsUnlocked; i++)
            {
                unlockLevel(i);
            }
        }
    }
    
    void unlockLevel(int i)
    {
        levels[i].GetComponent<Button>().interactable = true;
        levels[i].GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
        levels[i].transform.GetChild(0).gameObject.SetActive(true);
        levels[i].transform.GetChild(1).gameObject.SetActive(false);
    }
}
