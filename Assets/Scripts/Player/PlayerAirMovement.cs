using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerVelocity), typeof(PlayerBoost))]
public class PlayerAirMovement : MonoBehaviour {
    [SerializeField] float _boostSpeed = 450.0f;
    [SerializeField] Transform _directionTransform;

    private Vector3 _liftoffVelocity;

    private PlayerVelocity _playerVelocity;
    private PlayerBoost _playerBoost;


    private void Awake() {
        _playerVelocity = GetComponent<PlayerVelocity>();
        _playerBoost = GetComponent<PlayerBoost>();
    }

    void Update() {
        if (Input.GetButtonDown("Sprint")) {
            var targetYVelocity = Input.GetAxisRaw("FlightAscend") - Input.GetAxisRaw("FlightDescend");
            var targetVelocity = new Vector3(Input.GetAxis("HorizontalMovement"), targetYVelocity, Input.GetAxis("VerticalMovement"));
            targetVelocity = Vector3.ClampMagnitude(targetVelocity, 1.0f);
            targetVelocity = _directionTransform.TransformDirection(targetVelocity);

            targetVelocity *= _boostSpeed * Time.fixedDeltaTime;

            _playerVelocity.SetInputVelocity(targetVelocity);
        } else {
            _playerVelocity.SetInputVelocity(Vector3.zero);
        }
    }

    public void Enable() {
        enabled = true;
        _liftoffVelocity = _playerVelocity.Velocity;
    }

    public void Disable() {
        enabled = false;
    }
}
