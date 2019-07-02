using UnityEngine;

public class Print : MonoBehaviour {
    public void Log(RaycastHit hit) {
        Debug.Log(hit.transform.name);
    }
}
