using UnityEngine;

[RequireComponent(typeof(CameraArm))]
public class CameraFollowPosition : MonoBehaviour {
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
        InitializePosition();
        AddAnchorPositionChangedEvent();
    }

    private void OnDisable() {
        RemoveAnchorPositionChangedEvent();
    }
    #endregion

    #region Awake Methods
    private void InitializeCameraArm() {
        _arm = GetComponent<CameraArm>();
    }
    #endregion

    #region OnEnable Methods
    private void InitializeAnchor() {
        _anchor = _arm.GetAnchor();
    }

    private void InitializePosition() {
        transform.position = _anchor.GetDesiredPosition();
    }

    private void AddAnchorPositionChangedEvent() {
        _anchor.desiredPositionChanged.dynamicCalls += SetPosition;
    }
    #endregion

    #region OnDisable Methods
    private void RemoveAnchorPositionChangedEvent() {
        _anchor.desiredPositionChanged.dynamicCalls -= SetPosition;
    }
    #endregion

    private void SetPosition(Vector3 position) {
        transform.position = position;
    }

    private void OnAnchorChanged(CameraAnchor newAnchor) {
        _anchor.desiredPositionChanged.dynamicCalls -= SetPosition;
        _anchor = newAnchor;
        _anchor.desiredPositionChanged.dynamicCalls += SetPosition;
    }
}