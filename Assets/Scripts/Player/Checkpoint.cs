using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [Header("Checkpoint Settings")]
    public GameObject checkpointEffect; // Visual effect saat checkpoint aktif
    public AudioClip checkpointSound; // Sound effect saat checkpoint diaktivasi
    
    private respawn playerRespawn;
    private bool isActivated = false;
    private AudioSource audioSource;
    
    private void Awake()
    {
        // Cari player respawn script
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerRespawn = player.GetComponent<respawn>();
            if (playerRespawn == null)
            {
                Debug.LogError("respawn component not found on Player GameObject!");
            }
        }
        else
        {
            Debug.LogError("Player GameObject not found!");
        }
        
        // Setup audio source
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null && checkpointSound != null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && playerRespawn != null && !isActivated)
        {
            // Update checkpoint position
            playerRespawn.UpdateCheckpoint(transform.position);
            
            // Mark as activated
            isActivated = true;
            
            // Play sound effect
            if (audioSource != null && checkpointSound != null)
            {
                audioSource.PlayOneShot(checkpointSound);
            }
            
            // Play visual effect
            if (checkpointEffect != null)
            {
                Instantiate(checkpointEffect, transform.position, Quaternion.identity);
            }
            
            Debug.Log("Checkpoint activated at: " + transform.position);
        }
    }
}
