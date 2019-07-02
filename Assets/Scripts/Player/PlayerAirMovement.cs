using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerAirMovement : MonoBehaviour {
    [SerializeField] float _speed = 1.5f;

    private Vector3 _sqrLiftoffVelocity;

    private Rigidbody _rigidbody;

    private void Awake() {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.freezeRotation = true;
    }

    void FixedUpdate() {
        var targetVelocity = new Vector3(Input.GetAxis("HorizontalMovement"), 0.0f, Input.GetAxis("VerticalMovement"));
        targetVelocity = Vector3.ClampMagnitude(targetVelocity, 1.0f);
        targetVelocity = transform.TransformDirection(targetVelocity);
        targetVelocity *= _speed;

        var velocity = _rigidbody.velocity;
        var airVelocityChange = targetVelocity - velocity;
        airVelocityChange.y = 0.0f;
        var targetVelocityChange = airVelocityChange * _speed;
        _rigidbody.AddForce(targetVelocityChange);
    }

    public void Enable() {
        enabled = true;
        _sqrLiftoffVelocity = _rigidbody.velocity;
        _sqrLiftoffVelocity.y = 0.0f;
    }

    public void Disable() {
        enabled = false;
    }
}
