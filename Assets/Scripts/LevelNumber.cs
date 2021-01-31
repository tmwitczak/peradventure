﻿using UnityEngine;
using UnityEngine.UI;

public class LevelNumber : MonoBehaviour {
    private Text text;

    private void Awake() {
        text = gameObject.GetComponent<Text>();
    }

    private void Update() {
        text.text = Global.hiveLevel.ToString();
    }
}
