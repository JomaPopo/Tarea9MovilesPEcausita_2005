using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TMP_Text finalScoreText;
    [SerializeField] private TMP_Text highScoreText;

    private int currentScore = 0; // Variable para almacenar el puntaje actual
    private int highScore = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        UpdateScoreDisplay(); // Inicializa el puntaje en 0 al inicio
    }

    private void Update()
    {
        // Actualiza el texto del puntaje en cada frame (innecesario si no hay cambios)
        UpdateScoreDisplay();
    }

    public void UpdateScore(int score)
    {
        currentScore = score; // Actualiza el puntaje actual
        UpdateScoreDisplay(); // Actualiza el texto del puntaje
    }

    public void AddScore(int points)
    {
        currentScore += points; // Añade puntos al puntaje actual
        UpdateScoreDisplay(); // Actualiza el texto del puntaje
    }

    private void UpdateScoreDisplay()
    {
        scoreText.text = $"Score: {currentScore}"; // Actualiza el texto del puntaje
    }

    public void GameOver(int finalscore)
    {
        if (currentScore > highScore)
        {
            highScore = currentScore;
            PlayerPrefs.SetInt("HighScore", highScore);
        }

        finalScoreText.text = $"Final Score: {currentScore}";
        highScoreText.text = $"High Score: {highScore}";
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