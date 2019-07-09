using UnityEngine;

// Based on Board To Bits Games code - https://www.youtube.com/watch?v=7KiK0Aqtmzc

[RequireComponent(typeof(PlayerVelocity), typeof(PlayerBoost))]
public class PlayerJumpEnhancement : MonoBehaviour {
    [SerializeField] protected float _fallMultiplier = 2.5f;
    [SerializeField] protected float _lowJumpMultiplier = 2f;

    private PlayerVelocity _playerVelocity;
    protected PlayerBoost _playerBoost;


    private bool _apexReached = false;
    public UltEvents.UltEvent apexReached;

    protected virtual void Awake() {
        _playerVelocity = GetComponent<PlayerVelocity>();
        _playerBoost = GetComponent<PlayerBoost>();
    }

    private void Update() {
        if (_playerVelocity.Velocity.y < 0) {
            if (!_apexReached) {
                _apexReached = true;
                apexReached.Invoke();
            }
            _playerVelocity.AddForcedVelocity(Vector3.up * Physics.gravity.y * (_fallMultiplier - 1) * Time.deltaTime);
        } else if (_playerVelocity.Velocity.y > 0 && !Input.GetButton("Jump")) {
            _playerVelocity.AddForcedVelocity(Vector3.up * Physics.gravity.y * (_lowJumpMultiplier - 1) * Time.deltaTime);
        }
    }

    public void Enable() {
        enabled = true;
        _apexReached = false;
    }

    public void Disable() {
        enabled = false;
    }
}
