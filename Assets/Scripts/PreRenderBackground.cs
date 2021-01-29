using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PreRenderBackground : MonoBehaviour {
    public GameObject environment;
    public List<GameObject> backgroundToTextureCameras;
    public List<Image> backgroundPlaneImages;

    private Material masterMaterial;

    private int frames = 0;
    private int targetCamera = 0;

    private void Start() {
        iTween.Init(gameObject);

        masterMaterial = Instantiate(backgroundPlaneImages[0].material);

        foreach (var image in backgroundPlaneImages) {
            image.material = masterMaterial;
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
        masterMaterial.SetFloat("_MixRange", a);
    }

    private void tweenOnUpdateCallBack(float newValue) {
        setBlending(newValue);
    }

    private void toggleRendering(bool activate) {
        for (int i = 0; i < backgroundToTextureCameras.Count; ++i) {
            backgroundToTextureCameras[i].SetActive(activate & i == targetCamera);
        }

        environment.SetActive(activate);

        if (activate) {
            frames = 0;
        } else {
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
