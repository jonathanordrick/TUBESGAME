using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    [Header("UI Elements")]
    public Text gameOverText;
    public Button restartButton;
    public Button mainMenuButton;
    public Button quitButton;
    
    [Header("Scene Names")]
    public string gameSceneName = "Main"; // Nama scene game utama untuk restart
    public string mainMenuSceneName = "MenuGame"; // Nama scene main menu
    
    [Header("Effects")]
    public AudioClip gameOverSound;
    private AudioSource audioSource;
    private SceneManagement sceneManager;
    
    void Start()
    {
        // Cari SceneManagement component
        sceneManager = FindObjectOfType<SceneManagement>();
        if (sceneManager == null)
        {
            Debug.LogWarning("SceneManagement not found! Creating temporary one.");
            GameObject temp = new GameObject("TempSceneManager");
            sceneManager = temp.AddComponent<SceneManagement>();
        }
        
        // Setup audio
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        
        // Play game over sound
        if (gameOverSound != null)
        {
            audioSource.PlayOneShot(gameOverSound);
        }
        
        // Setup button listeners
        if (restartButton != null)
        {
            restartButton.onClick.AddListener(RestartGame);
        }
        
        if (mainMenuButton != null)
        {
            mainMenuButton.onClick.AddListener(GoToMainMenu);
        }
        
        if (quitButton != null)
        {
            quitButton.onClick.AddListener(QuitGame);
        }
        
        // Update text
        if (gameOverText != null)
        {
            gameOverText.text = "GAME OVER\nNo more lives remaining!";
        }
        
        // Ensure normal time scale
        Time.timeScale = 1f;
        
        Debug.Log("Game Over Scene loaded successfully");
    }
    
    public void RestartGame()
    {
        Debug.Log("Restarting game...");
        
        // Optional: Reset stats atau lakukan action lain sebelum restart
        GameStats.OnGameRestart();
        
        if (sceneManager != null)
        {
            sceneManager.GantiScene(gameSceneName);
        }
    }
    
    public void GoToMainMenu()
    {
        Debug.Log("Going to main menu...");
        if (sceneManager != null)
        {
            sceneManager.GantiScene(mainMenuSceneName);
        }
    }
    
    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        if (sceneManager != null)
        {
            sceneManager.KeluarGame();
        }
    }
    
    // Method untuk dipanggil dari UI atau script lain
    public void LoadScene(string sceneName)
    {
        if (sceneManager != null)
        {
            sceneManager.GantiScene(sceneName);
        }
    }
    
    void Update()
    {
        // Shortcut keys
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }
        
        if (Input.GetKeyDown(KeyCode.M))
        {
            GoToMainMenu();
        }
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            QuitGame();
        }
    }
}
