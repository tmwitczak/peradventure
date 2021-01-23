using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timekeeper : MonoBehaviour {
    private List<Material> hiveMaterials;

    public DataCollectorScript dataCollector;
    public List<Image> hiveImages;

    private void Start() {
        hiveMaterials = new List<Material>();
        for (int i = 0; i < hiveImages.Count; ++i) {
            hiveMaterials.Add(hiveImages[i].material);
        }
    }

    private void Update() {
        float max = 100 * Mathf.Pow(2, dataCollector.hiveLevel - 1);
        float min = Convert.ToSingle(dataCollector.hiveLevel > 1)
            * 100 * Mathf.Pow(2, dataCollector.hiveLevel - 2);
        float fill = (dataCollector.honeyAmount - min) / (max - min);

        foreach (var material in hiveMaterials) {
            material.SetFloat("_UnscaledTime", Time.unscaledTime);
            material.SetFloat("_FillRange", fill);
        }
    }
}
