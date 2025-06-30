using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RespawnUI : MonoBehaviour
{
    [Header("UI Elements")]
    public Text respawnCountText; // UI Text untuk menampilkan respawn tersisa
    public Image[] respawnIcons; // Array of images untuk visual respawn count
    
    private respawn playerRespawn;
    
    void Start()
    {
        // Cari player respawn script
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerRespawn = player.GetComponent<respawn>();
        }
        
        // Update UI awal
        UpdateRespawnUI();
    }
    
    void Update()
    {
        // Update UI setiap frame (atau bisa pakai event system)
        UpdateRespawnUI();
    }
    
    private void UpdateRespawnUI()
    {
        if (playerRespawn == null) return;
        
        int remaining = playerRespawn.GetRemainingRespawns();
        int used = playerRespawn.GetUsedRespawns();
        
        // Update text
        if (respawnCountText != null)
        {
            respawnCountText.text = $"Lives: {remaining}";
        }
        
        // Update icons (jika ada)
        if (respawnIcons != null && respawnIcons.Length > 0)
        {
            for (int i = 0; i < respawnIcons.Length; i++)
            {
                if (i < remaining)
                {
                    // Show active life icon
                    respawnIcons[i].color = Color.white;
                }
                else
                {
                    // Show used/inactive life icon
                    respawnIcons[i].color = Color.gray;
                }
            }
        }
    }
}
