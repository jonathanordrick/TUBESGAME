using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class MeleeBoss : MonoBehaviour
{
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private float range = 1f;
    [SerializeField] private float colliderDistance = 0.5f;
    [SerializeField] private int damage = 1;
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private AudioSource attackAudioSource; // AudioSource untuk sound effect
    [SerializeField] private AudioClip meleeAttack1Sound; // Suara untuk MeleeAttack1
    [SerializeField] private AudioClip meleeAttack2Sound; // Suara untuk MeleeAttack2
    private float cooldownTimer = Mathf.Infinity;
    private bool isAttacking = false;
    private Animator anim;
    private PlayerHealth playerHealth;
    private EnemyPatrol enemyPatrol;
    private bool isUsingAttack1 = true;

    [SerializeField] private CinemachineImpulseSource impulseSource; // Tambahkan ini untuk screenshake

    private void Awake()
    {
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        enemyPatrol = GetComponent<EnemyPatrol>();

        if (anim == null || boxCollider == null || impulseSource == null || attackAudioSource == null)
        {
            Debug.LogError("Animator, BoxCollider2D, ImpulseSource, atau AudioSource tidak ditemukan!");
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
                if (attackAudioSource != null && meleeAttack1Sound != null)
                {
                    attackAudioSource.PlayOneShot(meleeAttack1Sound); // Putar suara MeleeAttack1
                }
            }
            else
            {
                anim.SetTrigger("MeleeAttack2");
                Debug.Log("Memulai MeleeAttack2 pada " + Time.time);
                if (attackAudioSource != null && meleeAttack2Sound != null)
                {
                    attackAudioSource.PlayOneShot(meleeAttack2Sound); // Putar suara MeleeAttack2
                }
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

            // Memicu screenshake saat pemain terkena damage
            if (impulseSource != null)
            {
                float randomX = Random.Range(-0.2f, 0.2f); // Variasi kecil di sumbu X
                float randomY = Random.Range(-0.5f, -0.3f); // Getaran ke bawah dengan variasi
                impulseSource.GenerateImpulse(new Vector3(randomX, randomY, 0));
                Debug.Log("Screenshake dipicu dengan vektor: " + new Vector3(randomX, randomY, 0));
            }
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
        cooldownTimer = 0; // Reset cooldown
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