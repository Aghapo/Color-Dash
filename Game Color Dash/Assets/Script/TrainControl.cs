using System;
using System.Collections.Generic;
using UnityEngine;

public class TrainControl : MonoBehaviour {

    public List<Transform> Waypoints = new List<Transform>();

    public float maxSpeed = 5f;  // Maksimum hýz
    public float acceleration = 0.2f;  // Hýzlanma miktarý
    public float deceleration = 2f;  // Yavaþlama miktarý
    public float currentSpeed = 0f;  // Anlýk hýz

    public int currentPoint = 0;  // Mevcut waypoint
    private bool movingForward = true;  // Hareket yönü

    private Vector3 currentRotation;
    private Quaternion targetRotation;
    public float turnSpeed = 3f;

    void Start() {
        DetectWaypoint();
        transform.position = Waypoints[currentPoint].position;
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
            MoveTrain();
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
            if (movingForward) {// && currentPoint < Waypoints.Count - 1
                currentPoint = (currentPoint + 1) % Waypoints.Count;
            }
            else if (!movingForward && currentPoint > 0) {
                currentPoint--;
            }
        }


        transform.position = Vector3.MoveTowards(transform.position, Waypoints[currentPoint].position, currentSpeed * Time.deltaTime);

        // Yeni yönü belirle (sadece 2D düzlemde)
        Transform nextWaypoint = Waypoints[(currentPoint + 1) % Waypoints.Count];
        Vector2 direction = (nextWaypoint.position - transform.position).normalized;

        // Açýyý hesapla ve sadece Z ekseninde döndür (2D için Atan2 kullanýmý)
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Dönüþü yumuþat
        Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);












        /*
        // Treni hedef waypoint'e doðru hareket ettir
        transform.position = Vector3.MoveTowards(transform.position, Waypoints[currentPoint].position, currentSpeed * Time.deltaTime);

        // Treni yönlendirmek için rotasyonu deðiþtir

        Transform currentWaypointDegree = Waypoints[currentPoint];
        Transform nextWaypointDegree = Waypoints[(currentPoint + 1) % Waypoints.Count];

        Vector3 directionToCurrent = (currentWaypointDegree.position - transform.position).normalized;
        Vector3 direktionToNext = (nextWaypointDegree.position - currentWaypointDegree.position).normalized;
        Vector3 turnDirection = Vector3.Lerp(directionToCurrent, direktionToNext, distanceToTarget / Vector3.Distance(transform.position, currentWaypointDegree.position));

        float targetAngle = MathF.Atan2(turnDirection.y, turnDirection.x) * Mathf.Rad2Deg;
        targetRotation = Quaternion.Euler(0, 0,targetAngle);

        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);*/

    }

    void DetectWaypoint() {
        GameObject[] WaypointObject = GameObject.FindGameObjectsWithTag("Waypoint");
        List<Transform> unsortedWaypoints = new List<Transform>();

        foreach (GameObject obj in WaypointObject) {
            unsortedWaypoints.Add(obj.transform);
        }
        Vector3 center = Vector3.zero;
        foreach (Transform t in unsortedWaypoints) {
            center += t.position;
        }
        center /= unsortedWaypoints.Count;
        Waypoints = unsortedWaypoints;
        Waypoints.Sort((a, b) => {
            Vector3 aDir = (a.position - center).normalized;
            Vector3 bDir = (b.position - center).normalized;
            return Mathf.Atan2(aDir.y, aDir.x).CompareTo(Mathf.Atan2(bDir.y, bDir.x));
        });
        // Waypoints.Sort((a, b) => Vector3.Distance(transform.position, a.position).CompareTo(Vector3.Distance(transform.position, b.position)));
    }
}
