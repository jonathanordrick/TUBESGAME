using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int currentHealth;
    public int maxHealth;

    private void Start()
    {
        currentHealth = maxHealth; // Inisialisasi health musuh
        gameObject.SetActive(true); // Pastikan aktif saat spawn
    }

    public void ChangeHealth(int amount)
    {
        currentHealth += amount; // Tambah atau kurangi health
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth; // Batasi health maksimum
        }
        else if (currentHealth <= 0)
        {
            Die(); // Panggil fungsi mati jika health habis
        }
    }

    private void Die()
    {
        // Dapatkan Animator dan atur animasi sebelum menonaktifkan
        Animator anim = GetComponent<Animator>();
        if (anim != null)
        {
            anim.SetBool("Moving", false); // Hentikan animasi bergerak
            anim.SetTrigger("Die"); // Opsional: Picu animasi kematian jika ada
        }

        // Nonaktifkan GameObject alih-alih menghancurkannya
        gameObject.SetActive(false);
    }
}