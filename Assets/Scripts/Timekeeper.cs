using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timekeeper : MonoBehaviour {
    public List<Image> hiveImages;

    private Material masterMaterial;
    private float originalTimescale;

    private void Awake() {
        iTween.Init(gameObject);
        originalTimescale = Time.timeScale;
    }

    private void Start() {
        masterMaterial = Instantiate(hiveImages[0].material);

        foreach (var image in hiveImages) {
            image.material = masterMaterial;
        }
    }

    private void Update() {
        float max = 100 * Mathf.Pow(2, Global.hiveLevel - 1);
        float min = Convert.ToSingle(Global.hiveLevel > 1)
            * 100 * Mathf.Pow(2, Global.hiveLevel - 2);
        float fill = (Global.honeyAmount - min) / (max - min);

        masterMaterial.SetFloat("_UnscaledTime", Time.unscaledTime);
        masterMaterial.SetFloat("_FillRange", fill);
    }

    public void slowdownTimescale() => changeTimescale(originalTimescale, 0.0f);

    public void speedupTimescale() => changeTimescale(0.0f, originalTimescale);

    private void changeTimescale(float from, float to) {
        iTween.Stop(gameObject);
        iTween.ValueTo(gameObject, iTween.Hash(
            "from", from,
            "to", to,
            "time", 0.5f,
            "ignoretimescale", true,
            // "onupdatetarget", gameObject,
            "onupdate", "tweenOnUpdateTimescaleCallback",
            "easetype", iTween.EaseType.easeOutQuad
            )
        );
    }

    private void tweenOnUpdateTimescaleCallback(float value) {
        Time.timeScale = value;
    }
}
