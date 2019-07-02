using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerJumpBoost : MonoBehaviour {
    [SerializeField] float _jumpBoostVelocity = 10.0f;
    [SerializeField] AdvancedInput _jumpInput;

    private Rigidbody _rigidbody;

    private bool _enabled = true;

    public UltEvents.UltEvent boosted;


    private void Awake() {
        _rigidbody = GetComponent<Rigidbody>();

        var inputGO = GameObject.Instantiate(_jumpInput);
        inputGO.transform.parent = transform;
        inputGO.doubleTapped.dynamicCalls += OnDoubleTapped;
    }

    private void OnDoubleTapped() {
        if (!_enabled) {
            return;
        }
        ApplyBoostForce();
        boosted.Invoke();
    }

    private void ApplyBoostForce() {
        var targetVelocity = _rigidbody.velocity;
        targetVelocity.y = _jumpBoostVelocity;
        _rigidbody.velocity = targetVelocity;
    }

    public void Enable() {
        _enabled = true;
        _jumpInput.Restart();
    }

    public void Disable() {
        _enabled = false;
    }

    public void Restart() {
        _jumpInput.Restart();
    }
}
