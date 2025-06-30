using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 10;
    public float currentHealth;
    private Animator anim;
    private bool isDead;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        currentHealth = maxHealth; // Pastikan health di-set di sini
    }

    public void ChangeHealth(int amount)
    {
        if (isDead) return;

        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // mencegah negatif berlebih

        if (currentHealth > 0)
        {
            anim.SetTrigger("hurt");
        }
        else
        {
            anim.SetTrigger("die");
            GetComponent<PlayerMovement>().enabled = false;
            isDead = true;
            
            // Panggil sistem respawn ketika player mati
            respawn playerRespawn = GetComponent<respawn>();
            if (playerRespawn != null)
            {
                playerRespawn.Die();
            }
            else
            {
                Debug.LogWarning("Respawn component tidak ditemukan pada Player!");
            }
        }
    }
    
    // Method untuk reset health ketika respawn
    public void ResetHealth()
    {
        currentHealth = maxHealth;
        isDead = false;
        
        // Re-enable player movement
        PlayerMovement playerMovement = GetComponent<PlayerMovement>();
        if (playerMovement != null)
        {
            playerMovement.enabled = true;
        }
        
        Debug.Log("Player health reset to: " + currentHealth);
    }
    
    // Method untuk check apakah player masih hidup
    public bool IsAlive()
    {
        return !isDead && currentHealth > 0;
    }
}
