using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Force : MonoBehaviour
{
    [SerializeField] Vector3 _direction;

    private Rigidbody _rigidbody;

    void Awake() {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate() {
        _rigidbody.AddForce(_direction);
    }
}
