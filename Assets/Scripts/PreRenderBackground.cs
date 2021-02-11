using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PreRenderBackground : MonoBehaviour {
    public BackgroundVariantGenerator backgroundVariantGenerator;
    public GameObject environment;
    public List<GameObject> backgroundToTextureCameras;
    public List<Image> backgroundPlaneImages;

    private Material masterMaterial;

    private int frames;
    public int targetCamera;

    private void Awake() {
        frames = 0;
        targetCamera = 0;

        iTween.Init(gameObject);

        masterMaterial = Instantiate(backgroundPlaneImages[1].material);

        for (int i = 0; i < backgroundPlaneImages.Count; ++i) {
            backgroundPlaneImages[i].material = masterMaterial;
        }

        setBlending(0);

        for (int i = 0; i < backgroundToTextureCameras.Count; ++i) {
            GameObject.Destroy(backgroundToTextureCameras[i]);
        }
        GameObject.Destroy(environment);
    }

    [ContextMenu("Refresh")]
    public void refresh() {
        targetCamera = 1 - targetCamera; // Flip between 0 and 1
    }

    private int l, nl, tc;

    [ContextMenu("Run transition")]
    public void runTransition(int level, int nextLevel) {
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
        l = level;
        nl = nextLevel;
        tc = targetCamera;
    }

    private void setBlending(float a) {
        masterMaterial.SetFloat("_MixRange", a);

        // Nasty way of refreshing the unruly shaders
        for (int i = 0; i < backgroundPlaneImages.Count; ++i) {
            // Force refresh all materials
            var enabled = backgroundPlaneImages[i].enabled;
            backgroundPlaneImages[i].enabled = false;
            backgroundPlaneImages[i].enabled = true;
        }
    }

    private void tweenOnUpdateCallBack(float newValue) {
        setBlending(newValue);
        backgroundVariantGenerator.lerpRotation(l, nl, tc == 1 ? 1 - newValue : newValue);
    }

    public void setBackgroundTextures(int level) {
        Debug.Log("Background texture update");

        masterMaterial.SetTexture("_MainTex",
                backgroundVariantGenerator.backgroundTextures[
                    backgroundVariantGenerator.rotationAtLevel(level + Convert.ToInt32(targetCamera))]);
        masterMaterial.SetTexture("_SecTex",
                backgroundVariantGenerator.backgroundTextures[
                    backgroundVariantGenerator.rotationAtLevel(level + Convert.ToInt32(1 - targetCamera))]);
    }
}
