using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour {

    [Range(0f, 2f)][SerializeField] float SphereVolume = 1f;

    
    private void OnDrawGizmos() {
        for (int i = 0; i < transform.childCount; i++) {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.GetChild(i).position, SphereVolume);
        }
        Gizmos.color = Color.red;
        for (int i = 0; i < transform.childCount - 1; i++) {
            Gizmos.DrawLine(transform.GetChild(i).position, transform.GetChild(i + 1).position);
        }
    }
    public Transform GetNextWaypoint(Transform CurrentWaypoint) {
        if (CurrentWaypoint == null) {
            return transform.GetChild(0);
        }
        if (CurrentWaypoint.GetSiblingIndex() < transform.childCount - 1) {
            return transform.GetChild(CurrentWaypoint.GetSiblingIndex() + 1);
        }
        else {
            return transform.GetChild(0);
        }
    }

}