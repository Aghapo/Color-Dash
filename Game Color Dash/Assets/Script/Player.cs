using UnityEngine;


public class Player : MonoBehaviour {
    public float moveSpeed = 5f; // Hareket hýzý

    private Rigidbody2D rb;

    private Vector2 movement;
    
    public float minX = -2.5f, maxX = 2.5f; //EKranýn sýnýrlarý
    public float minY = -4f, maxY = 4f;

    private SpriteRenderer spriteRenderer;

    public Color color; // Renk deðiþkeni eklendi

    void Start() {
        rb = GetComponent<Rigidbody2D>(); // Rigidbody2D bileþenini al
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (rb == null) {
            Debug.LogError("Rigidbody2D bileþeni eksik! Lütfen ekleyin.");
        }
    }

    void Update() {

        movement.x = Input.GetAxisRaw("Horizontal"); // A/D veya Sol/Sað Ok Tuþlarý
        movement.y = Input.GetAxisRaw("Vertical");   // W/S veya Yukarý/Aþaðý Ok Tuþlarý

    }

    void FixedUpdate() {
        // Rigidbody2D kullanarak hareket ettir
        rb.velocity = movement * moveSpeed;
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, minX, maxX), Mathf.Clamp(transform.position.y, minY, maxY), transform.position.z);

    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("ColorChanger")) { // Çarptýðý nesnenin Tag'ý ColorChanger olmalý
            SpriteRenderer otherSpriteRenderer = other.GetComponent<SpriteRenderer>();

            if (otherSpriteRenderer != null) {
                if (otherSpriteRenderer.color == spriteRenderer.color) {
                    spriteRenderer.color = new Color(Random.value, Random.value, Random.value); // Rastgele renk
                    ScoreWriter.Instance.AddScore();

                    Debug.Log("Renkler Eþleþti");
                }
                else {
                    Debug.Log("Renkler Eþleþmedi");
                    
                        ScoreWriter.Instance.GameOver();
                }
            
            }
        }
    }
}
 