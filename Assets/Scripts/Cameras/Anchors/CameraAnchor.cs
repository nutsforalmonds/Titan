using UnityEngine;

public class CameraAnchor : MonoBehaviour {
    #region Events
    [System.Serializable] public sealed class DesiredPositionChangedEvent : UltEvents.UltEvent<Vector3> { }
    public DesiredPositionChangedEvent desiredPositionChanged;

    [System.Serializable] public sealed class DesiredRotationChangedEvent : UltEvents.UltEvent<Quaternion> { }
    public DesiredRotationChangedEvent desiredRotationChanged;
    #endregion

    [SerializeField] protected Transform _desiredPosition;
    [SerializeField] protected Transform _desiredRotation;

    protected Vector3 _previousPosition;
    protected Quaternion _previousRotation;

    protected virtual void Start() {
        _desiredPosition.transform.parent = null;
        _desiredRotation.transform.parent = null;
    }

    protected virtual void FixedUpdate() {
        SetDesiredRotation();
        SetDesiredPosition();
    }


    public Vector3 GetDesiredPosition() {
        return _desiredPosition.position;
    }

    public Quaternion GetDesiredRotation() {
        return _desiredRotation.rotation;
    }

    protected virtual void SetDesiredPosition() {
        if (_desiredPosition.position != transform.position) {
            _desiredPosition.position = transform.position;
            desiredPositionChanged.Invoke(_desiredPosition.position);
        }
    }

    protected virtual void SetDesiredRotation() {
        if (_desiredRotation.rotation != transform.rotation) {
            _desiredRotation.rotation = transform.rotation;
            desiredRotationChanged.Invoke(_desiredRotation.rotation);
        }
    }
}
