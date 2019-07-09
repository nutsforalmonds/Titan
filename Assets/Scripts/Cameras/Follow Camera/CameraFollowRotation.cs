using UnityEngine;

[RequireComponent(typeof(CameraArm))]
public class CameraFollowRotation : MonoBehaviour {
    #region Local Variables
    private CameraArm _arm;
    private CameraAnchor _anchor;
    #endregion


    #region Monobehaviour Methods
    private void Awake() {
        InitializeCameraArm();
    }

    private void OnEnable() {
        InitializeAnchor();
        InitializeRotation();
        AddAnchorRotationChangedEvent();
    }

    private void OnDisable() {
        RemoveAnchorRotationChangedEvent();
    }
    #endregion

    #region Awake Methods
    private void InitializeCameraArm() {
        _arm = GetComponent<CameraArm>();
    }
    #endregion

    #region OnEnable Methods6
    private void InitializeAnchor() {
        _anchor = _arm.GetAnchor();
    }

    private void InitializeRotation() {
        transform.rotation = _anchor.GetDesiredRotation();
    }

    private void AddAnchorRotationChangedEvent() {
        _anchor.desiredRotationChanged.DynamicCalls += SetRotation;
    }
    #endregion

    #region OnDisable Methods
    private void RemoveAnchorRotationChangedEvent() {
        _anchor.desiredRotationChanged.DynamicCalls -= SetRotation;
    }
    #endregion

    private void SetRotation(Quaternion rotation) {
        transform.rotation = rotation;
    }

    private void OnAnchorChanged(CameraAnchor newAnchor) {
        _anchor.desiredRotationChanged.DynamicCalls -= SetRotation;
        _anchor = newAnchor;
        _anchor.desiredRotationChanged.DynamicCalls += SetRotation;
    }
}