using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int currentHealth;
    public int maxHealth;
    public float deathAnimationDuration = 1f; // Durasi animasi kematian
    public float hurtAnimationDuration = 0.5f; // Durasi animasi hurt
    private Animator anim;
    private bool isHurtAnimating = false; // Flag untuk mencegah hurt berulang
    private EnemyMovement enemyMovement; // Referensi ke EnemyMovement

    private void Start()
    {
        currentHealth = maxHealth;
        gameObject.SetActive(true);
        anim = GetComponent<Animator>();
        enemyMovement = GetComponent<EnemyMovement>(); // Ambil komponen EnemyMovement
        if (anim == null)
        {
            Debug.LogError("Animator tidak ditemukan pada musuh: " + gameObject.name);
        }
        if (enemyMovement == null)
        {
            Debug.LogError("EnemyMovement tidak ditemukan pada musuh: " + gameObject.name);
        }
    }

    public void ChangeHealth(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        else if (currentHealth <= 0)
        {
            Die();
        }
        else if (amount < 0 && !isHurtAnimating) // Hanya picu jika menerima damage dan tidak sedang animasi Hurt
        {
            if (anim != null)
            {
                anim.SetTrigger("Hurt");
                Debug.Log("Memainkan animasi Hurt pada musuh: " + gameObject.name);
                StartCoroutine(ResetHurtAnimation());
            }
        }
        Debug.Log($"Nyawa musuh {gameObject.name} sekarang: {currentHealth}");
    }

    private void Die()
    {
        if (anim != null)
        {
            anim.SetBool("Moving", false);
            anim.SetTrigger("Die");
            Debug.Log("Memainkan animasi Die pada musuh: " + gameObject.name);
        }
        if (enemyMovement != null)
        {
            enemyMovement.Die(); // Panggil Die() di EnemyMovement untuk menghentikan pergerakan
        }
        StartCoroutine(DisableAfterDeath());
    }

    private IEnumerator DisableAfterDeath()
    {
        yield return new WaitForSeconds(deathAnimationDuration);
        gameObject.SetActive(false);
        Debug.Log("Musuh dinonaktifkan: " + gameObject.name);
    }

    private IEnumerator ResetHurtAnimation()
    {
        isHurtAnimating = true; // Tandai bahwa animasi Hurt sedang berjalan
        yield return new WaitForSeconds(hurtAnimationDuration); // Tunggu durasi animasi Hurt
        isHurtAnimating = false; // Reset flag setelah animasi selesai
    }
}