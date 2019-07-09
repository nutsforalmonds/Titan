using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerVelocity), typeof(PlayerBoost))]
public class PlayerGroundMovement : MonoBehaviour {
    [SerializeField] float _runSpeed = 250.0f;
    [SerializeField] float _boostSpeed = 450.0f;

    private PlayerVelocity _playerVelocity;
    private PlayerBoost _playerBoost;

    private void Awake() {
        _playerVelocity = GetComponent<PlayerVelocity>();
        _playerBoost = GetComponent<PlayerBoost>();
    }

    void FixedUpdate() {
        var targetVelocity = new Vector3(Input.GetAxis("HorizontalMovement"), 0.0f, Input.GetAxis("VerticalMovement"));
        targetVelocity = Vector3.ClampMagnitude(targetVelocity, 1.0f);
        targetVelocity = transform.TransformDirection(targetVelocity);

        var speed = Mathf.Lerp(_runSpeed, _boostSpeed, _playerBoost.CurrentValue);

        targetVelocity *= speed * Time.fixedDeltaTime;

        _playerVelocity.SetInputVelocity(targetVelocity, Time.fixedDeltaTime);
    }

    public void Enable() {
        enabled = true;
    }

    public void Disable() {
        enabled = false;
    }
}
