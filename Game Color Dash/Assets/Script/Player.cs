using UnityEngine;


public class Player : MonoBehaviour {
    public float moveSpeed = 5f; // Hareket h�z�

    private Rigidbody2D rb;

    private Vector2 movement;
    
    public float minX = -2.5f, maxX = 2.5f; //EKran�n s�n�rlar�
    public float minY = -4f, maxY = 4f;

    private SpriteRenderer spriteRenderer;

    public Color color; // Renk de�i�keni eklendi

    void Start() {
        rb = GetComponent<Rigidbody2D>(); // Rigidbody2D bile�enini al
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (rb == null) {
            Debug.LogError("Rigidbody2D bile�eni eksik! L�tfen ekleyin.");
        }
    }

    void Update() {

        movement.x = Input.GetAxisRaw("Horizontal"); // A/D veya Sol/Sa� Ok Tu�lar�
        movement.y = Input.GetAxisRaw("Vertical");   // W/S veya Yukar�/A�a�� Ok Tu�lar�

    }

    void FixedUpdate() {
        // Rigidbody2D kullanarak hareket ettir
        rb.velocity = movement * moveSpeed;
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, minX, maxX), Mathf.Clamp(transform.position.y, minY, maxY), transform.position.z);

    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("ColorChanger")) { // �arpt��� nesnenin Tag'� ColorChanger olmal�
            SpriteRenderer otherSpriteRenderer = other.GetComponent<SpriteRenderer>();

            if (otherSpriteRenderer != null) {
                if (otherSpriteRenderer.color == spriteRenderer.color) {
                    spriteRenderer.color = new Color(Random.value, Random.value, Random.value); // Rastgele renk
                    ScoreWriter.Instance.AddScore();

                    Debug.Log("Renkler E�le�ti");
                }
                else {
                    Debug.Log("Renkler E�le�medi");
                    
                        ScoreWriter.Instance.GameOver();
                }
            
            }
        }
    }
}
 