using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timekeeper : MonoBehaviour
{
    public List<Image> backgroundPlaneImages;
    private List<Material> backgroundMaterials;
    public DataCollectorScript dataCollector;
    // public List<GameObject>  
    // Start is called before the first frame update
    void Start()
    {
        backgroundMaterials = new List<Material>();
        for (int i = 0; i < backgroundPlaneImages.Count; ++i) {
            backgroundMaterials.Add(backgroundPlaneImages[i].material);
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < backgroundMaterials.Count; ++i) {
            backgroundMaterials[i].SetFloat("_UnscaledTime", Time.unscaledTime);

            float max = 100 * Mathf.Pow(2, dataCollector.hiveLevel - 1);
            float min = Convert.ToSingle(dataCollector.hiveLevel > 1)
                * 100 * Mathf.Pow(2, dataCollector.hiveLevel - 2);
            float fill = (dataCollector.honeyAmount - min) / (max - min);

            backgroundMaterials[i].SetFloat("_FillRange", fill);
        }
    }
}
