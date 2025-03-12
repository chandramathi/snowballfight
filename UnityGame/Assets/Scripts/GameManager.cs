using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // For UI elements
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject startMenuCanvas; 
    public GameObject gameOverCanvas; 

    public GameObject scoreCanvas; 

    public TextMeshProUGUI livesText;     

    public TextMeshProUGUI scoreText;         
    

    public GameObject liveCanvas;
    
    public int playerLives = 3;  // Starting lives
    public int scoreCount = 0;

    private bool isGamePaused = true; 

    void Start()
    {
        Time.timeScale = 0f; // Pause game initially
        startMenuCanvas.SetActive(true);
        liveCanvas.SetActive(true);
        scoreCanvas.SetActive(true);
        UnlockCursor();
        UpdateLivesUI();
    }

    public void StartLevel(int StartLevel)
    {
        startMenuCanvas.SetActive(false);
        // SceneManager.LoadScene(StartLevel);
        Time.timeScale = 1f;
        LockCursor();
        isGamePaused = false;
    }

    public void ReduceLife()
    {
        playerLives--;
        UpdateLivesUI();
        Debug.Log($"Lives left {playerLives}");
        if (playerLives <= 0)
        {
            GameOver();
        }
    }

    public void IncrementScore()
    {
        scoreCount++;
        UpdateScoreUI();
        Debug.Log($"Score {scoreCount}");
    }

    void GameOver()
    {
        Time.timeScale = 0f;
        
        gameOverCanvas.SetActive(true); // Show game over menu
        scoreCount = 0;
        playerLives = 3;
        UnlockCursor();
        UpdateScoreUI();
    }

    private void UpdateLivesUI()
    {
        if (livesText != null)
        {
            livesText.text = "Lives: " + playerLives;
        }
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + scoreCount;
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0); // Reloads the first scene (Start Menu)
    }

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
