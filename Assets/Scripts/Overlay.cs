using UnityEngine;

public class Overlay : MonoBehaviour {
    public bool fadeIn;
    private void Awake() {
        fadeIn = false;
    }
    public void Disable() {
        gameObject.SetActive(fadeIn);
    }
}
