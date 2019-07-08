using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerVelocity), typeof(PlayerBoost))]
public class PlayerAirMovement : MonoBehaviour {
    [SerializeField] float _sprintSpeed = 9.0f;

    private PlayerVelocity _playerVelocity;
    private PlayerBoost _playerBoost;


    private void Awake() {
        _playerVelocity = GetComponent<PlayerVelocity>();
        _playerBoost = GetComponent<PlayerBoost>();
    }

    void Update() {
        // Not yet implemented
    }

    public void Enable() {
        enabled = true;
    }

    public void Disable() {
        enabled = false;
    }
}
