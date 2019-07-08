using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerVelocity : MonoBehaviour {
    private Vector3 _inputVelocity = Vector3.zero;
    public Vector3 InputVelocity { get { return _inputVelocity; } }

    private Vector3 __forcedVelocity; // for display in the inspector. should be removed in release code
    public Vector3 ForcedVelocity { get { return _rigidbody.velocity - _inputVelocity; } }

    public Vector3 Velocity { get { return _rigidbody.velocity; } }


    private Rigidbody _rigidbody;


    private void Awake() {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.freezeRotation = true;
    }

    private void Update() {
        __forcedVelocity = ForcedVelocity; // for display in the inspector. Remove for release
        if (Input.GetKeyDown(KeyCode.Q)) {
            _rigidbody.velocity += new Vector3(0, 0, 20);
        }
    }

    public void SetInputVelocity(Vector3 newInputVelocity) {
        _rigidbody.velocity -= _inputVelocity;
        _inputVelocity = newInputVelocity;
        _rigidbody.velocity += _inputVelocity;
        if (_rigidbody.velocity.sqrMagnitude < _inputVelocity.sqrMagnitude) {
            _rigidbody.velocity = _inputVelocity;
        }
    }

    public void AddForcedVelocity(Vector3 modifier) {
        _rigidbody.velocity += modifier;
    }

    public void Enable() {
        enabled = true;
    }

    public void Disable() {
        enabled = false;
    }
}
