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
    public LayerMask enemyLayer; // Layer musuh
    public int damage = 1; // Damage yang diberikan
    

    public void Attack()
    {
        if (!isAttacking) // Hanya izinkan serangan jika tidak sedang menyerang
        {
            anim.SetBool("IsAttacking", true);
            isAttacking = true;
            StartCoroutine(ResetAttack());
            
        }
    }
    
    public void DealDamage()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(attackPoint.position, WeaponRange, enemyLayer);
        if (enemies.Length > 0)
        {
            enemies[0].GetComponent<EnemyHealth>().ChangeHealth(-damage); // Kurangi health musuh
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
            Gizmos.DrawWireSphere(attackPoint.position, WeaponRange); // Gambar area serangan
        }
    }
}