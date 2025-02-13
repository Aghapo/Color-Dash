using System.Collections.Generic;
using UnityEngine;

public class TrainControl : MonoBehaviour {
    public List<Transform> Waypoints = new List<Transform>();

    public float maxSpeed = 5f;  // Maksimum hýz
    public float acceleration = 2f;  // Hýzlanma miktarý
    public float deceleration = 2f;  // Yavaþlama miktarý
    public float currentSpeed = 0f;  // Anlýk hýz

    public int currentPoint = 0;  // Mevcut waypoint
    private bool movingForward = false;  // Hareket yönü

    void Start() {
        DetectWaypoint();
    }
    void Update() {

        if (Waypoints.Count == 0) return;

        // Kullanýcý giriþlerini al
        bool moveForward = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
        bool moveBackward = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);

        // Kullanýcý ileri gitmek istiyorsa hýzlan
        if (moveForward || moveBackward) {
            currentSpeed = Mathf.MoveTowards(currentSpeed, maxSpeed , acceleration * Time.deltaTime);
        }
        // Kullanýcý yavaþlamak istiyorsa yavaþla
        else if (moveBackward) {
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0, deceleration * Time.deltaTime);
        }
        // Hiçbir tuþa basýlmýyorsa zamanla yavaþla (doðal frenleme efekti)
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
        // Mevcut waypoint ile tren arasýndaki mesafe
        float distanceToTarget = Vector3.Distance(transform.position, Waypoints[currentPoint].position);

        if (distanceToTarget > 0.1f) {
            currentSpeed = maxSpeed;
        }
        else {
            currentSpeed = 0f;
            movingForward = false;
        }

        if (currentSpeed < maxSpeed) {
            currentSpeed += acceleration * Time.deltaTime;
        }
        else if (currentSpeed > maxSpeed) {
            currentSpeed -= deceleration * Time.deltaTime;
        }

        // Yeterince yaklaþtýysa yeni bir waypoint seç
        if (distanceToTarget < 0.1f) {
            if (movingForward) {
                if (currentPoint < Waypoints.Count - 1) {
                    currentPoint++;
                    movingForward = true;
                }
            }
            else {
                if (currentPoint > 0) {
                    currentPoint--;
                    movingForward = true;
                }
            }
        }

        // Treni hedef waypoint'e doðru hareket ettir
        transform.position = Vector3.MoveTowards(transform.position, Waypoints[currentPoint].position, currentSpeed * Time.deltaTime);
        
        // Treni yönlendirmek için rotasyonu deðiþtir
        if (Waypoints.Count > 1) {
            Vector3 direction = (Waypoints[currentPoint].position - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
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
