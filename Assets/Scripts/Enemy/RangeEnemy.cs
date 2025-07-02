using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeEnemy : MonoBehaviour
{
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private float range = 5f;
    [SerializeField] private float detectionHeight = 1f;
    [SerializeField] private float colliderDistance = 1f;
    [SerializeField] private int damage = 1;
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private Transform firepoint;
    [SerializeField] private GameObject[] Bomb;
    private float cooldownTimer = Mathf.Infinity;
    private bool isAttacking = false;
    private Animator anim;
    private PlayerHealth playerHealth;
    private Transform player;
    private int facingDirection = -1;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        if (anim == null) Debug.LogError("Komponen Animator tidak ditemukan pada " + gameObject.name);
        if (boxCollider == null) Debug.LogError("Komponen BoxCollider2D tidak ditemukan pada " + gameObject.name);
        if (firepoint == null) Debug.LogError("Firepoint tidak diatur pada " + gameObject.name);
        if (Bomb == null || Bomb.Length == 0) Debug.LogError("Array Bomb kosong atau tidak diatur pada " + gameObject.name);
        else
        {
            for (int i = 0; i < Bomb.Length; i++)
            {
                if (Bomb[i] == null) Debug.LogError("Bom pada indeks " + i + " null pada " + gameObject.name);
                else if (Bomb[i].GetComponent<EnemyProjectile>() == null) Debug.LogError("Bom pada indeks " + i + " tidak memiliki skrip EnemyProjectile pada " + gameObject.name);
            }
        }
    }

    private void Update()
    {
        if (anim == null || boxCollider == null) return;

        cooldownTimer += Time.deltaTime;
        Debug.Log("Update: cooldownTimer = " + cooldownTimer + ", isAttacking = " + isAttacking + ", PlayerInSight = " + PlayerInSight());

        if (PlayerInSight())
        {
            if (player != null)
            {
                Vector2 direction = (player.position - transform.position).normalized;
                if (direction.x > 0 && facingDirection == -1 || direction.x < 0 && facingDirection == 1)
                {
                    Flip();
                }
            }

            if (!isAttacking && cooldownTimer >= attackCooldown)
            {
                cooldownTimer = 0;
                anim.ResetTrigger("RangedAttack");
                anim.SetTrigger("RangedAttack");
                isAttacking = true;
                Debug.Log("Memulai trigger RangedAttack pada " + gameObject.name + ", isAttacking: " + isAttacking + ", anim state: " + anim.GetCurrentAnimatorStateInfo(0).IsName("throw"));
            }
        }
        // Fallback jika animation event gagal
        if (isAttacking && anim.GetCurrentAnimatorStateInfo(0).IsName("throw") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.5f) // Sesuaikan dengan durasi animasi
        {
            EndAttack();
        }
    }

    private void RangedAttack()
    {
        if (firepoint == null)
        {
            Debug.LogError("Firepoint tidak diatur pada " + gameObject.name);
            return;
        }
        if (Bomb == null || Bomb.Length == 0)
        {
            Debug.LogError("Array Bomb kosong atau tidak diatur pada " + gameObject.name);
            return;
        }
        int bombIndex = FindBomb();
        Debug.Log("Mencoba menggunakan bom pada indeks " + bombIndex + ", active: " + (Bomb[bombIndex] != null && Bomb[bombIndex].activeInHierarchy));
        if (Bomb[bombIndex] == null)
        {
            Debug.LogError("Bom pada indeks " + bombIndex + " null pada " + gameObject.name);
            return;
        }
        EnemyProjectile projectile = Bomb[bombIndex].GetComponent<EnemyProjectile>();
        if (projectile == null)
        {
            Debug.LogError("Bom pada indeks " + bombIndex + " tidak memiliki skrip EnemyProjectile pada " + gameObject.name);
            return;
        }

        Debug.Log("Mengeluarkan bom pada indeks " + bombIndex + " dari posisi " + firepoint.position);
        Bomb[bombIndex].transform.position = firepoint.position;
        if (player != null)
        {
            float direction = Mathf.Sign(player.position.x - transform.position.x);
            projectile.ActivateProjectile(direction);
        }
        else
        {
            projectile.ActivateProjectile(facingDirection);
        }
    }

    private void EndAttack()
    {
        isAttacking = false;
        Debug.Log("Mengakhiri serangan pada " + gameObject.name + ", isAttacking diatur ke " + isAttacking + " pada " + Time.time);
    }

    private int FindBomb()
    {
        for (int i = 0; i < Bomb.Length; i++)
        {
            if (Bomb[i] != null && !Bomb[i].activeInHierarchy)
            {
                return i;
            }
        }
        Debug.LogWarning("Tidak ada bom yang tersedia pada " + gameObject.name + ". Mencoba menggunakan indeks 0 pada " + Time.time);
        return 0;
    }

    public bool PlayerInSight()
    {
        if (boxCollider == null) return false;

        RaycastHit2D hit = Physics2D.BoxCast(
            boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, detectionHeight, boxCollider.bounds.size.z),
            0, Vector2.left, 0, playerLayer);

        if (hit.collider != null)
        {
            player = hit.transform;
            playerHealth = hit.transform.GetComponent<PlayerHealth>();
            if (playerHealth == null)
            {
                Debug.LogWarning("Komponen PlayerHealth tidak ditemukan pada " + hit.collider.gameObject.name);
            }
            else
            {
                Debug.Log("Pemain terdeteksi pada posisi " + hit.collider.transform.position + " pada " + Time.time);
            }
        }
        else
        {
            player = null;
        }

        return hit.collider != null;
    }

    private void Flip()
    {
        facingDirection *= -1;
        Vector3 newScale = transform.localScale;
        newScale.x *= -1;
        transform.localScale = newScale;
        if (firepoint != null) firepoint.localPosition = new Vector3(-firepoint.localPosition.x, firepoint.localPosition.y, firepoint.localPosition.z);
        Debug.Log("Musuh flip ke arah " + facingDirection + " pada " + gameObject.name + " pada " + Time.time);
    }

    private void OnDrawGizmos()
    {
        if (boxCollider == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(
            boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, detectionHeight, boxCollider.bounds.size.z)
        );
    }
}