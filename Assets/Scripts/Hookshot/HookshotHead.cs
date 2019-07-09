using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class HookshotHead : MonoBehaviour {
    private void OnCollisionEnter(Collision other) {
        var rigidbody = GetComponent<Rigidbody>();
        rigidbody.detectCollisions = false;
        rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative; // Need to do this for kinematic rigidbodys even if we're not detecting collisions
        rigidbody.isKinematic = true;
    }

    public void Reel(float rate, float delta) {
        GetComponent<RopeCollidePoint>().Reel(rate, delta);
    }

    public void Destroy() {
        GetComponent<RopeCollidePoint>().Destroy();
    }
}
