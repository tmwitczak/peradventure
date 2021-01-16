using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PreRenderBackground : MonoBehaviour {
    public GameObject environment;
    public List<GameObject> backgroundToTextureCameras;
    public List<Image> backgroundPlaneImages;
    private List<Material> backgroundMaterials;

    private int frames = 0;
    private int targetCamera = 0;

    void Start() {
        iTween.Init(gameObject);

        backgroundMaterials = new List<Material>();
        for (int i = 0; i < backgroundPlaneImages.Count; ++i) {
            backgroundMaterials.Add(backgroundPlaneImages[i].material);
        }

        setBlending(0);
    }

    [ContextMenu("Refresh")]
    public void refresh() => toggleRendering(true);

    [ContextMenu("Run transition")]
    public void runTransition() {
        iTween.Stop(gameObject);
        iTween.ValueTo(gameObject, iTween.Hash(
            "from", targetCamera,
            "to", 1 - targetCamera,
            "time", 5.0f,
            "ignoretimescale", true,
            "onupdatetarget", gameObject,
            "onupdate", "tweenOnUpdateCallBack",
            "easetype", iTween.EaseType.easeInOutSine
            )
        );
    }

    private void setBlending(float a) {
        for (int i = 0; i < backgroundMaterials.Count; ++i) {
            backgroundMaterials[i].SetFloat("_MixRange", a);
        }
    }

    private void tweenOnUpdateCallBack(float newValue) {
        setBlending(newValue);
    }

    private void toggleRendering(bool activate) {
        for (int i = 0; i < backgroundToTextureCameras.Count; ++i) {
            backgroundToTextureCameras[i].SetActive(activate & i == targetCamera);
        }

        environment.SetActive(activate);

        if(activate) {
            frames = 0;
        }
        else {
            targetCamera = (targetCamera + 1) % backgroundToTextureCameras.Count;
        }
    }

    private void Update() {
        // Disable after two frames
        if (frames == 0 || frames == 1) {
            frames++;
        } else if (frames == 2) {
            frames++;
            toggleRendering(false);
            runTransition();
        }
    }
}
