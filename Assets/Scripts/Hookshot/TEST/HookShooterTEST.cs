using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookShooterTEST : MonoBehaviour {
    [SerializeField] Rigidbody _player;
    [SerializeField] HookshotHead _shotPrefab;

    [SerializeField] float _startingReelRate = 15.0f;
    [SerializeField] float _reelAcceleration = 6.0f;

    [SerializeField] AdvancedInput _fireInput;

    private HookshotHead _shot;
    private float _currentReelAcceleration = 0.0f;
    private float _currentReelSpeed = 0.0f;


    private void Awake() {
        var inputGO = GameObject.Instantiate(_fireInput);
        inputGO.transform.parent = transform;
        inputGO.tapped.DynamicCalls += OnFired;
    }

    private void Update() {
        if (Input.GetButton("Reel") && _shot != null) {
            _currentReelSpeed += _reelAcceleration * Time.deltaTime;
            _shot.Reel(_currentReelSpeed, Time.deltaTime);
        } else {
            _currentReelSpeed = _startingReelRate;
        }
    }

    private void OnFired() {
        if (_shot != null) {
            _shot.Destroy();
            _shot = null;
        } else {
            var go = Instantiate(_shotPrefab, transform.position, transform.rotation);
            go.GetComponent<SpringJoint>().connectedBody = _player;
            go.GetComponent<SpringJoint>().connectedAnchor = new Vector3(0.5f, 1, 0);
            go.GetComponent<Rigidbody>().AddForce(transform.forward * 4000.0f, ForceMode.Force);
            _shot = go;
        }
    }
}
