using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacingDirection : MonoBehaviour {
    [SerializeField] float _slowTurnSpeed = 100.0f;
    [SerializeField] float _quickTurnSpeed = 800.0f;
    [SerializeField] float _angleForQuickTurn = 65.0f;

    private Quaternion _targetRotation;
    private float _currentSpeed;

    void Awake() {
        _targetRotation = transform.rotation;
    }

    void Update() {
        var angle = Quaternion.Angle(transform.rotation, _targetRotation);
        _currentSpeed = Mathf.Lerp(_slowTurnSpeed, _quickTurnSpeed, angle / _angleForQuickTurn);
        var step = _currentSpeed * Time.deltaTime;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, _targetRotation, step);
    }

    public void Rotate(Quaternion direction) {
        var directionAsEuler = direction.eulerAngles;
        _targetRotation = Quaternion.Euler(0.0f, directionAsEuler.y, 0.0f);
    }
}
