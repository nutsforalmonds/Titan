using UnityEngine;

public class GroundedTester : MonoBehaviour {
    [SerializeField] Vector3 _offset = Vector3.zero;
    [SerializeField] float _radius = 0.25f;
    [SerializeField] LayerMask _layersToIgnore;

    private bool _grounded;


    #region Events
    public UltEvents.UltEvent grounded;
    public UltEvents.UltEvent leftGround;
    #endregion


    private void Awake() {
        _layersToIgnore = ~_layersToIgnore;
        InitialTest();
    }

    private void FixedUpdate() {
        var newGrounded = IsGrounded();
        if (_grounded != newGrounded) {
            _grounded = newGrounded;
            InvokeEvents();
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + _offset, _radius);
    }

    public bool Grounded {
        get {
            return _grounded;
        }
    }

    private bool IsGrounded() {
        Collider[] overlaps = new Collider[4];
        return Physics.OverlapSphereNonAlloc(transform.position + _offset, _radius, overlaps, _layersToIgnore, QueryTriggerInteraction.Ignore) > 0;
    }

    private void InitialTest() {
        var newGrounded = IsGrounded();
        _grounded = newGrounded;
        InvokeEvents();
    }

    private void InvokeEvents() {
        if (_grounded) {
            grounded.Invoke();
        } else {
            leftGround.Invoke();
        }
    }

    private void Enable() {
        enabled = true;
        InitialTest();
    }

    private void Disable() {
        enabled = false;
    }
}
