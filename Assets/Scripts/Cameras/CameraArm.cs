using UnityEngine;

public class CameraArm : MonoBehaviour {
    #region SerializeFields
    [SerializeField] CameraAnchor _startingAnchor = null;
    #endregion

    #region Local Variables
    private CameraAnchor _anchor;
    #endregion

    #region Events
    [System.Serializable] public sealed class CameraAnchorChangedEvent : UltEvents.UltEvent<CameraAnchor> { }
    public CameraAnchorChangedEvent cameraAnchorChangedEvent;
    #endregion


    #region Monobehaviours
    private void Awake() {
        _anchor = _startingAnchor;
    }
    #endregion


    #region Getters and Setters
    public CameraAnchor GetAnchor() {
        return _anchor;
    }

    public void SetCameraAnchor(CameraAnchor anchor) {
        _anchor = anchor;
        cameraAnchorChangedEvent.Invoke(_anchor);
    }
    #endregion
}
