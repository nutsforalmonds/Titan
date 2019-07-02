using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpringJoint))]
public class RopeCollidePoint : MonoBehaviour {
    [SerializeField] float _reflectionDistance = 0.3f;
    [SerializeField] RopeCollidePoint _collidePartGO;
    [SerializeField] LayerMask _layersToIgnore;
    [SerializeField] float _minDistance = 0.2f;

    private SpringJoint _spring;
    private Rigidbody _objectToFree;
    private LineRenderer _lineRenderer;


    private bool _collided = false;
    private Vector3 _collideFreeDirection;
    private Vector3 _rayDirection;


    private float _timeToRescan = 0.15f;
    private float _currentTime = 0.0f;


    public bool final = false;


    #region Events
    [System.Serializable] public sealed class RopeCollidedEvent : UltEvents.UltEvent<Rigidbody> { }
    public RopeCollidedEvent collided;

    [System.Serializable] public sealed class RopeFreedEvent : UltEvents.UltEvent<RopeCollidePoint> { }
    public RopeFreedEvent freed;
    #endregion


    private void Awake() {
        _spring = GetComponent<SpringJoint>();
        _lineRenderer = GetComponent<LineRenderer>();
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + _collideFreeDirection);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + _rayDirection);

        if (_spring.connectedBody != null) {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, _spring.connectedBody.position);
        }

        if (_objectToFree != null) {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, _objectToFree.position);
        }
    }

    private void Update() {
        _lineRenderer.SetPosition(0, transform.position);
        _lineRenderer.SetPosition(1, _spring.connectedBody.position + _spring.connectedAnchor);
    }

    private void FixedUpdate() {
        if (_spring.connectedBody != null) {
            if (!_collided) {
                if (_spring.maxDistance < _minDistance) {
                    return;
                }
                if (Vector3.Distance(transform.position, _spring.connectedBody.transform.position) < _minDistance) {
                    return;
                }
                RaycastHit hit;
                if (Physics.Linecast(transform.position, _spring.connectedBody.transform.position, out hit, ~_layersToIgnore)) {
                    Collided(hit);
                }
            } else {
                _currentTime += Time.fixedDeltaTime;
                if (_currentTime > _timeToRescan) {
                    TryFreeObject();
                }
            }
        }
    }

    public void SetNext(Rigidbody next, float remainingRopeLength) {
        _spring.connectedBody = next;
        _spring.maxDistance = remainingRopeLength;
    }

    private void Collided(RaycastHit hit) {
        print(gameObject.name + " Collided: " + hit.collider.gameObject.name);

        _collided = true;
        var rayDirection = hit.point - transform.position;
        var normalUpVector = Vector3.Cross(rayDirection, hit.normal);
        _collideFreeDirection = Vector3.Cross(normalUpVector, rayDirection);

        var _collideDirectionReflected = Vector3.Reflect((hit.point - transform.position).normalized, hit.normal);
        var newPos = hit.point + _collideDirectionReflected * _reflectionDistance;
        var collidePoint = Instantiate(_collidePartGO, newPos, Quaternion.identity);

        collidePoint.collided.dynamicCalls += OnCollided;
        collidePoint.freed.dynamicCalls += OnFreed;

        var distance = Vector3.Distance(transform.position, collidePoint.transform.position);
        collidePoint.SetNext(_spring.connectedBody, _spring.maxDistance - distance);

        _objectToFree = _spring.connectedBody;
        var collideRB = collidePoint.GetComponent<Rigidbody>();
        SetNext(collideRB, distance);
        _currentTime = 0.0f;

        GetComponent<SpringJoint>().connectedAnchor = Vector3.zero;
        collidePoint.GetComponent<SpringJoint>().connectedAnchor = new Vector3(0.5f, 1, 0);

        collided.Invoke(collideRB);
    }

    private void Freed() {
        print(gameObject.name + " Freed: " + _spring.connectedBody.gameObject.name);
        if (_spring.connectedBody == null) {
            return;
        }

        var toFree = _spring.connectedBody.GetComponent<RopeCollidePoint>();
        if (toFree == null) {
            return;
        }
        if (!toFree.TryFreeObject()) {
            return;
        }

        var toFreeSpring = toFree.GetComponent<SpringJoint>();
        _spring.maxDistance += toFreeSpring.maxDistance;

        var newConnectedBody = toFreeSpring.connectedBody;
        _spring.connectedBody = newConnectedBody;

        _collided = false;
        toFree.collided.Clear();
        toFree.freed.Clear();

        GetComponent<SpringJoint>().connectedAnchor = new Vector3(0.5f, 1, 0);

        freed.Invoke(toFree);
        Destroy(toFree.gameObject);
    }


    private void OnCollided(Rigidbody point) {
        print(gameObject.name + " Collide Revieved VS: " + point.gameObject.name);
        _objectToFree = point;
    }

    private void OnFreed(RopeCollidePoint destroyed) {
        print(gameObject.name + " Freed Received VS: " + destroyed.gameObject.name);
        _objectToFree = destroyed._spring.connectedBody;
    }

    private bool TryFreeObject() {
        if (_objectToFree != null) {
            _rayDirection = _objectToFree.transform.position - transform.position;
            RaycastHit hit;
            if (!Physics.Linecast(transform.position, _objectToFree.transform.position, out hit, ~_layersToIgnore)) {
                if (Vector3.Dot(_rayDirection, _collideFreeDirection) >= 0.0f) {
                    Freed();
                    return true;
                }
            }
            return false;
        }
        return true;
    }

    public void Reel(float speed, float delta) {
        if (_spring.connectedBody != null) {
            var asPoint = _spring.connectedBody.GetComponent<RopeCollidePoint>();
            if (asPoint == null) {
                var distance = Vector3.Distance(transform.position, _spring.connectedBody.transform.position);
                if (distance + 0.2f < _spring.maxDistance) {
                    _spring.maxDistance = distance;
                }
                _spring.maxDistance -= speed * delta;
            } else {
                asPoint.Reel(speed, delta);
            }
            if (_spring.maxDistance < 0.2f) {
                Freed();
            }
        }
    }

    public void Destroy() {
        if (_spring.connectedBody != null) {
            var asPoint = _spring.connectedBody.GetComponent<RopeCollidePoint>();
            if (asPoint != null) {
                asPoint.Destroy();
            }
        }
        Destroy(gameObject);
    }
}
