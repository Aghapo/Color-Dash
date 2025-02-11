using UnityEngine;

public class TrainControl : MonoBehaviour {
    public Transform[] Waypoint;
    
    //Hýz Deðiþkenleri
    public float Speed = 5f; //maks speed
    private float acceleration = 2f; //hýzlanma
    private float deceleration = 2f; //yavaþlama
    public float CurrentSpeed = 0f;
    public float targetSpeed = 0f;


    public int CurrentPoint = 0;

    private bool IsMovingNext = false;
    void Update() {
        float distanceToTarget = Vector3.Distance(transform.position, Waypoint[CurrentPoint].position);

        bool MoveForward = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
        bool MoveBackward = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);
        if (distanceToTarget < 0.1f && !IsMovingNext) {
            if (MoveForward && CurrentPoint < Waypoint.Length - 1) {
                CurrentPoint++;
                IsMovingNext = true;
            }
            else if (MoveBackward && CurrentPoint > 0) {

                CurrentPoint--;
                IsMovingNext = true;
            }
        }


        if (distanceToTarget > 0.1f) {
            targetSpeed = Speed;
        }
        else {
            targetSpeed = 0f;
            IsMovingNext = false;
        }
        if (CurrentSpeed < targetSpeed) {
            CurrentSpeed += acceleration * Time.deltaTime;
        }
        else if (CurrentSpeed > targetSpeed) {
            CurrentSpeed -= deceleration * Time.deltaTime;

        }
        if ((CurrentPoint == 0 || CurrentPoint == Waypoint.Length - 1) && distanceToTarget < 0.1f) {
            CurrentSpeed = 0f;
            IsMovingNext = false;
        }


        transform.position = Vector3.Lerp(transform.position, Waypoint[CurrentPoint].position, CurrentSpeed * Time.deltaTime);


    }
}
