using UnityEngine;
using UnityEngine.UI;

public class BeeCount : MonoBehaviour {
    private Text text;

    private void Awake() {
        text = gameObject.GetComponent<Text>();
    }

    private void Update() {
        text.text = Global.amountOfBees.ToString();
    }
}
