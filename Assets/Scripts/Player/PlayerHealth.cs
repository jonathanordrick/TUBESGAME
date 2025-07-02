using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 10;
    public int currentHealth; // was private

    [Header("Lives Settings")]
    public int maxLives = 2; // Kesempatan hidup
    private int currentLives;

    [Header("Respawn Settings")]
    public float respawnDelay = 1f;
    public string endGameSceneName = "EndGame";

    private Animator anim;
    private bool isDead;
    private RespawnSystem playerRespawn; // Perbarui referensi ke RespawnSystem
    private PlayerMovement playerMovement;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        playerRespawn = GetComponent<RespawnSystem>(); // Perbarui referensi ke RespawnSystem
        playerMovement = GetComponent<PlayerMovement>();

        if (playerRespawn == null)
            Debug.LogError("RespawnSystem component not found on Player!");
        if (playerMovement == null)
            Debug.LogError("PlayerMovement component not found on Player!");
    }

    private void Start()
    {
        currentHealth = maxHealth;
        currentLives = maxLives;
    }

    public void ChangeHealth(int amount)
    {
        if (isDead) return;

        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (currentHealth > 0)
        {
            // Hurt animation
            anim.SetTrigger("hurt");
        }
        else
        {
            // Player mati
            isDead = true;
            currentLives--;
            Debug.Log($"Player died! Lives remaining: {currentLives}");

            if (currentLives <= 0)
            {
                // Game Over
                Debug.Log("Game Over! Loading EndGame scene...");
                SceneManager.LoadScene(endGameSceneName);
            }
            else
            {
                // Respawn sequence
                anim.SetTrigger("die");
                if (playerMovement != null) playerMovement.enabled = false;

                // Delay dan respawn
                Invoke(nameof(PerformRespawn), respawnDelay);
            }
        }
    }

    public void PerformRespawn()
    {
        if (currentLives <= 0)
        {
            Debug.Log("No lives left! Game Over.");
            SceneManager.LoadScene(endGameSceneName);
            return;
        }

        // Teleport ke checkpoint
        if (playerRespawn != null)
            playerRespawn.RespawnPlayer();

        // Reset health dan state
        currentHealth = maxHealth;
        isDead = false;

        // Enable movement
        if (playerMovement != null)
            playerMovement.enabled = true;

        Debug.Log("Player respawned at checkpoint with full health");
    }

    // Public methods untuk UI dan debugging
    public int GetCurrentLives()
    {
        return currentLives;
    }

    public int GetMaxLives()
    {
        return maxLives;
    }

    public bool IsDead()
    {
        return isDead;
    }
}
