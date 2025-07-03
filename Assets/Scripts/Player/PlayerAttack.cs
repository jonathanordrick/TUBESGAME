using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Animator anim;
    public bool isAttacking = false; // Public untuk akses dari PlayerMovement
    private float attackCooldown = 0.55f; // Sesuaikan dengan durasi animasi Attack (0.3 detik + buffer kecil)
    public Transform attackPoint; // Titik serangan, bisa diatur di Inspector
    public float WeaponRange = 0.5f; // Jarak serangan
    public LayerMask enemyLayer; // Layer musuh + box
    public int damage; // Damage yang diberikan
    public PlayerMovement playerMovement; // Referensi ke PlayerMovement

    public void Attack()
    {
        if (!isAttacking && !playerMovement.isHurting) // Cek apakah tidak sedang hurt
        {
            Debug.Log("Attack dimulai! IsAttacking = true, IsHurting = " + playerMovement.isHurting);
            anim.SetBool("IsAttacking", true);
            isAttacking = true;

            DealDamage();

            StartCoroutine(ResetAttack());
        }
        else
        {
            Debug.Log("Attack diblokir! isAttacking = " + isAttacking + ", isHurting = " + playerMovement.isHurting);
        }
    }

    public void DealDamage()
    {
        Debug.Log("Menyerang...");

        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, WeaponRange, enemyLayer);

        if (hits.Length == 0)
        {
            Debug.Log("Tidak ada yang kena.");
        }
        else
        {
            if (hits.Length > 0)
            {
                Collider2D firstHit = hits[0];
                EnemyHealth enemyHealth = firstHit.GetComponent<EnemyHealth>();
                if (enemyHealth != null)
                {
                    Debug.Log("Musuh pertama terkena: " + firstHit.name);
                    enemyHealth.ChangeHealth(-damage);
                }
            }
        }

        foreach (Collider2D hit in hits)
        {
            Debug.Log("Kena: " + hit.name);

            Health enemy = hit.GetComponent<Health>();
            if (enemy != null)
            {
                Debug.Log("Musuh terkena (Health)!");
                enemy.TakeDamage(damage);
            }

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
        Debug.Log($"Mulai ResetAttack, menunggu {attackCooldown} detik");
        yield return new WaitForSeconds(attackCooldown);
        anim.SetBool("IsAttacking", false);
        isAttacking = false;
        Debug.Log("ResetAttack selesai, IsAttacking = false");
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, WeaponRange);
        }
    }
}