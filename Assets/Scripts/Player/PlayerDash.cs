using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;
using DG.Tweening;

[RequireComponent(typeof(PlayerVelocity))]
public class PlayerDash : MonoBehaviour {
    [SerializeField] float _dashSpeed = 1200.0f;
    [SerializeField] float _dashDuration = 0.35f;
    [SerializeField] AdvancedInput _advancedInputGO = null;

    [SerializeField] DashInputDirections _directions = new DashInputDirections();

    private PlayerVelocity _playerVelocity;

    private void Awake() {
        _playerVelocity = GetComponent<PlayerVelocity>();
        foreach (var kvp in _directions) {
            var input = GameObject.Instantiate(_advancedInputGO);
            input.SetInputName(kvp.Key);
            var dash = new Dash();
            _playerVelocity.AddAdditionalVelocity(dash);
            input.doubleTapped.DynamicCalls += () => {
                var velocity = transform.TransformDirection(kvp.Value) * _dashSpeed;
                dash.StartDash(velocity, _dashDuration);
            };
        }
    }

    private class Dash : IVelocityAppliable {
        private Vector3 _previousVelocity;
        private Vector3 _currentVelocity;

        private float _dashDuration;

        public VelocityChangedEvent velocityChangedEvent = new VelocityChangedEvent();

        public Dash() { }


        public void StartDash(Vector3 dashVelocity, float dashDuration) {
            _previousVelocity = Vector3.zero;
            _currentVelocity = dashVelocity;
            _dashDuration = dashDuration;
            DOTween.To(() => _currentVelocity, v => _currentVelocity = v, Vector3.zero, dashDuration)
                   .SetEase(Ease.OutCubic)
                   .SetUpdate(UpdateType.Fixed)
                   .OnUpdate(() => VelocityChanged());
        }

        public void VelocityChanged() {
            if (_currentVelocity != _previousVelocity) {
                var newVelocity = (_currentVelocity - _previousVelocity) * Time.fixedDeltaTime;
                velocityChangedEvent.Invoke(newVelocity);
                _previousVelocity = _currentVelocity;
            }
        }

        public Vector3 CurrentVelocity() {
            return _currentVelocity;
        }

        public VelocityChangedEvent GetVelocityChangedEvent() {
            return velocityChangedEvent;
        }
    }

    [System.Serializable]
    private class DashInputDirections : SerializableDictionaryBase<string, Vector3> { }
}
