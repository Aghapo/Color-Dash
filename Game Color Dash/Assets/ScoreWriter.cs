using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;

public class ScoreWriter : MonoBehaviour
{
    public static ScoreWriter Instance;

    public TextMeshProUGUI scoreText;

    private int score = 0;

    private void Awake() {
        if (Instance == null) {
            Instance = this;    
        }
    }
    void Start()
    {
        UpdateScoreUI(); 
    }

   public void AddScore() {
        score += 1;
        UpdateScoreUI();
    }

    public void GameOver() {
        if (score == 0) {
            Debug.Log("Game Over");
        }
        else {
            score = score - 1;
        }
        UpdateScoreUI();
    }

    void UpdateScoreUI() {
        if (score != null) {
            scoreText.text = "Score : " + score;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
