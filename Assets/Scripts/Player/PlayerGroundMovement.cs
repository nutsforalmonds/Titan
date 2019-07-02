using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerGroundMovement : MonoBehaviour {
    [SerializeField] float _runSpeed = 5.0f;
    [SerializeField] float _sprintSpeed = 9.0f;
    [SerializeField] float _reactivityPercent = 0.5f;

    private Rigidbody _rigidbody;


    public UltEvents.UltEvent sprintStarted;
    public UltEvents.UltEvent sprintEnded;

    private void Awake() {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.freezeRotation = true;
    }

    void FixedUpdate() {
        var targetVelocity = new Vector3(Input.GetAxis("HorizontalMovement"), 0.0f, Input.GetAxis("VerticalMovement"));
        targetVelocity = Vector3.ClampMagnitude(targetVelocity, 1.0f);
        targetVelocity = transform.TransformDirection(targetVelocity);

        var speed = Mathf.Lerp(_runSpeed, _sprintSpeed, Input.GetAxis("Sprint"));
        targetVelocity *= speed;

        var currentVelocity = new Vector3(_rigidbody.velocity.x, 0.0f, _rigidbody.velocity.z);
        var groundVelocityChange = targetVelocity - currentVelocity;
        groundVelocityChange.y = 0.0f;
        var targetVelocityChange = groundVelocityChange * speed;
        _rigidbody.AddForce(targetVelocityChange);
    }

    public void Enable() {
        enabled = true;
    }

    public void Disable() {
        enabled = false;
    }
}
