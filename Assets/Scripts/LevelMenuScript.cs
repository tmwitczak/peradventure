using UnityEngine;
using UnityEngine.UI;

public class LevelMenuScript : MonoBehaviour {
    public GameObject[] levelsButton;

    void Start() {
        for (int i = 1; i < Global.levelsUnlocked; i++) {
            if (i >= 4) continue;
            unlockLevel(i);
        }
        for (int i = 0; i < Global.levelsUnlocked - 1; i++) {
            if (i >= 5) continue;
            tickFinishedLevels(i);
        }
    }

    void unlockLevel(int i) {
        levelsButton[i].GetComponent<Button>().interactable = true;
        levelsButton[i].GetComponent<Image>().color = Color.white;
        levelsButton[i].transform.GetChild(0).gameObject.SetActive(true);
        levelsButton[i].transform.GetChild(1).gameObject.SetActive(false);
    }

    void tickFinishedLevels(int i) {
        if (i == 0) {
            levelsButton[i].transform.GetChild(1).gameObject.SetActive(true);
        } else levelsButton[i].transform.GetChild(2).gameObject.SetActive(true);
        levelsButton[i].transform.GetChild(0).gameObject.SetActive(false);
    }
}
