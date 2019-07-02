using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[RequireComponent(typeof(Rigidbody))]
public class PlayerHorizontalBoost : SerializedMonoBehaviour {
    [SerializeField] float _boostVelocity = 100.0f;
    [SerializeField] Dictionary<AdvancedInput, Vector3> _inputs;

    private bool _enabled = true;

    private Rigidbody _rigidbody;

    private void Awake() {
        _rigidbody = GetComponent<Rigidbody>();

        foreach (var input in _inputs) {
            var inputGO = GameObject.Instantiate(input.Key);
            inputGO.transform.parent = transform;
            inputGO.doubleTapped.dynamicCalls += () => OnDoubleTapped(input.Value);
        }
    }

    private void OnDoubleTapped(Vector3 direction) {
        if (!_enabled) {
            return;
        }
        print(direction);
        var targetVelocity = direction;
        targetVelocity = Vector3.ClampMagnitude(targetVelocity, 1.0f);
        targetVelocity = transform.TransformDirection(targetVelocity);
        targetVelocity *= _boostVelocity;

        var velocityChange = targetVelocity - _rigidbody.velocity;
        velocityChange.y = 0.0f;
        var targetVelocityChange = velocityChange * _boostVelocity;
        _rigidbody.AddForce(targetVelocityChange);
    }
}
