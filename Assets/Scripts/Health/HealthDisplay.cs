using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    public PlayerHealth playerHealth; // Referensi ke PlayerHealth
    public Sprite emptyHeart;
    public Sprite fullHeart;
    public Image[] hearts;

    // Start is called before the first frame update
    void Start()
    {
        // Opsional: Pastikan jumlah hati sesuai dengan maxHealth awal
        UpdateHearts();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHearts();
    }

    void UpdateHearts()
    {
        if (playerHealth == null) return; // Pastikan referensi tidak null

        float currentHealth = playerHealth.currentHealth; // Ambil currentHealth dari PlayerHealth
        int maxHealth = playerHealth.maxHealth; // Ambil maxHealth dari PlayerHealth

        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < Mathf.CeilToInt(currentHealth)) // Gunakan currentHealth untuk menentukan hati penuh
            {
                hearts[i].sprite = fullHeart;
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
                if (i < maxHealth) // Hanya tampilkan hati kosong sampai maxHealth
                {
                    hearts[i].enabled = true;
                }
                else
                {
                    hearts[i].enabled = false; // Sembunyikan hati berlebih
                }
            }
        }
    }
}