using UnityEngine;

[RequireComponent(typeof(PlayerVelocity), typeof(PlayerBoost))]
public class PlayerJump : MonoBehaviour {
    [SerializeField] float _jumpHeightVelocity = 5.0f;

    private PlayerBoost _playerBoost;
    private PlayerVelocity _playerVelocity;

    public UltEvents.UltEvent jumpedRunning;
    public UltEvents.UltEvent jumpedBoosting;


    private void Awake() {
        _playerBoost = GetComponent<PlayerBoost>();
        _playerVelocity = GetComponent<PlayerVelocity>();
    }

    private void Update() {
        if (Input.GetButtonDown("Jump")) {
            var force = Vector3.up * _jumpHeightVelocity;
            _playerVelocity.AddForcedVelocity(force);
            if (_playerBoost.Boosting) {
                jumpedBoosting.Invoke();
            } else {
                jumpedRunning.Invoke();
            }
        }
    }

    public void Enable() {
        enabled = true;
    }

    public void Disable() {
        enabled = false;
    }
}