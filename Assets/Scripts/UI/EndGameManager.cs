using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGameManager : MonoBehaviour
{
    [Header("UI References")]
    public Button restartButton;
    public Button mainMenuButton;
    public Button quitButton;
    
    [Header("Scene Names")]
    public string gameSceneName = "Main"; // Nama scene game utama
    public string mainMenuSceneName = "MainMenu"; // Nama scene main menu
    
    private void Start()
    {
        // Setup button listeners
        if (restartButton != null)
            restartButton.onClick.AddListener(RestartGame);
            
        if (mainMenuButton != null)
            mainMenuButton.onClick.AddListener(LoadMainMenu);
            
        if (quitButton != null)
            quitButton.onClick.AddListener(QuitGame);
            
        Debug.Log("Game Over! Player ran out of lives.");
    }
    
    public void RestartGame()
    {
        Debug.Log("Restarting game...");
        Time.timeScale = 1f; // Reset time scale
        SceneManager.LoadScene(gameSceneName);
    }
    
    public void LoadMainMenu()
    {
        Debug.Log("Loading main menu...");
        Time.timeScale = 1f; // Reset time scale
        SceneManager.LoadScene(mainMenuSceneName);
    }
    
    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
