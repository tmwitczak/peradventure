using System.Collections;
using UnityEngine;

public class TrailRendererHelper : MonoBehaviour {
    protected TrailRenderer mTrail;
    protected float mTime = 0f;

    private void Awake() {
        mTrail = gameObject.GetComponent<TrailRenderer>();
        if (null == mTrail) {
            return;
        }
        mTime = mTrail.time;
    }

    private void OnEnable() {
        if (null == mTrail) {
            return;
        }
        mTrail.Clear();
        StartCoroutine(ResetTrails());
    }

    private IEnumerator ResetTrails() {
        mTrail.time = -1f;
        yield return new WaitForEndOfFrame();
        mTrail.time = mTime;
    }
}
