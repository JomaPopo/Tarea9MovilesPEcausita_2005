using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text highScoreText;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TMP_Text finalScoreText;

    private int currentScore = 0;
    private int highScore = 0;

    private int lastDisplayedScore = -1;
    private int lastDisplayedHighScore = -1;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        UpdateScoreDisplay(); // Mostrar los puntajes al inicio
    }

    private void Update()
    {
        // Mostrar puntajes en tiempo real si cambian
        UpdateScoreDisplay();
    }

    public void UpdateScore(int score)
    {
        currentScore = score;

        // Si superamos el high score, lo actualizamos en tiempo real
        if (currentScore > highScore)
        {
            highScore = currentScore;
            PlayerPrefs.SetInt("HighScore", highScore);
        }
    }

    public void AddScore(int points)
    {
        currentScore += points;

        if (currentScore > highScore)
        {
            highScore = currentScore;
            PlayerPrefs.SetInt("HighScore", highScore);
        }
    }

    private void UpdateScoreDisplay()
    {
        if (lastDisplayedScore != currentScore)
        {
            scoreText.text = $"Score: {currentScore}";
            lastDisplayedScore = currentScore;
        }

        if (lastDisplayedHighScore != highScore)
        {
            highScoreText.text = $"High Score: {highScore}";
            lastDisplayedHighScore = highScore;
        }
    }

    public void GameOver(int finalScore)
    {
        finalScoreText.text = $"Final Score: {currentScore}";
        gameOverPanel.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
