using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public static GameManager Instance;
    public Text scoreText;
    private int score = 0;

    void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        else {
            Destroy(gameObject);
        }
    }

    public void AddScore(int points) {
        score += points;
        UpdateScoreText();
    }

    void UpdateScoreText() {
        if (scoreText != null) {
            scoreText.text = "Score: " + score.ToString();
        }
    }

    public void GameOver() {
        // Oyun biti� mant���
        Debug.Log("Game Over!");
        // Burada oyun biti� ekran�n� g�sterebilirsiniz
    }
}