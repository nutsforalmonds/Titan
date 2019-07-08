using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBoost : MonoBehaviour {
    private bool _boosting = false;
    public bool Boosting { get { return _boosting; } }

    public UltEvents.UltEvent boostStarted;
    public UltEvents.UltEvent boostEnded;

    private void Update() {
        var isBoosting = Input.GetButton("Sprint");
        if (_boosting != isBoosting) {
            _boosting = isBoosting;
            if (_boosting) {
                boostStarted.Invoke();
            } else {
                boostEnded.Invoke();
            }
        }
    }
}
