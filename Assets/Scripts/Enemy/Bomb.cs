using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
   [SerializeField] private float speed;
   private bool hit;

   private Animator anim;
   private CapsuleCollider2D capsuleCollider;
   private Rigidbody2D rb;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (hit) return;
        float movementSpeed = speed * Time.deltaTime;
        transform.Translate(movementSpeed, 0, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            hit = true;
            rb.velocity = Vector2.zero;
            anim.SetTrigger("Explode");
            capsuleCollider.enabled = false; // Nonaktifkan collider saat meledak
            // Tambahkan logika untuk mengurangi nyawa pemain di sini
        }
        else if (collision.CompareTag("Ground"))
        {
            hit = true;
            rb.velocity = Vector2.zero;
            anim.SetTrigger("Explode");
            capsuleCollider.enabled = false; // Nonaktifkan collider saat meledak
        }
    }

    public void SetDirection(float _direction)
    {
        gameObject.SetActive(true);
        hit = false;
        capsuleCollider.enabled = true; // Aktifkan collider saat dilempar
        float localScaleX = transform.localScale.x;
        if (Mathf.Sign(localScaleX) != _direction)
            localScaleX *= -1; // Flip arah jika perlu
        transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);
    }
    
    private void Deactivate()
    {
        gameObject.SetActive(false); // Hapus bom saat keluar dari layar
    }
}
