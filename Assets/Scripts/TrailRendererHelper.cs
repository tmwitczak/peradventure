using System.Collections;
using UnityEngine;

public class TrailRendererHelper : MonoBehaviour {
    protected TrailRenderer mTrail;
    protected float mTime = 0f;

    void Awake() {
        mTrail = gameObject.GetComponent<TrailRenderer>();
        if (null == mTrail) {
            return;
        }
        mTime = mTrail.time;
    }

    void OnEnable() {
        if (null == mTrail) {
            return;
        }
        mTrail.Clear();
        StartCoroutine(ResetTrails());
    }

    IEnumerator ResetTrails() {
        mTrail.time = -1f;
        yield return new WaitForEndOfFrame();
        mTrail.time = mTime;
    }
}
