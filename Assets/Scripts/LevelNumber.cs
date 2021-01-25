using UnityEngine;
using UnityEngine.UI;

public class LevelNumber : MonoBehaviour {
    private Text text;

    void Start() {
        text = gameObject.GetComponent<Text>();
    }

    void Update() {
        text.text = Mathf.Max(Global.hiveLevel, 1).ToString();
    }
}
