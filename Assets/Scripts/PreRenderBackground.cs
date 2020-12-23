using UnityEngine;

public class PreRenderBackground : MonoBehaviour {
    private int frames = 0;
    private void Update() {
        // Disable after two frames
        if (frames <= 1) {
            frames++;
        } else {
            gameObject.SetActive(false);
            GameObject.Find("Environment").SetActive(false);
        }
    }
}
