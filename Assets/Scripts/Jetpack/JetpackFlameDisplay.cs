using System.Collections.Generic;
using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;

public class JetpackFlameDisplay : MonoBehaviour {
    public enum FlameDirection { Forward, Up, Left, Right }
    [SerializeField] JetpackFlameDisplayDictionary _flameModels;

    private List<FlameDirection> _currentDirections = new List<FlameDirection>();

    private void Update() {
        if (Input.GetButton("Boost")) {
            var directions = GetDirections();
            foreach (var direction in _currentDirections) {
                _flameModels[direction].SetActive(false);
            }

            _currentDirections = directions;
            foreach (var direction in _currentDirections) {
                _flameModels[direction].SetActive(true);
            }
        } else if (_currentDirections.Count > 0) {
            foreach (var direction in _currentDirections) {
                _flameModels[direction].SetActive(false);
            }
            _currentDirections.Clear();
        }
    }

    private List<FlameDirection> GetDirections() {
        var input = new Vector3(Input.GetAxis("HorizontalMovement"), Input.GetAxis("Jump"), Input.GetAxis("VerticalMovement"));
        var directions = new List<FlameDirection>();
        if (input.x < 0) {
            directions.Add(FlameDirection.Left);
        } else if (input.x > 0) {
            directions.Add(FlameDirection.Right);
        }
        if (input.y > 0) {
            directions.Add(FlameDirection.Up);
        }
        if (input.z > 0) {
            directions.Add(FlameDirection.Forward);
        }
        return directions;
    }

    [System.Serializable]
    public class JetpackFlameDisplayDictionary : SerializableDictionaryBase<FlameDirection, GameObject> { }
}
