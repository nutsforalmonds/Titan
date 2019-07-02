using UnityEngine;

public class ClickDetection : MonoBehaviour {
    #region SerializeFields
    [SerializeField] Camera _camera = null;
    #endregion

    #region Local Variables
    private int _layerMask;
    #endregion

    #region Constants
    private const string CLICK_BUTTON_NAME = "Select";
    #endregion

    #region Events
    [System.Serializable] public sealed class ClickDetectedEvent : UltEvents.UltEvent<RaycastHit> { }
    public ClickDetectedEvent clickDetected;
    #endregion


    #region Monobehaviours
    void Awake() {
        SetLayerMask();
    }

    void Update() {
        HandleInput();
    }
    #endregion

    #region Awake Methods
    private void SetLayerMask() {
        _layerMask = 1 << LayerMask.NameToLayer("Ground");
        _layerMask |= 1 << LayerMask.NameToLayer("Character");
        _layerMask |= 1 << LayerMask.NameToLayer("Titan");
    }
    #endregion

    #region Update Methods
    private void HandleInput() {
        HandleSelectDetection();
    }

    private void HandleSelectDetection() {
        if (SelectInputDetected()) {
            DetectClickHit();
        }
    }

    private bool SelectInputDetected() {
        return Input.GetButtonDown(CLICK_BUTTON_NAME);
    }

    private void DetectClickHit() {
        RaycastHit hit;
        Ray ray = GetRayToMouse();
        if (SelectionDetected(ray, out hit)) {
            InvokeClickDetectedEvent(hit);
        }
    }

    private Ray GetRayToMouse() {
        return _camera.ScreenPointToRay(Input.mousePosition);
    }

    private bool SelectionDetected(Ray ray, out RaycastHit hit) {
        return Physics.Raycast(ray, out hit, Mathf.Infinity, _layerMask);
    }

    private void InvokeClickDetectedEvent(RaycastHit hit) {
        clickDetected.Invoke(hit);
    }
    #endregion
}