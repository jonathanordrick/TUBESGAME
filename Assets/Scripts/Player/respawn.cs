using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RespawnSystem : MonoBehaviour
{
    [Header("Respawn Settings")]
    public float respawnDelay = 1f;
    public string endGameSceneName = "EndGame";
    public int maxLives = 2;
    public int maxHealth = 10;

    private Vector2 checkpointPos;
    private int currentLives;
    private int currentHealth;
    private bool isDead = false;

    private Rigidbody2D rb;
    private PlayerMovement playerMovement;
    private Collider2D playerCollider;

    void Start()
    {
        checkpointPos = transform.position;
        rb = GetComponent<Rigidbody2D>();
        playerMovement = GetComponent<PlayerMovement>();
        playerCollider = GetComponent<Collider2D>();
        currentLives = maxLives;
        currentHealth = maxHealth;
        isDead = false;
        Debug.Log("Player spawned at " + transform.position);
    }

    public void UpdateCheckpoint(Vector2 newCheckpointPos)
    {
        checkpointPos = newCheckpointPos;
        Debug.Log("Checkpoint updated to " + checkpointPos);
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;
        currentHealth -= Mathf.Abs(amount);
        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            Debug.Log("Player took damage, health: " + currentHealth);
        }
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;
        currentLives--;
        Debug.Log("Player died! Lives left: " + currentLives);
        if (currentLives <= 0)
        {
            Debug.Log("Game Over! Loading EndGame scene...");
            SceneManager.LoadScene(endGameSceneName);
        }
        else
        {
            StartCoroutine(RespawnCoroutine());
        }
    }

    IEnumerator RespawnCoroutine()
    {
        yield return new WaitForSeconds(respawnDelay);
        RespawnPlayer();
    }

    public void RespawnPlayer()
    {
        if (currentLives <= 0)
        {
            Debug.Log("No lives left! Game Over.");
            SceneManager.LoadScene(endGameSceneName);
            return;
        }
        transform.position = checkpointPos;
        currentHealth = maxHealth;
        isDead = false;
        if (rb != null) rb.velocity = Vector2.zero;
        if (playerMovement != null) playerMovement.enabled = true;
        if (playerCollider != null) playerCollider.enabled = true;
        Debug.Log("Player respawned at " + checkpointPos);
    }
}
