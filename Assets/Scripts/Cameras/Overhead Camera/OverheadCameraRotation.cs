using UnityEngine;

public class OverheadCameraRotation : MonoBehaviour {
    #region SerializeField Variables
    [SerializeField] float _inputSensitivity = 300.0f;
    [SerializeField] float _clampAngleTop = 65.0f;
    [SerializeField] float _clampAngleBottom = -35.0f;
    #endregion

    #region Local Variables
    private float _eulerRotationX = 0.0f;
    private float _eulerRotationY = 0.0f;
    #endregion

    #region Constants
    private const string ENABLE_MOUSE_ROTATION_BUTTON_NAME = "EnableMouseCameraRotation";
    private const string MOUSE_X_AXIS_NAME = "HorizontalLook";
    private const string MOUSE_Y_AXIS_NAME = "VerticalLook";
    #endregion

    #region Events
    [System.Serializable] public sealed class CameraRotatedEvent : UltEvents.UltEvent<Quaternion> { }
    public CameraRotatedEvent cameraRotated;
    #endregion


    #region Monobehaviour Methods
    private void OnEnable() {
        SetStartingEulerRotations();
    }

    void Update() {
        HandleInput();
    }
    #endregion

    #region OnEnable Methods
    private void SetStartingEulerRotations() {
        Vector3 startingEulerRotation = transform.localEulerAngles;
        _eulerRotationX = startingEulerRotation.x;
        _eulerRotationY = startingEulerRotation.y;
    }
    #endregion

    #region Update Methods
    private void HandleInput() {
        HandleCursorVisibility();
        HandleCameraRotation();
    }

    #region Cursor Visibility Methods
    private void HandleCursorVisibility() {
        HandleHideCursor();
        HandleShowCursor();
    }

    private void HandleHideCursor() {
        if (LookButtonDown()) {
            HideCursor();
        }
    }

    private bool LookButtonDown() {
        return Input.GetButtonDown(ENABLE_MOUSE_ROTATION_BUTTON_NAME);
    }

    private void HideCursor() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void HandleShowCursor() {
        if (LookButtonUp()) {
            ShowCursor();
        }
    }

    private bool LookButtonUp() {
        return Input.GetButtonUp(ENABLE_MOUSE_ROTATION_BUTTON_NAME);
    }

    private void ShowCursor() {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    #endregion

    #region Camera Rotation Methods
    private void HandleCameraRotation() {
        if (CameraRotationEnabled()) {
            RotateCamera();
        }
    }

    private bool CameraRotationEnabled() {
        return Input.GetButton(ENABLE_MOUSE_ROTATION_BUTTON_NAME);
    }

    private void RotateCamera() {
        var input = GetRotationInput();
        if (MouseInputDetected(input)) {
            RotateFromMouseInput(input);
            InvokeCameraRotatedEvent();
        }
    }

    private Vector2 GetRotationInput() {
        return new Vector2(Input.GetAxis(MOUSE_X_AXIS_NAME), Input.GetAxis(MOUSE_Y_AXIS_NAME));
    }

    private bool MouseInputDetected(Vector2 input) {
        return AxisInputDetected(input.x) || AxisInputDetected(input.y);
    }

    private bool AxisInputDetected(float direction) {
        return direction != 0.0f;
    }

    private void RotateFromMouseInput(Vector2 input) {
        SetLocalRotationValues(input);
        SetTransformRotation();
    }

    private void SetLocalRotationValues(Vector2 input) {
        SetRotationValuesFromInput(input);
        ClampXRotation();
    }

    private void SetRotationValuesFromInput(Vector2 input) {
        _eulerRotationX += GetModifiedInput(input.y);
        _eulerRotationY += GetModifiedInput(input.x);
    }

    private float GetModifiedInput(float rawInput) {
        return rawInput * _inputSensitivity * Time.deltaTime;
    }

    private void ClampXRotation() {
        _eulerRotationX = Mathf.Clamp(_eulerRotationX, _clampAngleBottom, _clampAngleTop);
    }

    private void SetTransformRotation() {
        var localRotation = Quaternion.Euler(_eulerRotationX, _eulerRotationY, 0.0f);
        transform.rotation = localRotation;
    }

    private void InvokeCameraRotatedEvent() {
        cameraRotated.Invoke(transform.rotation);
    }
    #endregion
    #endregion
}
