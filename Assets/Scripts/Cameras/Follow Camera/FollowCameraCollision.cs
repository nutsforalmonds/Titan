using UnityEngine;

public class FollowCameraCollision : MonoBehaviour {
    #region SerializeFields
    [SerializeField] float _smoothTime = 0.02f;
    [SerializeField] float _startingMinDistanceToAnchor = 1.0f;
    [SerializeField] float _startingMaxDistanceToAnchor = 4.0f;
    [SerializeField] LayerMask _layersToIgnore = 0;
    #endregion

    #region Local Variables
    private Vector3 _dollyDirection;
    private LayerMask _toIgnore;

    private float _minDistanceToAnchor;
    private float _maxDistanceToAnchor;
    #endregion

    #region Constants
    private const float _DISTANCE_OFFSET = 0.9f;
    #endregion


    #region Monobehaviour Methods
    void Awake() {
        _toIgnore = ~_layersToIgnore;
        InitializeDollyDirection();
        InitializeDistancesToAnchor();
    }

    void LateUpdate() {
        HandleCameraCollision();
    }
    #endregion

    #region Awake Methods
    private void InitializeDollyDirection() {
        _dollyDirection = -Vector3.forward;
    }

    private void InitializeDistancesToAnchor() {
        _minDistanceToAnchor = _startingMinDistanceToAnchor;
        _maxDistanceToAnchor = _startingMaxDistanceToAnchor;
    }
    #endregion

    #region LateUpdate Methods
    private void HandleCameraCollision() {
        ModifyMaxDistanceByVerticalRotation();
        var desiredCameraPosition = GetDesiredCameraPosition();
        var realCameraPosition = GetRealCameraPosition(desiredCameraPosition);
        SetLocalPositionSmoothed(realCameraPosition);
    }

    private void ModifyMaxDistanceByVerticalRotation() {
        var horizontalForward = new Vector3(transform.forward.x, 0.0f, transform.forward.z);
        var verticalRotation = Vector3.Angle(transform.forward, horizontalForward);
        var interpolation = 1 - Mathf.Clamp01(verticalRotation / 50.0f);
        _maxDistanceToAnchor = Mathf.Lerp(_startingMinDistanceToAnchor, _startingMaxDistanceToAnchor, interpolation);
    }

    private Vector3 GetDesiredCameraPosition() {
        return transform.parent.TransformPoint(_dollyDirection * _maxDistanceToAnchor);
    }

    private Vector3 GetRealCameraPosition(Vector3 desiredCameraPosition) {
        float newDistanceToAnchor = GetDistanceToAnchor(desiredCameraPosition);
        return GetNewCameraPositionFromDistance(newDistanceToAnchor);
    }

    private float GetDistanceToAnchor(Vector3 desiredCameraPosition) {
        RaycastHit hit;
        if (IsObjectBlockingPathToCharacter(desiredCameraPosition, out hit)) {
            return GetBlockedDistanceToAnchor(hit);
        } else {
            return _maxDistanceToAnchor;
        }
    }

    private bool IsObjectBlockingPathToCharacter(Vector3 desiredCameraPosition, out RaycastHit hit) {
        return Physics.Linecast(transform.parent.position, desiredCameraPosition, out hit, _toIgnore, QueryTriggerInteraction.Ignore);
    }

    private float GetBlockedDistanceToAnchor(RaycastHit hit) {
        return Mathf.Clamp(hit.distance * _DISTANCE_OFFSET, _minDistanceToAnchor, _maxDistanceToAnchor);
    }

    private Vector3 GetNewCameraPositionFromDistance(float distanceToAnchor) {
        return _dollyDirection * distanceToAnchor;
    }

    private void SetLocalPositionSmoothed(Vector3 newCameraPosition) {
        var vel = Vector3.zero;
        transform.localPosition = Vector3.SmoothDamp(transform.localPosition, newCameraPosition, ref vel, _smoothTime);
    }
    #endregion
}
