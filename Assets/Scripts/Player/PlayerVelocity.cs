using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerVelocity : MonoBehaviour {
    private Vector3 _inputVelocity = Vector3.zero;
    public Vector3 InputVelocity { get { return _inputVelocity; } }

    public Vector3 ForcedVelocity { get { return _rigidbody.velocity - _inputVelocity; } }

    public Vector3 Velocity { get { return _rigidbody.velocity; } }


    private Rigidbody _rigidbody;


    private void Awake() {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.freezeRotation = true;
    }

    public void SetInputVelocity(Vector3 newInputVelocity) {
        _rigidbody.velocity -= _inputVelocity;
        _inputVelocity = newInputVelocity;
        _rigidbody.velocity += _inputVelocity;
    }

    public void ModifyForcedVelocity(Vector3 modifier) {
        _rigidbody.velocity += modifier;
    }

    public void Enable() {
        enabled = true;
    }

    public void Disable() {
        enabled = false;
    }
}
