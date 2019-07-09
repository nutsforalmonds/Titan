using UnityEngine;

public interface IVelocityAppliable {
    VelocityChangedEvent GetVelocityChangedEvent();

    Vector3 CurrentVelocity();
}

public class VelocityChangedEvent : UltEvents.UltEvent<Vector3> { }
