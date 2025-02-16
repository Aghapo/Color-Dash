using System;
using System.Collections.Generic;
using UnityEngine;

public class TrainControl : MonoBehaviour {

    public List<Transform> Waypoints = new List<Transform>();

    public float maxSpeed = 5f;  // Maksimum h�z
    public float acceleration = 0.2f;  // H�zlanma miktar�
    public float deceleration = 2f;  // Yava�lama miktar�
    public float currentSpeed = 0f;  // Anl�k h�z

    public int currentPoint = 0;  // Mevcut waypoint
    private bool movingForward = true;  // Hareket y�n�

    private Vector3 currentRotation;
    private Quaternion targetRotation;
    public float turnSpeed = 3f;

    void Start() {
        DetectWaypoint();
        transform.position = Waypoints[currentPoint].position;
    }
    void Update() {
        if (Waypoints.Count == 0) return;

        // Kullan�c� giri�lerini al
        bool moveForward = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
        bool moveBackward = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);

        // Kullan�c� ileri gitmek istiyorsa h�zlan
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

        // H�z� maksimum ve minimum s�n�rlarla k�s�tla
        currentSpeed = Mathf.Clamp(currentSpeed, 0, maxSpeed);

        // E�er hareket edecek kadar h�z varsa waypoint'e do�ru ilerle
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

        // Yeni y�n� belirle (sadece 2D d�zlemde)
        Transform nextWaypoint = Waypoints[(currentPoint + 1) % Waypoints.Count];
        Vector2 direction = (nextWaypoint.position - transform.position).normalized;

        // A��y� hesapla ve sadece Z ekseninde d�nd�r (2D i�in Atan2 kullan�m�)
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // D�n��� yumu�at
        Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);












        /*
        // Treni hedef waypoint'e do�ru hareket ettir
        transform.position = Vector3.MoveTowards(transform.position, Waypoints[currentPoint].position, currentSpeed * Time.deltaTime);

        // Treni y�nlendirmek i�in rotasyonu de�i�tir

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
