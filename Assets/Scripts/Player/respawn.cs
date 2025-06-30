using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class respawn : MonoBehaviour
{
    [Header("Respawn Settings")]
    public float respawnDelay = 1f;
    public int maxRespawns = 2; // Jumlah maksimal respawn
    public LayerMask deathLayer = -1; // Layer yang menyebabkan kematian (seperti spikes, lava, dll)
    
    [Header("Components")]
    private Vector2 checkpointPos;
    private Rigidbody2D rb;
    private Collider2D playerCollider;
    private SpriteRenderer spriteRenderer;
    
    [Header("Effects")]
    public GameObject deathEffect; // Particle effect saat mati (opsional)
    public GameObject respawnEffect; // Particle effect saat respawn (opsional)
    
    [Header("Game Over")]
    public GameObject gameOverUI; // UI Game Over (opsional)
    public string gameOverSceneName = "EndGame"; // Nama scene Game Over
    public float gameOverDelay = 2f; // Delay sebelum pindah scene
    
    private bool isDead = false;
    private int currentRespawns = 0; // Counter respawn yang sudah digunakan
    private SceneManagement sceneManager; // Reference ke SceneManagement
    // Start is called before the first frame update
    void Start()
    {
        // Force set correct scene name (untuk memastikan tidak ada cache lama)
        gameOverSceneName = "EndGame"; // Force set tanpa kondisi
        Debug.Log($"Game Over Scene Name forced to: {gameOverSceneName}");
        
        // Set initial checkpoint sebagai posisi awal player
        checkpointPos = transform.position;
        
        // Ambil component yang dibutuhkan
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        // Cari SceneManagement component
        sceneManager = FindObjectOfType<SceneManagement>();
        if (sceneManager == null)
        {
            Debug.LogWarning("SceneManagement not found in scene!");
        }
        
        // Validasi component
        if (rb == null)
            Debug.LogWarning("Rigidbody2D not found on " + gameObject.name);
        if (playerCollider == null)
            Debug.LogWarning("Collider2D not found on " + gameObject.name);
    }

    // Update is called once per frame
    void Update()
    {
        // Cek jika player jatuh ke bawah map (berikan damage fatal)
        if (transform.position.y < -50f && !isDead)
        {
            PlayerHealth playerHealth = GetComponent<PlayerHealth>();
            if (playerHealth != null && playerHealth.IsAlive())
            {
                // Berikan damage yang cukup untuk membunuh player
                playerHealth.ChangeHealth(-playerHealth.maxHealth);
            }
        }
    }
    
    // Method untuk update checkpoint position (dipanggil dari Checkpoint script)
    public void UpdateCheckpoint(Vector2 newCheckpointPos)
    {
        checkpointPos = newCheckpointPos;
        Debug.Log("Checkpoint updated to: " + checkpointPos);
    }
    
    // Method untuk mendeteksi collision dengan objek berbahaya (hanya untuk damage, bukan instant death)
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isDead) return;
        
        PlayerHealth playerHealth = GetComponent<PlayerHealth>();
        if (playerHealth == null || !playerHealth.IsAlive()) return;
        
        int damage = 0;
        
        // Hanya berikan damage untuk objek dengan tag khusus, BUKAN untuk Ground atau Default
        // Hapus semua referensi tag yang belum terdefinisi untuk mencegah error
        // if (other.CompareTag("Spikes"))
        // {
        //     damage = 3; // Damage dari spikes
        // }
        // Hapus referensi DeathZone karena tag belum terdefinisi
        // else if (other.CompareTag("DeathZone"))
        // {
        //     damage = playerHealth.maxHealth; // Instant death untuk death zone
        // }
        // Hapus referensi tag yang tidak terdefinisi untuk mencegah error
        // JANGAN gunakan death layer detection untuk mencegah ground memberikan damage
        
        // Berikan damage jika ada
        if (damage > 0)
        {
            playerHealth.ChangeHealth(-damage);
            Debug.Log("Player took " + damage + " damage from " + other.name);
        }
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead) return;
        
        PlayerHealth playerHealth = GetComponent<PlayerHealth>();
        if (playerHealth == null || !playerHealth.IsAlive()) return;
        
        int damage = 0;
        
        // Hanya berikan damage untuk objek dengan tag khusus, BUKAN untuk Ground atau Default
        // Hapus semua referensi tag yang belum terdefinisi untuk mencegah error
        // if (collision.gameObject.CompareTag("Spikes"))
        // {
        //     damage = 3; // Damage dari spikes
        // }
        // Hapus referensi DeathZone karena tag belum terdefinisi
        // else if (collision.gameObject.CompareTag("DeathZone"))
        // {
        //     damage = playerHealth.maxHealth; // Instant death untuk death zone
        // }
        // Hapus referensi tag yang tidak terdefinisi untuk mencegah error
        // JANGAN gunakan death layer detection untuk mencegah ground memberikan damage
        
        // Berikan damage jika ada
        if (damage > 0)
        {
            playerHealth.ChangeHealth(-damage);
            Debug.Log("Player took " + damage + " damage from " + collision.gameObject.name);
        }
    }

    // Method ini dipanggil dari PlayerHealth ketika player mati
    public void Die()
    {
        if (isDead) return; // Prevent multiple deaths
        
        isDead = true;
        
        // Cek apakah masih ada respawn tersisa
        if (currentRespawns < maxRespawns)
        {
            currentRespawns++;
            GameStats.RecordDeath();
            GameStats.RecordRespawn();
            Debug.Log($"Player died! Respawning... ({currentRespawns}/{maxRespawns})");
            
            // Stop player movement
            if (rb != null)
            {
                rb.velocity = Vector2.zero;
                rb.isKinematic = true;
            }
            
            // Play death effect
            if (deathEffect != null)
            {
                Instantiate(deathEffect, transform.position, Quaternion.identity);
            }
            
            // Start respawn coroutine
            StartCoroutine(RespawnCoroutine());
        }
        else
        {
            // Game Over - tidak ada respawn lagi
            GameStats.RecordDeath();
            Debug.Log("No more respawns left! Game Over!");
            GameOver();
        }
    }
    
    private IEnumerator RespawnCoroutine()
    {
        // Wait for respawn delay
        yield return new WaitForSeconds(respawnDelay);
        
        // Respawn player
        RespawnPlayer();
    }
    
    private void RespawnPlayer()
    {
        // Move player to checkpoint
        transform.position = checkpointPos;
        
        // Reset physics
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.isKinematic = false;
        }
        
        // Reset health dan re-enable player controls
        PlayerHealth playerHealth = GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.ResetHealth();
        }
        
        // Play respawn effect
        if (respawnEffect != null)
        {
            Instantiate(respawnEffect, transform.position, Quaternion.identity);
        }
        
        // Reset death state
        isDead = false;
        
        Debug.Log($"Player respawned at checkpoint: {checkpointPos}. Respawns used: {currentRespawns}/{maxRespawns}");
    }
    
    private void GameOver()
    {
        // Stop all player movement
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.isKinematic = true;
        }
        
        // Disable player controls permanently
        PlayerHealth playerHealth = GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.enabled = false;
        }
        
        PlayerMovement playerMovement = GetComponent<PlayerMovement>();
        if (playerMovement != null)
        {
            playerMovement.enabled = false;
        }
        
        // Show Game Over UI
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true);
        }
        
        // Play death effect
        if (deathEffect != null)
        {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
        }
        
        Debug.Log("=== GAME OVER ===");
        
        // Pindah ke scene Game Over setelah delay
        StartCoroutine(LoadGameOverScene());
    }
    
    private IEnumerator LoadGameOverScene()
    {
        yield return new WaitForSeconds(gameOverDelay);
        
        // Force set scene name again just to be absolutely sure
        gameOverSceneName = "EndGame";
        
        // Load scene Game Over menggunakan SceneManagement yang sudah ada
        Debug.Log("=== LOADING GAME OVER SCENE ===");
        Debug.Log("Scene Name: " + gameOverSceneName);
        Debug.Log("SceneManager exists: " + (sceneManager != null));
        
        if (sceneManager != null)
        {
            Debug.Log("Using SceneManagement.GantiScene() method");
            sceneManager.GantiScene(gameOverSceneName);
        }
        else
        {
            // Fallback jika SceneManagement tidak ditemukan
            Debug.LogWarning("SceneManagement not found! Using fallback method.");
            Debug.Log("Using UnityEngine.SceneManagement.SceneManager.LoadScene()");
            UnityEngine.SceneManagement.SceneManager.LoadScene(gameOverSceneName);
        }
    }
    
    // Public method untuk respawn manual (bisa dipanggil dari script lain)
    public void ForceRespawn()
    {
        if (!isDead)
        {
            Die();
        }
    }
    
    // Method untuk reset checkpoint ke posisi awal
    public void ResetCheckpointToStart()
    {
        checkpointPos = transform.position;
    }
    
    // Public methods untuk UI atau debugging
    public int GetRemainingRespawns()
    {
        return maxRespawns - currentRespawns;
    }
    
    public int GetUsedRespawns()
    {
        return currentRespawns;
    }
    
    public bool CanRespawn()
    {
        return currentRespawns < maxRespawns;
    }
    
    // Method untuk reset respawn count (misalnya saat mencapai checkpoint baru)
    public void ResetRespawnCount()
    {
        currentRespawns = 0;
        Debug.Log("Respawn count reset! Full respawns available again.");
    }
    
    // Method untuk menambah max respawns (power-up, dll)
    public void AddRespawn(int amount = 1)
    {
        maxRespawns += amount;
        Debug.Log($"Respawn limit increased! New limit: {maxRespawns}");
    }
    
    // Debug helper untuk cek nilai di Inspector
    void OnValidate()
    {
        // Method ini dipanggil saat nilai di Inspector berubah
        // Force set ke EndGame selalu
        gameOverSceneName = "EndGame";
        Debug.Log($"OnValidate: Game Over Scene Name forced to: {gameOverSceneName}");
    }
}
