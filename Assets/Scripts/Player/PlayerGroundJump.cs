using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerGroundJump : MonoBehaviour {
    [SerializeField] float _jumpHeightVelocity = 5.0f;
    [SerializeField] float _jumpForwardBoostVelocity = 1.5f;

    private Rigidbody _rigidbody;

    public UltEvents.UltEvent jumped;


    private void Awake() {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update() {
        if (Input.GetButtonDown("Jump")) {
            var force = Vector3.up * _jumpHeightVelocity;
            var sprintInput = Input.GetAxis("Sprint");
            if (sprintInput != 0.0f) {
                force += sprintInput * _rigidbody.velocity.normalized * _jumpForwardBoostVelocity;
            }
            _rigidbody.velocity += force;
            jumped.Invoke();
        }
    }

    public void Enable() {
        enabled = true;
    }

    public void Disable() {
        enabled = false;
    }
}
