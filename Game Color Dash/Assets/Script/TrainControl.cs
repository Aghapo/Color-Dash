using System.Collections.Generic;
using UnityEngine;

public class TrainControl : MonoBehaviour {
    public List<Transform> Waypoints = new List<Transform>();

    public float maxSpeed = 5f;  // Maksimum hýz
    public float acceleration = 2f;  // Hýzlanma miktarý
    public float deceleration = 2f;  // Yavaþlama miktarý
    public float currentSpeed = 0f;  // Anlýk hýz

    public int currentPoint = 0;  // Mevcut waypoint
    private bool movingForward = true;  // Hareket yönü

    void Start() {
        DetectWaypoint();
    }
    void Update() {
        if (Waypoints.Count == 0) return;

        // Kullanýcý giriþlerini al
        bool moveForward = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
        bool moveBackward = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);

        // Kullanýcý ileri gitmek istiyorsa hýzlan
        if (moveForward) {
            movingForward = true;
            currentSpeed = Mathf.MoveTowards(currentSpeed, maxSpeed, acceleration * Time.deltaTime);
        }
        else if (moveBackward) {
            movingForward = false;
            currentSpeed = Mathf.MoveTowards(currentSpeed, maxSpeed, acceleration * Time.deltaTime);
        }
        else {
            currentSpeed -= deceleration * Time.deltaTime * 0.5f;
        }

        // Hýzý maksimum ve minimum sýnýrlarla kýsýtla
        currentSpeed = Mathf.Clamp(currentSpeed, 0, maxSpeed);

        // Eðer hareket edecek kadar hýz varsa waypoint'e doðru ilerle
        if (currentSpeed > 0.01f && Waypoints.Count > 1) {
            MoveTrain();
        }
    }

    void MoveTrain() {
        float distanceToTarget = Vector3.Distance(transform.position, Waypoints[currentPoint].position);

        if (distanceToTarget < 0.1f) {
            if (movingForward && currentPoint < Waypoints.Count - 1) {
                currentPoint++;
            }
            else if (!movingForward && currentPoint > 0) {
                currentPoint--;
            }
        }

        // Treni hedef waypoint'e doðru hareket ettir
        transform.position = Vector3.MoveTowards(transform.position, Waypoints[currentPoint].position, currentSpeed * Time.deltaTime);

        // Treni yönlendirmek için rotasyonu deðiþtir
        if (Waypoints.Count > 1) {
            Vector3 direction = (Waypoints[currentPoint].position - transform.position).normalized;
            float angle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, -angle+270f);
        }
    }

    void DetectWaypoint() {
        GameObject[] WaypointObject = GameObject.FindGameObjectsWithTag("Waypoint");
        foreach (GameObject obj in WaypointObject) {
            Waypoints.Add(obj.transform);
        }
        Waypoints.Sort((a, b) => Vector3.Distance(transform.position, a.position).CompareTo(Vector3.Distance(transform.position, b.position)));
    }
}
