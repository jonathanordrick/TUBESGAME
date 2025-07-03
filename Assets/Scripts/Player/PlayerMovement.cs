using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 5f;
    public Rigidbody2D rb;
    public Animator anim;
    public int facingDirection = 1;
    public PlayerAttack playerAttack;
    public Transform groundCheck;
    public float groundCheckRadius = 0.1f;
    public LayerMask groundLayer;
    public int maxJumps = 2;
    private int jumpCount = 0;
    private bool isGrounded;
    private bool jumpPressed = false;
    public bool isHurting = false;
    private bool isUnderSpeedEffect = false; // Menandakan apakah sedang terkena efek percepatan

    public bool IsUnderSpeedEffect()
    {
        return isUnderSpeedEffect;
    }

    public void SetSpeedEffect(bool state)
    {
        isUnderSpeedEffect = state;
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");

        // Cek apakah menyentuh tanah
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        anim.SetBool("Grounded", !isGrounded);
        if (isGrounded)
        {
            jumpCount = 0;
        }

        // Jika Space ditekan dan masih punya jatah lompatan
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount < maxJumps)
        {
            jumpPressed = true;
            jumpCount++;
        }

        // Serangan
        if (Input.GetButtonDown("Attack"))
        {
            Debug.Log("Input Attack terdeteksi! isAttacking = " + (playerAttack != null ? playerAttack.isAttacking.ToString() : "null") + ", isHurting = " + isHurting);
            if (!playerAttack.isAttacking && !isHurting)
            {
                Debug.Log("Tombol Attack ditekan! Memanggil playerAttack.Attack()");
                playerAttack.Attack();
            }
        }

        // Animasi jalan
        anim.SetFloat("Horizontal", Mathf.Abs(horizontal));

        
        // Flip arah
        if (horizontal > 0 && transform.localScale.x < 0 ||
            horizontal < 0 && transform.localScale.x > 0)
            
        {
            Flip();
        }
    }

    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);

        if (jumpPressed)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpPressed = false;
        }
    }

    void Flip()
    {
        facingDirection *= -1;
        transform.localScale = new Vector3(
            transform.localScale.x * -1,
            transform.localScale.y,
            transform.localScale.z
        );
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}