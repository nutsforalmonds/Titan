using UnityEngine;

public class AdvancedInput : MonoBehaviour {
    [SerializeField] string _inputName = "";

    [SerializeField] float _heldCooldownDuration = 0.0f;
    [SerializeField] float _singleTapCooldownDuration = 0.0f;
    [SerializeField] float _doubleTapDuration = 0.0f;
    [SerializeField] float _doubleTapCooldownDuration = 0.0f;


    private bool _held = false;
    public bool Held { get { return _held; } }

    private bool _heldOnCooldown = false;
    private float _elapsedHeldCooldownTime = 0.0f;

    private bool _singleTapOnCooldown = false;
    private float _elapsedSingleTapCooldownTime = 0.0f;

    private bool _checkDoubleTap = false;
    private float _elapsedDoubleTapCheckTime = 0.0f;

    private bool _doubleTapOnCooldown = false;
    private float _elapsedDoubleTapCooldownTime = 0.0f;

    public UltEvents.UltEvent tapped;
    public UltEvents.UltEvent doubleTapped;
    public UltEvents.UltEvent heldStarted;
    public UltEvents.UltEvent heldReleased;


    private void Update() {
        if (SingleTapCooldownTimerExceeded()) {
            DisableSingleTapCooldown();
        }
        if (DoubleTapTimerExceeded()) {
            DisableCheckDoubleTap();
        }
        if (DoubleTapCooldownTimerExceeded()) {
            DisableDoubleTapCooldown();
        }

        if (_heldOnCooldown) {
            _elapsedHeldCooldownTime += Time.deltaTime;
            if (_elapsedHeldCooldownTime >= _heldCooldownDuration) {
                _heldOnCooldown = false;
            }
        } else {
            if (Input.GetButtonDown(_inputName)) {
                _held = true;
                heldStarted.Invoke();
            } else if (Input.GetButtonUp(_inputName)) {
                _held = false;
                heldReleased.Invoke();
            }
        }

        if (_checkDoubleTap) {
            _elapsedDoubleTapCheckTime += Time.deltaTime;
            if (InputPressed()) {
                DisableCheckDoubleTap();
                EnableDoubleTapCooldown();
                doubleTapped.Invoke();
            }
        } else if (_doubleTapOnCooldown) {
            _elapsedDoubleTapCooldownTime += Time.deltaTime;
        } else if (InputPressed() && !_singleTapOnCooldown) {
            EnableSingleTapCooldown();
            EnableCheckDoubleTap();
            tapped.Invoke();
        }
        if (_singleTapOnCooldown) {
            _elapsedSingleTapCooldownTime += Time.deltaTime;
        }
    }

    public void Enable() {
        enabled = true;
        _held = Input.GetButton(_inputName);
    }

    public void Disable() {
        _held = false;
        enabled = false;
    }

    public void Restart() {
        _heldOnCooldown = false;
        _elapsedHeldCooldownTime = 0.0f;

        _singleTapOnCooldown = false;
        _elapsedSingleTapCooldownTime = 0.0f;

        _checkDoubleTap = false;
        _doubleTapOnCooldown = false;
        _elapsedDoubleTapCooldownTime = 0.0f;
        _elapsedDoubleTapCheckTime = 0.0f;
    }

    public void StartCooldowns() {
        _heldOnCooldown = true;
        _elapsedHeldCooldownTime = 0.0f;

        _singleTapOnCooldown = true;
        _elapsedSingleTapCooldownTime = 0.0f;

        _checkDoubleTap = false;
        _doubleTapOnCooldown = true;
        _elapsedDoubleTapCooldownTime = 0.0f;
        _elapsedDoubleTapCheckTime = 0.0f;
    }

    private bool InputPressed() {
        return Input.GetButtonDown(_inputName);
    }

    private bool SingleTapCooldownTimerExceeded() {
        return _singleTapOnCooldown && _elapsedSingleTapCooldownTime > _singleTapCooldownDuration;
    }

    private void DisableSingleTapCooldown() {
        _singleTapOnCooldown = false;
        _elapsedSingleTapCooldownTime = 0.0f;
    }

    private bool DoubleTapTimerExceeded() {
        return _checkDoubleTap && _elapsedDoubleTapCheckTime > _doubleTapDuration;
    }

    private void DisableCheckDoubleTap() {
        _checkDoubleTap = false;
        _elapsedDoubleTapCheckTime = 0.0f;
    }

    private bool DoubleTapCooldownTimerExceeded() {
        return _doubleTapOnCooldown && _elapsedDoubleTapCooldownTime > _doubleTapCooldownDuration;
    }

    private void DisableDoubleTapCooldown() {
        _doubleTapOnCooldown = false;
        _elapsedDoubleTapCooldownTime = 0.0f;
    }

    private void EnableDoubleTapCooldown() {
        _doubleTapOnCooldown = true;
        _elapsedDoubleTapCooldownTime = 0.0f;
    }

    private void EnableCheckDoubleTap() {
        _checkDoubleTap = true;
        _elapsedDoubleTapCheckTime = 0.0f;
    }

    private void EnableSingleTapCooldown() {
        _singleTapOnCooldown = true;
        _elapsedSingleTapCooldownTime = 0.0f;
    }
}