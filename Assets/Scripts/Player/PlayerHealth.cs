using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 10;
    public int currentHealth;

    [Header("Lives Settings")]
    public int maxLives = 2;
    private int currentLives;

    [Header("Respawn Settings")]
    public float respawnDelay = 1f;
    public string endGameSceneName = "EndGame";
    public float hurtDuration = 1f; // Sesuaikan dengan durasi animasi Hurt

    private Animator anim;
    private bool isDead;
    private RespawnSystem playerRespawn;
    private PlayerMovement playerMovement;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        playerRespawn = GetComponent<RespawnSystem>();
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
            anim.SetTrigger("hurt");
            playerMovement.isHurting = true;
            anim.SetBool("IsHurting", true);
            StartCoroutine(ResetHurt());
            Debug.Log("Pemain terluka, isHurting = true");
        }
        else
        {
            isDead = true;
            currentLives--;
            Debug.Log($"Player died! Lives remaining: {currentLives}");

            if (currentLives <= 0)
            {
                Debug.Log("Game Over! Loading EndGame scene...");
                SceneManager.LoadScene(endGameSceneName);
            }
            else
            {
                anim.SetTrigger("die");
                if (playerMovement != null) playerMovement.enabled = false;

                Invoke(nameof(PerformRespawn), respawnDelay);
            }
        }
    }

    private IEnumerator ResetHurt()
    {
        Debug.Log($"Mulai ResetHurt, durasi: {hurtDuration}");
        yield return new WaitForSeconds(hurtDuration);
        playerMovement.isHurting = false;
        anim.SetBool("IsHurting", false);
        Debug.Log("Hurt selesai, isHurting = false");
    }

    public void PerformRespawn()
    {
        if (currentLives <= 0)
        {
            Debug.Log("No lives left! Game Over.");
            SceneManager.LoadScene(endGameSceneName);
            return;
        }

        if (playerRespawn != null)
            playerRespawn.RespawnPlayer();

        currentHealth = maxHealth;
        isDead = false;
        playerMovement.isHurting = false;
        anim.SetBool("IsHurting", false);
        anim.ResetTrigger("die"); // Tambahan untuk reset trigger die
        anim.SetBool("IsDead", false); // Tambahan jika ada parameter IsDead
        anim.Play("Idle"); // Paksa ke state Idle

        if (playerMovement != null)
            playerMovement.enabled = true;

        Debug.Log("Player respawned at checkpoint with full health");
    }

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