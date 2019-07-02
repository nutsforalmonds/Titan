using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Tethering : MonoBehaviour {
    [SerializeField] Transform _tetherPoint;
    [SerializeField] float _tetherLength;

    private Rigidbody _rigidbody;


    private void Awake() {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate() {
        if (Vector3.Distance(transform.position, _tetherPoint.position) > _tetherLength) {
            transform.position = (transform.position - _tetherPoint.position).normalized * _tetherLength;
        }
    }
}
