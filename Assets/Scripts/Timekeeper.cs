using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timekeeper : MonoBehaviour {
    // public DataCollectorScript dataCollector;
    public List<Image> hiveImages;

    private Material masterMaterial;

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
}
