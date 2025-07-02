using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * Mathf.Sign(direction), transform.localScale.y, transform.localScale.z);
        rb.velocity = new Vector2(speed * direction, 0); // Gerakan awal dengan gravitasi
        Debug.Log("Bom diaktifkan dengan arah " + direction + " pada " + gameObject.name);
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
    }

    private void Explode()
    {
        if (anim != null)
        {
            anim.SetTrigger("Explode");
            Debug.Log("Animasi Explode dipicu pada " + gameObject.name);
        }
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        if (capsuleCollider != null) capsuleCollider.enabled = false;
        rb.velocity = Vector2.zero; // Hentikan gerakan
        Invoke("Deactivate", 0.5f);
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
        Debug.Log("Bom dinonaktifkan setelah ledakan pada " + gameObject.name);
    }
}