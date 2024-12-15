using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Button startButton;         // Reference to the Start Button
    [SerializeField] private TextMeshProUGUI _scoreText;  // Reference to the score text
    [SerializeField] private TextMeshProUGUI _gameoverText; // Reference to the game over text
    [SerializeField] private Button restartButton;       // Reference to the Restart Button
    [SerializeField] private PlatformManager platformManager; // Reference to the platform generator
    [Header("Game Objects")]
    [SerializeField] private GameObject ball;            // Reference to the ball object
   
    private int score = 0;           // Player's current score
    private bool isGameStarted = false; // Tracks if the game has started
   
    public bool isGameOver = false; // Tracks if the game is over
   
   

    private void Start()
    {
        platformManager = FindObjectOfType<PlatformManager>();
        InitializeGame();
    }

    /// <summary>
    /// Initializes the game by setting up the UI and pausing gameplay.
    /// </summary>
    private void InitializeGame()
    {
        // Pause the game initially
        Time.timeScale = 0;

        // Disable gameplay elements initially
        ball.SetActive(false);
        platformManager.enabled = false;

        // Set up UI state
        _gameoverText.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);
        _scoreText.text = "Score: 0";

        // Add listeners to buttons
        startButton.onClick.AddListener(StartGame);
        restartButton.onClick.AddListener(RestartGame);
    }

    /// <summary>
    /// Starts the game by enabling gameplay elements and resuming time.
    /// </summary>
    private void StartGame()
    {
        if (isGameStarted) return;

        isGameStarted = true;

        // Enable gameplay elements
        ball.SetActive(true);
        platformManager.enabled = true;

        // Resume the game
        Time.timeScale = 1;

        // Hide the start button
        startButton.gameObject.SetActive(false);
    }

    /// <summary>
    /// Increases the player's score.
    /// </summary>
    public void IncreaseScore()
    {
        if (isGameOver) return;

        score++;
        UpdateScoreText();
    }

    /// <summary>
    /// Updates the score text UI.
    /// </summary>
    private void UpdateScoreText()
    {
        _scoreText.text = $"Score: {score}";
    }

    /// <summary>
    /// Ends the game by displaying the Game Over screen and stopping time.
    /// </summary>
    public void GameOver()
    {
        if (isGameOver) return;

        isGameOver = true;

        // Stop the game
        Time.timeScale = 0;

        // Update and show UI
        _scoreText.gameObject.SetActive(false);
        _gameoverText.gameObject.SetActive(true);
        _gameoverText.text = $"Game Over\nScore: {score}";
        restartButton.gameObject.SetActive(true);
    }

    /// <summary>
    /// Restarts the game by reloading the current scene.
    /// </summary>
    private void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
