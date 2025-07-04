using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float resetTime = 5f;
    [SerializeField] private int damage = 2;
    private float lifetime;
    private PlayerHealth playerHealth;
    private Animator anim;
    private Rigidbody2D rb;
    private CapsuleCollider2D capsuleCollider;
    public CinemachineImpulseSource impulseSource; // Impulse Source untuk screenshake

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        if (anim == null) Debug.LogError("Komponen Animator tidak ditemukan pada " + gameObject.name);
        if (rb == null) Debug.LogError("Komponen Rigidbody2D tidak ditemukan pada " + gameObject.name);
    }

    public void ActivateProjectile(float direction)
{
    lifetime = 0;
    gameObject.SetActive(true);

    // Hadap sesuai arah
    transform.localScale = new Vector3(
        Mathf.Abs(transform.localScale.x) * Mathf.Sign(direction), 
        transform.localScale.y, 
        transform.localScale.z
    );

    rb.velocity = new Vector2(speed * Mathf.Sign(direction), 0);
    Debug.Log("Bom diaktifkan ke arah " + direction);
}

    void Update()
    {
        if (!gameObject.activeInHierarchy) return;

        lifetime += Time.deltaTime;
        if (lifetime >= resetTime)
        {
            gameObject.SetActive(false);
            Debug.Log("Bom dinonaktifkan karena waktu hidup habis pada " + gameObject.name);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!gameObject.activeInHierarchy) return;

        if (collision.CompareTag("Player"))
        {
            playerHealth = collision.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.ChangeHealth(-damage);
                Debug.Log("Bom mengenai pemain, memberikan " + damage + " kerusakan pada " + Time.time);
            }
            else
            {
                Debug.LogWarning("Komponen PlayerHealth tidak ditemukan pada objek pemain.");
            }
            Explode();
        }
        else if (collision.CompareTag("Ground"))
        {
            Explode();
        }

        // Tambahkan variasi getaran alami
            float randomX = Random.Range(-0.2f, 0.2f); // Variasi kecil di sumbu X
            float randomY = Random.Range(-0.5f, -0.3f); // Getaran ke bawah dengan variasi
            impulseSource.GenerateImpulse(new Vector3(randomX, randomY, 0));
            Debug.Log("Screenshake dipicu dengan vektor: " + new Vector3(randomX, randomY, 0));
    }

    private void Explode()
{
    if (anim != null)
    {
        anim.SetTrigger("Explode");
        Debug.Log("Animasi Explode dipicu pada " + gameObject.name);
    }

    // Nonaktifkan collider agar tidak memicu tabrakan lagi
    if (capsuleCollider == null)
        capsuleCollider = GetComponent<CapsuleCollider2D>();
    if (capsuleCollider != null)
        capsuleCollider.enabled = false;

    // Hentikan gerakan
    rb.velocity = Vector2.zero;

    // 🛑 Matikan pengaruh gravitasi dan physics
    rb.gravityScale = 0;
    rb.isKinematic = true;  // agar physics system berhenti memprosesnya

    // Nonaktifkan setelah ledakan
    Invoke("Deactivate", 0.5f);
}

    private void Deactivate()
{
    // Reset agar bisa digunakan lagi
    if (capsuleCollider != null)
        capsuleCollider.enabled = true;

    rb.gravityScale = 0.5f; // atau nilai default kamu
    rb.isKinematic = false;

    gameObject.SetActive(false);
    Debug.Log("Bom dinonaktifkan setelah ledakan pada " + gameObject.name);
}
}