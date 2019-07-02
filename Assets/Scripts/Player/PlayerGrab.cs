using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerGrab : MonoBehaviour {
    [SerializeField] float _grabRadius = 0.6f;
    [SerializeField] LayerMask _grabbableLayer = 0;

    private bool _canGrab = true;
    private bool _grabbed = false;
    private Rigidbody _rigidbody;


    #region Events
    [System.Serializable] public sealed class GrabbedEvent : UltEvents.UltEvent<Collider> { }
    public GrabbedEvent grabbed;

    public UltEvents.UltEvent released;
    #endregion

    private void Update() {
        if (_grabbed) {
            if (Input.GetButtonDown("Release")) {
                _grabbed = false;
                released.Invoke();
            }
        } else if (_canGrab) {
            if (Input.GetButton("Grab")) {
                Collider grabbedCollider;
                if (CanGrab(out grabbedCollider)) {
                    _grabbed = true;
                    _canGrab = false;
                    grabbed.Invoke(grabbedCollider);
                }
            }
        } else if (!Input.GetButton("Release")) {
            _canGrab = true;
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _grabRadius);
    }


    private bool CanGrab(out Collider overlap) {
        Collider[] overlaps = new Collider[4];
        if (Physics.OverlapSphereNonAlloc(transform.position, _grabRadius, overlaps, _grabbableLayer, QueryTriggerInteraction.Collide) > 0) {
            overlap = overlaps[1];
            return true;
        }
        overlap = null;
        return false;
    }
}
