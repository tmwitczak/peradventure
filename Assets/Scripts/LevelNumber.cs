using UnityEngine;
using UnityEngine.UI;

public class LevelNumber : MonoBehaviour {
    private Text text;

    private void Start() {
        text = gameObject.GetComponent<Text>();
    }

    private void Update() {
        text.text = Mathf.Max(Global.hiveLevel, 1).ToString();
    }
}
