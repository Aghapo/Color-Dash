using System;
using System.Collections.Generic;
using UnityEngine;

public class TrainControl : MonoBehaviour {

    public List<Transform> Waypoints = new List<Transform>();

    public float maxSpeed = 5f;  // Maksimum hýz
    public float acceleration = 4f;  // Hýzlanma miktarý
    public float deceleration = 6f;  // Yavaþlama miktarý
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
        if (moveForward && movingForward) {
            currentSpeed = Mathf.MoveTowards(currentSpeed, maxSpeed, acceleration * Time.deltaTime);
        }
        //Ýleri giderken durdurma
        else if (moveForward && !movingForward) {
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0, deceleration * Time.deltaTime);
            if (currentSpeed <= 0.01f) { movingForward = true; }
        }
        //Ýleri giderken yavaþla
        else if (moveBackward && movingForward) {
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0, deceleration * Time.deltaTime);
            if (currentSpeed <= 0.01f) { movingForward = false; }
        }
        //Geri gidiyorsa
        else if (moveBackward && !movingForward) {
            currentSpeed = Mathf.MoveTowards(currentSpeed, maxSpeed, acceleration * Time.deltaTime);

        }
        else if (moveForward && !movingForward) {
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0, deceleration * Time.deltaTime);
            if (currentSpeed <= 0.01f) { movingForward = true; }
        }
        else {
            currentSpeed -= deceleration * Time.deltaTime * 0.01f;
        }

        // Hýzý maksimum ve minimum sýnýrlarla kýsýtla
        currentSpeed = Mathf.Clamp(currentSpeed, 0, maxSpeed);

        // Eðer hareket edecek kadar hýz varsa waypoint'e doðru ilerle
        if (currentSpeed > 0.01f && Waypoints.Count > 1) {
            MoveTrain();
        }

    }

    void MoveTrain() {
        if (Waypoints.Count == 0) return;

        int closestWaypoints = FindClosestWaypoint();

        if (movingForward) {
            currentPoint = (closestWaypoints + 1) % Waypoints.Count;
        }
        else if (closestWaypoints > 0) {
            currentPoint = closestWaypoints - 1;//ileri giderken sonraki waypoint
        }
        else currentPoint = 0;//Geri giderken önceki waypoint


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
        AlignRotation(); 

    }

    int FindClosestWaypoint() {
        int closestIndex = 0;
        float minDistance = float.MaxValue;

        for (int i = 0; i < Waypoints.Count; i++) {
            float distance = Vector3.Distance(transform.position, Waypoints[i].position);
            if (distance < minDistance) {
                minDistance = distance;
                closestIndex = i;
            }
        }
        return closestIndex;
    }

    void AlignRotation() {

        if (Waypoints.Count < 2) return;

       // if (movingForward) {
            // Yeni yönü belirle (sadece 2D düzlemde)
            Transform nextWaypoint = Waypoints[(currentPoint + 1) % Waypoints.Count];
            Vector2 direction = (nextWaypoint.position - transform.position).normalized;

            // Açýyý hesapla ve sadece Z ekseninde döndür (2D için Atan2 kullanýmý)
            float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Dönüþü yumuþat
            Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
       // }


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
