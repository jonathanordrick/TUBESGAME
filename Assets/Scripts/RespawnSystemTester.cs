using UnityEngine;
using UnityEngine.SceneManagement;

public class RespawnSystemTester : MonoBehaviour
{
    [Header("Testing Tools")]
    [SerializeField] private respawn respawnScript;
    [SerializeField] private PlayerHealth playerHealth;
    
    [Header("Test Controls")]
    [Tooltip("Press this key to force kill the player")]
    public KeyCode forceDeathKey = KeyCode.K;
    [Tooltip("Press this key to reset respawn count")]
    public KeyCode resetRespawnKey = KeyCode.R;
    [Tooltip("Press this key to show current status")]
    public KeyCode showStatusKey = KeyCode.I;

    void Start()
    {
        // Auto-find components if not assigned
        if (respawnScript == null)
            respawnScript = FindObjectOfType<respawn>();
        
        if (playerHealth == null)
            playerHealth = FindObjectOfType<PlayerHealth>();
        
        // Log initial status
        LogCurrentStatus();
    }

    void Update()
    {
        // Test controls
        if (Input.GetKeyDown(forceDeathKey))
        {
            ForceDeath();
        }
        
        if (Input.GetKeyDown(resetRespawnKey))
        {
            ResetRespawns();
        }
        
        if (Input.GetKeyDown(showStatusKey))
        {
            LogCurrentStatus();
        }
    }

    public void ForceDeath()
    {
        if (playerHealth != null && playerHealth.IsAlive())
        {
            Debug.Log("=== FORCING PLAYER DEATH (TEST) ===");
            playerHealth.ChangeHealth(-playerHealth.maxHealth);
        }
        else
        {
            Debug.LogWarning("Cannot force death: Player is already dead or PlayerHealth not found!");
        }
    }

    public void ResetRespawns()
    {
        if (respawnScript != null)
        {
            respawnScript.ResetRespawnCount();
            Debug.Log("=== RESPAWN COUNT RESET (TEST) ===");
            LogCurrentStatus();
        }
        else
        {
            Debug.LogWarning("Respawn script not found!");
        }
    }

    public void LogCurrentStatus()
    {
        Debug.Log("=== RESPAWN SYSTEM STATUS ===");
        
        if (respawnScript != null)
        {
            Debug.Log($"Used Respawns: {respawnScript.GetUsedRespawns()}");
            Debug.Log($"Remaining Respawns: {respawnScript.GetRemainingRespawns()}");
            Debug.Log($"Can Respawn: {respawnScript.CanRespawn()}");
        }
        else
        {
            Debug.LogWarning("Respawn script not found!");
        }
        
        if (playerHealth != null)
        {
            Debug.Log($"Player Health: {playerHealth.currentHealth}/{playerHealth.maxHealth}");
            Debug.Log($"Player Alive: {playerHealth.IsAlive()}");
        }
        else
        {
            Debug.LogWarning("PlayerHealth script not found!");
        }
        
        // Check if EndGame scene is available
        bool endGameSceneExists = Application.CanStreamedLevelBeLoaded("EndGame");
        Debug.Log($"EndGame scene can be loaded: {endGameSceneExists}");
        
        Debug.Log($"Current Scene: {SceneManager.GetActiveScene().name}");
        
        // Check GameStats
        Debug.Log($"Deaths: {GameStats.Deaths}");
        Debug.Log($"Respawns: {GameStats.Respawns}");
        Debug.Log($"Checkpoints: {GameStats.Checkpoints}");
    }

    void OnGUI()
    {
        // Simple on-screen instructions
        GUI.Label(new Rect(10, 10, 300, 20), $"Press {forceDeathKey} to force death");
        GUI.Label(new Rect(10, 30, 300, 20), $"Press {resetRespawnKey} to reset respawn count");
        GUI.Label(new Rect(10, 50, 300, 20), $"Press {showStatusKey} to show status");
        
        if (respawnScript != null)
        {
            GUI.Label(new Rect(10, 80, 300, 20), $"Respawns: {respawnScript.GetUsedRespawns()}/{respawnScript.GetUsedRespawns() + respawnScript.GetRemainingRespawns()}");
        }
        
        if (playerHealth != null)
        {
            GUI.Label(new Rect(10, 100, 300, 20), $"Health: {playerHealth.currentHealth}/{playerHealth.maxHealth}");
        }
    }
}
