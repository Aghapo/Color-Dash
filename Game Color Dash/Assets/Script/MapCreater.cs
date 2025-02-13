using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class TrackTemplate : MonoBehaviour { // MonoBehaviour olarak d�zeltildi
    public Transform startPoint;
    public Transform endPoint;
    public bool hasJunction;
}

public class MapCreater : MonoBehaviour {
    public GameObject[] trackPrefabs;
    public int trackCount = 3;  // Maksimum aktif ray say�s�
    public float generateDistance = 15f; // Oyuncuya 15 birim yakla�t���nda yeni ray olu�tur

    private List<GameObject> activeTracks = new List<GameObject>();
    private List<Transform> waypoints = new List<Transform>();
    private Transform playerTransform;
    private Transform lastEndPointTransform;

    void Start() {
        playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (playerTransform == null) {
            Debug.LogError("Oyuncu bulunamad�! L�tfen Player nesnesine 'Player' tag'� ekleyin.");
            return;
        }

        lastEndPointTransform = transform;

        // Ba�lang�� i�in birka� ray olu�tur
        for (int i = 0; i < trackCount; i++) {
            GenerateNextTrack();
        }
    }

    void Update() {
        if (playerTransform == null || lastEndPointTransform == null) return;

        // Yeni ray olu�turma kontrol�
        if (Vector3.Distance(playerTransform.position, lastEndPointTransform.position) < generateDistance) {
            GenerateNextTrack();
        }

        // Fazla raylar� temizle
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

                // Yeni waypoint olu�tur
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
