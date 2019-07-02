using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerGroundJump : MonoBehaviour {
    [SerializeField] float _jumpHeightVelocity = 5.0f;
    [SerializeField] float _jumpForwardBoostVelocity = 1.5f;

    private bool _sprinting = false;

    private Rigidbody _rigidbody;

    public UltEvents.UltEvent jumped;


    private void Awake() {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update() {
        if (Input.GetButtonDown("Jump")) {
            var force = Vector3.up * _jumpHeightVelocity;
            if (_sprinting) {
                force += _rigidbody.velocity.normalized * _jumpForwardBoostVelocity;
            }
            _rigidbody.velocity += force;
            jumped.Invoke();
        }
    }

    private void OnSprintStarted() {
        _sprinting = true;
    }

    private void OnSprintEnded() {
        _sprinting = false;
    }

    public void Enable() {
        enabled = true;
    }

    public void Disable() {
        enabled = false;
    }
}
