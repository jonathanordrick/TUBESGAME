using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeBoss : MonoBehaviour
{
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private float range = 1f;
    [SerializeField] private float colliderDistance = 0.5f;
    [SerializeField] private int damage = 1;
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private LayerMask playerLayer;
    private float cooldownTimer = Mathf.Infinity;
    private bool isAttacking = false;
    private Animator anim;
    private PlayerHealth playerHealth;
    private EnemyPatrol enemyPatrol;
    private bool isUsingAttack1 = true;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        enemyPatrol = GetComponent<EnemyPatrol>();

        if (anim == null || boxCollider == null)
        {
            Debug.LogError("Animator atau BoxCollider2D tidak ditemukan!");
        }
    }

    private void Update()
    {
        cooldownTimer += Time.deltaTime;

        // Debugging untuk melacak status
        Debug.Log($"Status: PlayerInSight={PlayerInSight()}, isAttacking={isAttacking}, cooldownTimer={cooldownTimer}, isUsingAttack1={isUsingAttack1}, AnimatorState={anim.GetCurrentAnimatorStateInfo(0).fullPathHash}");

        if (PlayerInSight() && !isAttacking && cooldownTimer >= attackCooldown && !anim.GetBool("IsHurt"))
        {
            cooldownTimer = 0;
            isAttacking = true;
            anim.SetBool("Moving", false);
            anim.ResetTrigger("MeleeAttack1");
            anim.ResetTrigger("MeleeAttack2");

            if (isUsingAttack1)
            {
                anim.SetTrigger("MeleeAttack1");
                Debug.Log("Memulai MeleeAttack1 pada " + Time.time);
            }
            else
            {
                anim.SetTrigger("MeleeAttack2");
                Debug.Log("Memulai MeleeAttack2 pada " + Time.time);
            }
        }

        if (enemyPatrol != null)
        {
            enemyPatrol.enabled = !PlayerInSight() && !anim.GetBool("IsHurt");
        }
    }

    public bool PlayerInSight()
    {
        RaycastHit2D hit = Physics2D.BoxCast(
            boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0, Vector2.left, 0, playerLayer);

        if (hit.collider != null)
        {
            playerHealth = hit.transform.GetComponent<PlayerHealth>();
        }
        else
        {
            playerHealth = null;
        }

        return hit.collider != null;
    }

    public void DamagePlayer()
    {
        if (playerHealth != null && PlayerInSight())
        {
            playerHealth.ChangeHealth(-damage);
            Debug.Log("Serangan ke player: " + damage + " damage pada " + Time.time);
        }
        else
        {
            Debug.LogWarning("playerHealth null atau pemain tidak dalam jangkauan saat DamagePlayer dipanggil.");
        }
    }

    public void EndAttack()
    {
        isAttacking = false;
        isUsingAttack1 = !isUsingAttack1; // Ganti ke serangan berikutnya
        Debug.Log("Mengakhiri serangan, beralih ke " + (isUsingAttack1 ? "MeleeAttack1" : "MeleeAttack2") + " pada " + Time.time);
    }

    public void OnHurt()
    {
        anim.SetBool("IsHurt", true);
        isAttacking = false; // Reset serangan saat Hurt
        cooldownTimer = 0; // Reset cooldown ifdef
        if (enemyPatrol != null)
        {
            enemyPatrol.enabled = false; // Hentikan patroli saat Hurt
        }
        Debug.Log("Boss terkena Hurt pada " + Time.time);
    }

    public void EndHurt()
    {
        anim.SetBool("IsHurt", false);
        isAttacking = false;
        if (enemyPatrol != null)
        {
            enemyPatrol.enabled = !PlayerInSight(); // Kembalikan patroli jika perlu
        }
        Debug.Log("Animasi Hurt selesai pada " + Time.time);
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