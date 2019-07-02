using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CharacterJumpFall : MonoBehaviour {
    [SerializeField] float _fallModifier = 2.5f;
    [SerializeField] float _lowJumpMultiplier = 2.0f;

    private Rigidbody _rigidbody;


    private void Awake() {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate() {
        var force = Vector3.zero;
        if (_rigidbody.velocity.y < 0.0f) {
            force = Physics.gravity * (_fallModifier - 1) * Time.fixedDeltaTime;
        } else if (!Input.GetButton("Jump")) {
            force = Physics.gravity * (_lowJumpMultiplier - 1) * Time.fixedDeltaTime;
        }
        _rigidbody.velocity += force;
    }

    public void Enable() {
        enabled = true;
    }

    public void Disable() {
        enabled = false;
    }
}
