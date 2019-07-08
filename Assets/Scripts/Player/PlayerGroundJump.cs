using UnityEngine;

[RequireComponent(typeof(PlayerVelocity), typeof(PlayerBoost))]
public class PlayerGroundJump : MonoBehaviour {
    [SerializeField] float _jumpHeightVelocity = 5.0f;

    private PlayerVelocity _playerVelocity;
    private PlayerBoost _playerBoost;

    public UltEvents.UltEvent jumped;


    private void Awake() {
        _playerVelocity = GetComponent<PlayerVelocity>();
        _playerBoost = GetComponent<PlayerBoost>();
    }

    private void Update() {
        if (Input.GetButtonDown("Jump")) {
            var force = Vector3.up * _jumpHeightVelocity;
            _playerVelocity.AddForcedVelocity(force);
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