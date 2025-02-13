using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class TrackTemplate : MonoBehaviour { // MonoBehaviour olarak düzeltildi
    public Transform startPoint;
    public Transform endPoint;
    public bool hasJunction;
}

public class MapCreater : MonoBehaviour {
    public GameObject[] trackPrefabs;
    public int trackCount = 3;  // Maksimum aktif ray sayýsý
    public float generateDistance = 15f; // Oyuncuya 15 birim yaklaþtýðýnda yeni ray oluþtur

    private List<GameObject> activeTracks = new List<GameObject>();
    private List<Transform> waypoints = new List<Transform>();
    private Transform playerTransform;
    private Transform lastEndPointTransform;

    void Start() {
        playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (playerTransform == null) {
            Debug.LogError("Oyuncu bulunamadý! Lütfen Player nesnesine 'Player' tag'ý ekleyin.");
            return;
        }

        lastEndPointTransform = transform;

        // Baþlangýç için birkaç ray oluþtur
        for (int i = 0; i < trackCount; i++) {
            GenerateNextTrack();
        }
    }

    void Update() {
        if (playerTransform == null || lastEndPointTransform == null) return;

        // Yeni ray oluþturma kontrolü
        if (Vector3.Distance(playerTransform.position, lastEndPointTransform.position) < generateDistance) {
            GenerateNextTrack();
        }

        // Fazla raylarý temizle
        CleanUpTracks();
    }

    void GenerateNextTrack() {
        if (trackPrefabs.Length == 0) return;

        int randomIndex = Random.Range(0, trackPrefabs.Length);
        GameObject trackPrefab = trackPrefabs[randomIndex];

        if (trackPrefab != null) {
            Vector3 spawnPosition = lastEndPointTransform.position;
            Quaternion spawnRotation = lastEndPointTransform.rotation;

            GameObject newTrack = Instantiate(trackPrefab, spawnPosition, spawnRotation);
            activeTracks.Add(newTrack);

            TrackTemplate template = newTrack.GetComponent<TrackTemplate>();
            if (template != null && template.endPoint != null) {
                lastEndPointTransform = template.endPoint;

                // Yeni waypoint oluþtur
                GameObject waypointObj = new GameObject($"Waypoint_{waypoints.Count}");
                waypointObj.transform.position = template.endPoint.position;
                waypointObj.transform.rotation = template.endPoint.rotation;
                waypoints.Add(waypointObj.transform);
            }
        }
    }

    void CleanUpTracks() {
        while (activeTracks.Count > trackCount) {
            if (activeTracks[0] != null) {
                Destroy(activeTracks[0]);
            }
            activeTracks.RemoveAt(0);
        }

        while (waypoints.Count > trackCount) {
            if (waypoints[0] != null) {
                Destroy(waypoints[0].gameObject);
            }
            waypoints.RemoveAt(0);
        }
    }

    public Transform[] GetWaypoints() {
        return waypoints.ToArray();
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        if (lastEndPointTransform != null) {
            Gizmos.DrawWireSphere(lastEndPointTransform.position, 0.5f);
            Gizmos.DrawLine(transform.position, lastEndPointTransform.position);
        }
    }
}
