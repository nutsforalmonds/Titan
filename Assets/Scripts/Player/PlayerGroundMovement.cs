using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerVelocity), typeof(PlayerBoost))]
public class PlayerGroundMovement : MonoBehaviour {
    [SerializeField] float _runSpeed = 250.0f;
    [SerializeField] float _boostSpeed = 450.0f;
    [SerializeField] Transform _directionTransform;

    private PlayerVelocity _playerVelocity;
    private PlayerBoost _playerBoost;

    private void Awake() {
        _playerVelocity = GetComponent<PlayerVelocity>();
        _playerBoost = GetComponent<PlayerBoost>();
    }

    void FixedUpdate() {
        var y = Input.GetAxisRaw("FlightAscend") - Input.GetAxisRaw("FlightDescend");
        var targetVelocity = new Vector3(Input.GetAxis("HorizontalMovement"), y, Input.GetAxis("VerticalMovement"));
        targetVelocity = Vector3.ClampMagnitude(targetVelocity, 1.0f);
        targetVelocity = _directionTransform.TransformDirection(targetVelocity);
        targetVelocity *= _runSpeed * Time.fixedDeltaTime;

        _playerVelocity.SetInputVelocity(targetVelocity);
    }

    public void Enable() {
        enabled = true;
    }

    public void Disable() {
        enabled = false;
    }
}
