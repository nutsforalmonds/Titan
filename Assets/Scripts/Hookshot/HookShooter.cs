using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookShooter : MonoBehaviour {
    [SerializeField] Rigidbody _player = null;
    [SerializeField] HookshotHead _shotPrefab = null;

    [SerializeField] float _startingReelRate = 15.0f;
    [SerializeField] float _reelAcceleration = 6.0f;

    [SerializeField] AdvancedInput _fireInputGO = null;
    [SerializeField] AdvancedInput _reelInputGO = null;
    [SerializeField] AdvancedInput _releaseInputGO = null;


    private AdvancedInput _fireInput;
    private AdvancedInput _reelInput;
    private AdvancedInput _releaseInput;

    private HookshotHead _shot;
    private float _currentReelSpeed = 0.0f;

    private bool _canFire = true;
    private bool _canReel = false;
    private bool _canRelease = false;

    private void Awake() {
        _fireInput = GameObject.Instantiate(_fireInputGO);
        _fireInput.transform.parent = transform;
        _fireInput.tapped.DynamicCalls += OnFireDown;
        _fireInput.heldReleased.DynamicCalls += OnFireReleased;

        _reelInput = GameObject.Instantiate(_reelInputGO);
        _reelInput.transform.parent = transform;

        _releaseInput = GameObject.Instantiate(_releaseInputGO);
        _releaseInput.transform.parent = transform;
        _releaseInput.tapped.DynamicCalls += OnReleased;
    }

    private void Update() {
        if (_canReel && _reelInput.Held && _shot != null) {
            if (_shot.Attached) {
                _currentReelSpeed += _reelAcceleration * Time.deltaTime;
                _shot.Reel(_currentReelSpeed, Time.deltaTime);
            } else {
                DestroyShot();
            }
        } else {
            _currentReelSpeed = _startingReelRate;
        }
    }

    private void DestroyShot() {
        if (_shot == null) {
            return;
        }

        _shot.Destroy();
        _shot = null;

        _canFire = true;
        _canReel = false;
        _canRelease = false;
    }

    private void OnFireDown() {
        if (!_canFire) {
            return;
        }

        var go = Instantiate(_shotPrefab, transform.position, transform.rotation);
        go.GetComponent<SpringJoint>().connectedBody = _player;
        go.GetComponent<SpringJoint>().connectedAnchor = new Vector3(0.5f, 1, 0);
        go.GetComponent<Rigidbody>().AddForce(transform.forward * 4000.0f, ForceMode.Force);
        _shot = go;

        _canReel = false;
        _canRelease = false;

        _canFire = false;
    }

    private void OnFireReleased() {
        if (_canFire) {
            return;
        }
        _canReel = true;
        _canRelease = true;
    }

    private void OnReleased() {
        if (!_canRelease) {
            return;
        }
        DestroyShot();
    }
}
