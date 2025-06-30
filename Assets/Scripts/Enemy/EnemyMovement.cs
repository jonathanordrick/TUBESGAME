using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed; // Speed of the enemy
    private bool isChasing;

    private Rigidbody2D rb;
    private Transform player;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        // Validasi component
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D not found on " + gameObject.name);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Cek apakah sedang chasing dan player reference valid
        if (isChasing && player != null && rb != null)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            rb.velocity = direction * speed;
        }
        else if (isChasing && player == null)
        {
            // Jika player reference hilang, stop chasing
            isChasing = false;
            if (rb != null)
            {
                rb.velocity = Vector2.zero;
            }
            Debug.LogWarning("Player reference lost, stopping chase");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player = collision.transform; // Set player reference
            isChasing = true;
            Debug.Log("Enemy started chasing player");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            rb.velocity = Vector2.zero; // Stop moving when player exits trigger
            isChasing = false;
            player = null; // Clear player reference
            Debug.Log("Enemy stopped chasing player");
        }
    }
}
