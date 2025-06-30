using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed = 5f; // Kecepatan mengejar, atur di Inspector
    [SerializeField] private float attackRange = 2f; // Jarak serangan, sinkron dengan MeleeEnemy
    private int facingDirection = -1; // 1 untuk kanan, -1 untuk kiri
    private EnemyState enemyState;
    private bool isChasing; // Status apakah musuh sedang mengejar pemain
    private Rigidbody2D rb;
    private Transform player;
    private Animator anim;
    private MeleeEnemy meleeEnemy; // Referensi ke MeleeEnemy

    void Start()
    {
        ChangeState(EnemyState.Idle);
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        meleeEnemy = GetComponent<MeleeEnemy>(); // Ambil komponen MeleeEnemy
        if (rb == null || anim == null || meleeEnemy == null)
        {
            Debug.LogError("Rigidbody2D, Animator, atau MeleeEnemy tidak ditemukan!");
        }
        meleeEnemy.enabled = false; // Matikan MeleeEnemy secara default
    }

    void FixedUpdate()
    {
        if (isChasing && player != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);

            // Flip arah berdasarkan posisi pemain selama mengejar
            Vector2 direction = (player.position - transform.position).normalized;
            if (direction.x > 0 && facingDirection == -1 || direction.x < 0 && facingDirection == 1)
            {
                Flip();
            }

            if (distanceToPlayer > attackRange)
            {
                // Mengejar dengan animasi berjalan
                Vector2 movement = new Vector2(direction.x * speed, rb.velocity.y);
                rb.velocity = movement;

                anim.SetBool("Moving", true); // Aktifkan animasi berjalan
                anim.SetBool("Idle", false); // Matikan animasi Idle
                ChangeState(EnemyState.Chase);
                meleeEnemy.enabled = false; // Matikan MeleeEnemy saat mengejar
            }
            else
            {
                // Hentikan gerakan dan aktifkan MeleeEnemy untuk serangan
                rb.velocity = Vector2.zero;
                anim.SetBool("Moving", false); // Matikan animasi berjalan
                ChangeState(EnemyState.Attack);
                meleeEnemy.enabled = true; // Aktifkan MeleeEnemy untuk serangan
            }
        }
        else
        {
            // Kembali ke Idle jika tidak mengejar
            ChangeState(EnemyState.Idle);
            rb.velocity = new Vector2(0, rb.velocity.y); // Hentikan gerakan horizontal
            anim.SetBool("Moving", false); // Matikan animasi berjalan
            anim.SetBool("Idle", true); // Aktifkan animasi Idle
            meleeEnemy.enabled = false; // Matikan MeleeEnemy saat Idle
        }
    }

    private void Flip()
    {
        facingDirection *= -1;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isChasing = true; // Setel isChasing ke true
            player = collision.transform;
            ChangeState(EnemyState.Chase);
            Debug.Log("Pemain masuk jangkauan pada " + Time.time + ", mulai mengejar!");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isChasing = false;
            ChangeState(EnemyState.Idle);
            player = null;
            rb.velocity = new Vector2(0, rb.velocity.y);
            Debug.Log("Pemain keluar jangkauan pada " + Time.time + ", berhenti mengejar!");
        }
    }

    void ChangeState(EnemyState newState)
    {
        enemyState = newState;
    }

    private void OnDrawGizmos()
    {
        if (player != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange); // Visualisasi jangkauan serangan
        }
    }

    public enum EnemyState
    {
        Idle,
        Patrol,
        Chase,
        Attack
    }
}