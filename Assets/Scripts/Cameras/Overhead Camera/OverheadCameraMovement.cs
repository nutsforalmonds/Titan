using UnityEngine;

public class OverheadCameraMovement : MonoBehaviour {
    #region SerializeFields
    [SerializeField] float _movementSpeed = 6.0f;
    #endregion

    #region Constants
    private const string HORIZONTAL_MOVEMENT_AXIS_NAME = "HorizontalMovement";
    private const string VERTICAL_MOVEMENT_AXIS_NAME = "VerticalMovement";
    #endregion

    #region Events
    [System.Serializable] public sealed class CameraMovedEvent : UltEvents.UltEvent<Vector3> { }
    public CameraMovedEvent cameraMoved;
    #endregion


    #region Monobehaviour Methods
    private void Update() {
        HandleInput();
    }
    #endregion

    #region Update Methods
    private void HandleInput() {
        HandleCameraMovement();
    }

    #region Camera Movement Methods
    private void HandleCameraMovement() {
        var input = GetMovementInput();
        if (MovementInputDetectied(input)) {
            MoveCamera(input);
            InvokeCameraMovedEvent();
        }
    }

    private Vector2 GetMovementInput() {
        return new Vector2(Input.GetAxis(HORIZONTAL_MOVEMENT_AXIS_NAME), Input.GetAxis(VERTICAL_MOVEMENT_AXIS_NAME));
    }

    private bool MovementInputDetectied(Vector2 input) {
        return AxisInputDetected(input.x) || AxisInputDetected(input.y);
    }

    private bool AxisInputDetected(float input) {
        return input != 0.0f;
    }

    private void MoveCamera(Vector2 input) {
        var movementVector = GetMovementVector(input);
        SetTransformPosition(movementVector);
    }

    private Vector3 GetMovementVector(Vector2 input) {
        var movementDirection = GetMovementDirection(input);
        var movementVector = ApplyMagnitude(movementDirection);
        return movementVector;
    }

    private Vector3 GetMovementDirection(Vector2 input) {
        Vector3 localDirection = GetLocalDirection(input);
        Vector3 globalDirection = LocalDirectionToGlobalDirection(localDirection);
        return globalDirection;
    }

    private Vector3 GetLocalDirection(Vector2 input) {
        return new Vector3(input.x, 0.0f, input.y);
    }

    private Vector3 LocalDirectionToGlobalDirection(Vector3 input) {
        return transform.rotation * input;
    }

    private Vector3 ApplyMagnitude(Vector3 globalDirection) {
        return globalDirection * _movementSpeed * Time.deltaTime;
    }

    private void SetTransformPosition(Vector3 movementVector) {
        transform.position += movementVector;
    }

    private void InvokeCameraMovedEvent() {
        cameraMoved.Invoke(transform.position);
    }
    #endregion
    #endregion
}
