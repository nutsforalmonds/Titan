using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class HookshotHead : MonoBehaviour {
    private void OnCollisionEnter(Collision other) {
        var rb = GetComponent<Rigidbody>();
        rb.collisionDetectionMode = CollisionDetectionMode.Discrete;
        GetComponent<Rigidbody>().isKinematic = true;
    }

    public void Reel(float rate, float delta) {
        GetComponent<RopeCollidePoint>().Reel(rate, delta);
    }

    public void Destroy() {
        GetComponent<RopeCollidePoint>().Destroy();
    }
}
