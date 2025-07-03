using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Animator anim;
    public bool isAttacking = false; // Public untuk akses dari PlayerMovement
    private float attackCooldown = 2f; // Sinkron dengan durasi animasi
    public Transform attackPoint; // Titik serangan, bisa diatur di Inspector
    public float WeaponRange = 0.5f; // Jarak serangan
    public LayerMask enemyLayer; // Layer musuh + box
    public int damage = 1; // Damage yang diberikan

    public void Attack()
    {
        if (!isAttacking) // Hanya izinkan serangan jika tidak sedang menyerang
        {
            anim.SetBool("IsAttacking", true);
            isAttacking = true;

            DealDamage(); // ✅ langsung panggil serangan saat tombol ditekan

            StartCoroutine(ResetAttack());
        }
    }

    public void DealDamage()
    {
        Debug.Log("Menyerang...");

        // Deteksi semua objek dalam jangkauan attackPoint
        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, WeaponRange, enemyLayer);

        if (hits.Length == 0)
        {
            Debug.Log("Tidak ada yang kena.");
        }

        foreach (Collider2D hit in hits)
        {
            Debug.Log("Kena: " + hit.name);

            // Coba serang musuh (Health)
            Health enemy = hit.GetComponent<Health>();
            if (enemy != null)
            {
                Debug.Log("Musuh terkena!");
                enemy.TakeDamage(damage);
            }

            // Coba hancurkan box (BreakableBox)
            BreakableBox box = hit.GetComponent<BreakableBox>();
            if (box != null)
            {
                Debug.Log("Box terkena!");
                box.TakeDamage(damage);
            }
        }
    }

    IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(attackCooldown); // Tunggu hingga animasi selesai
        anim.SetBool("IsAttacking", false);
        isAttacking = false; // Reset flag setelah animasi selesai
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, WeaponRange); // Gambar area serangan di editor
        }
    }
}
