using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirMove : MonoBehaviour {
    private Rigidbody _rb;

    void Awake() {
        _rb = GetComponent<Rigidbody>();
    }

    private void Update() {
        _rb.AddForce(transform.TransformDirection(new Vector3(Input.GetAxis("HorizontalMovement"), 0.0f, Input.GetAxis("VerticalMovement") * 5)));
    }
}
