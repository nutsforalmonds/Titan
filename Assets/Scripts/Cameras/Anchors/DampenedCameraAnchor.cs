using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DampenedCameraAnchor : CameraAnchor {
    [SerializeField] float _positionDampening = 0.04f;
    [SerializeField] float _rotationDampening = 0.0f;

    protected override void SetDesiredPosition() {
        var velocity = Vector3.zero;
        _desiredPosition.position = Vector3.SmoothDamp(_desiredPosition.position, transform.position, ref velocity, _positionDampening);
        desiredPositionChanged.Invoke(_desiredPosition.position);
    }

    protected override void SetDesiredRotation() {
        var derivative = Quaternion.identity;
        _desiredRotation.rotation = QuaternionUtil.SmoothDamp(_desiredRotation.rotation, transform.rotation, ref derivative, _rotationDampening);
        desiredRotationChanged.Invoke(_desiredRotation.rotation);
    }
}
