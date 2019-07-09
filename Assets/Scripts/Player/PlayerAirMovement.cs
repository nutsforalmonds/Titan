using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerVelocity), typeof(PlayerBoost))]
public class PlayerAirMovement : MonoBehaviour {
    [SerializeField] float _boostSpeed = 450.0f;
    [SerializeField] Transform _directionTransform;

    private PlayerVelocity _playerVelocity;
    private PlayerBoost _playerBoost;


    private void Awake() {
        _playerVelocity = GetComponent<PlayerVelocity>();
        _playerBoost = GetComponent<PlayerBoost>();
    }

    void Update() {
        if (_playerBoost.Boosting) {
            var targetYVelocity = Input.GetAxisRaw("FlightAscend") - Input.GetAxisRaw("FlightDescend");
            var targetVelocity = new Vector3(Input.GetAxis("HorizontalMovement"), targetYVelocity, Input.GetAxis("VerticalMovement"));
            targetVelocity = Vector3.ClampMagnitude(targetVelocity, 1.0f);
            targetVelocity = _directionTransform.TransformDirection(targetVelocity);

            targetVelocity *= _boostSpeed * Time.fixedDeltaTime;

            _playerVelocity.SetInputVelocity(targetVelocity);
        }
        // should maintain last used input velocity while not boosting
    }

    public void Enable() {
        enabled = true;
    }

    public void Disable() {
        enabled = false;
    }
}
