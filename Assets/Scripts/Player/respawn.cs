using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class respawn : MonoBehaviour
{
    [Header("Respawn Settings")]
    public float respawnDelay = 1f;
    public LayerMask deathLayer = -1; // Layer yang menyebabkan kematian (seperti spikes, lava, dll)
    
    [Header("Components")]
    private Vector2 checkpointPos;
    private Rigidbody2D rb;
    private Collider2D playerCollider;
    private SpriteRenderer spriteRenderer;
    
    [Header("Effects")]
    public GameObject deathEffect; // Particle effect saat mati (opsional)
    public GameObject respawnEffect; // Particle effect saat respawn (opsional)
    
    private bool isDead = false;
    // Start is called before the first frame update
    void Start()
    {
        // Set initial checkpoint sebagai posisi awal player
        checkpointPos = transform.position;
        
        // Ambil component yang dibutuhkan
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        // Validasi component
        if (rb == null)
            Debug.LogWarning("Rigidbody2D not found on " + gameObject.name);
        if (playerCollider == null)
            Debug.LogWarning("Collider2D not found on " + gameObject.name);
    }

    // Update is called once per frame
    void Update()
    {
        // Cek jika player jatuh ke bawah map (opsional)
        if (transform.position.y < -50f && !isDead)
        {
            Die();
        }
    }
    
    // Method untuk update checkpoint position (dipanggil dari Checkpoint script)
    public void UpdateCheckpoint(Vector2 newCheckpointPos)
    {
        checkpointPos = newCheckpointPos;
        Debug.Log("Checkpoint updated to: " + checkpointPos);
    }
    
    // Method untuk mendeteksi collision dengan objek mematikan
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Cek jika collide dengan objek di death layer
        if (((1 << other.gameObject.layer) & deathLayer) != 0 && !isDead)
        {
            Die();
        }
        
        // Atau cek berdasarkan tag
        if (other.CompareTag("DeathZone") || other.CompareTag("Enemy") || other.CompareTag("Spikes"))
        {
            Die();
        }
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Cek collision dengan objek mematikan
        if (((1 << collision.gameObject.layer) & deathLayer) != 0 && !isDead)
        {
            Die();
        }
        
        if (collision.gameObject.CompareTag("DeathZone") || collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Spikes"))
        {
            Die();
        }
    }

    void Die()
    {
        if (isDead) return; // Prevent multiple deaths
        
        isDead = true;
        Debug.Log("Player died! Respawning...");
        
        // Stop player movement
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.isKinematic = true;
        }
        
        // Disable player controls (jika ada script movement)
        var playerMovement = GetComponent<MonoBehaviour>();
        if (playerMovement != null)
        {
            playerMovement.enabled = false;
        }
        
        // Play death effect
        if (deathEffect != null)
        {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
        }
        
        // Start respawn coroutine
        StartCoroutine(RespawnCoroutine());
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
        
        // Re-enable player controls
        var playerMovement = GetComponent<MonoBehaviour>();
        if (playerMovement != null)
        {
            playerMovement.enabled = true;
        }
        
        // Play respawn effect
        if (respawnEffect != null)
        {
            Instantiate(respawnEffect, transform.position, Quaternion.identity);
        }
        
        // Reset death state
        isDead = false;
        
        Debug.Log("Player respawned at: " + checkpointPos);
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
}
