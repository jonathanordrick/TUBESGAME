using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    [SerializeField] private float range;
    [SerializeField] private float colliderDistance;
    [SerializeField] private int damage = 1;
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private LayerMask playerLayer;
    private float cooldownTimer = Mathf.Infinity;

    private Animator anim;
    private PlayerHealth playerHealth;
    private EnemyPatrol enemyPatrol;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Hanya proses jika yang collision adalah player, bukan ground atau objek lain
        if (!collision.collider.CompareTag("Player")) return;
        
        // Cek apakah cooldown sudah selesai dan player masih hidup
        if (cooldownTimer >= attackCooldown)
        {
            PlayerHealth playerHealth = collision.collider.GetComponent<PlayerHealth>();
            if (playerHealth != null && playerHealth.IsAlive())
            {
                playerHealth.ChangeHealth(-damage);
                anim.SetTrigger("MeleeAttack");
                cooldownTimer = 0; // Reset cooldown setelah attack
                Debug.Log("Enemy attacked player for " + damage + " damage via collision");
            }
        }
    }

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        enemyPatrol = GetComponent<EnemyPatrol>();

        if (anim.GetCurrentAnimatorStateInfo(0).IsTag("MeleeAttack"))
        {
            anim.SetBool("Moving", false);
            return;
        }
    }

    private void Update()
    {
        cooldownTimer += Time.deltaTime;

        if (PlayerInSight())
        {
            if (cooldownTimer >= attackCooldown)
            {
                cooldownTimer = 0;
                anim.SetBool("Moving", false); // Stop animasi jalan dulu
                anim.ResetTrigger("MeleeAttack"); // Hindari trigger dobel
                anim.SetTrigger("MeleeAttack");
            }
        }
        if (enemyPatrol != null)
        {
            enemyPatrol.enabled = !PlayerInSight(); // Aktifkan patrol jika player tidak terdeteksi
        }
    }

    public bool PlayerInSight()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0, Vector2.left, 0, playerLayer);

        if (hit.collider != null)
            playerHealth = hit.transform.GetComponent<PlayerHealth>();

        return hit.collider != null;
    }

    public void DamagePlayer()
    {
        // Method ini dipanggil dari Animation Event saat serangan
        if (playerHealth != null && playerHealth.IsAlive())
        {
            playerHealth.ChangeHealth(-damage);
            Debug.Log("Enemy attacked player for " + damage + " damage via animation event at " + Time.time);
        }
        else
        {
            // Cari player lagi jika reference hilang
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                PlayerHealth health = player.GetComponent<PlayerHealth>();
                if (health != null && health.IsAlive())
                {
                    health.ChangeHealth(-damage);
                    Debug.Log("Enemy attacked player for " + damage + " damage (found player again)");
                }
            }
            else
            {
                Debug.LogWarning("Player not found when DamagePlayer called");
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (boxCollider == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(
            boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z)
        );
    }
}