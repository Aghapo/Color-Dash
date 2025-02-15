using UnityEngine;

public class WayPointMove : MonoBehaviour {
    [SerializeField] private Waypoint Waypoint;
    [SerializeField] private float speed = 1f;
    [SerializeField] private float rotationSpeed = 5f;

    private Transform CurrentWaypoint;
    private SpriteRenderer spriteRenderer; // Sprite'� �evirmek i�in

    private void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        CurrentWaypoint = Waypoint.GetNextWaypoint(CurrentWaypoint);
        transform.position = CurrentWaypoint.position;
        CurrentWaypoint = Waypoint.GetNextWaypoint(CurrentWaypoint);
    }

    private void Update() {
        // Hedefe olan y�n� hesapla
        Vector2 direction = (CurrentWaypoint.position - transform.position).normalized;

        // Karakterin y�n�n� belirle (sa�a veya sola bakmas� i�in)
        if (direction.x != 0) {
            spriteRenderer.flipX = direction.x < 0;
        }

        // Karakteri hareket ettir
        transform.position = Vector2.MoveTowards(transform.position,
                                               CurrentWaypoint.position,
                                               speed * Time.deltaTime);

        // Waypoint'e ula��ld���nda sonrakine ge�
        if (Vector2.Distance(transform.position, CurrentWaypoint.position) < 0.01f) {
            CurrentWaypoint = Waypoint.GetNextWaypoint(CurrentWaypoint);
        }
    }
}