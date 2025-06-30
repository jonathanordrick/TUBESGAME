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
    }

    // Update is called once per frame
    void Update()
    {
        if (isChasing == true)
        {
        Vector2 direction = (player.position - transform.position).normalized;
        rb.velocity = direction * speed;
        } 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if(player != null)
            {
                player = collision.transform; // Get the player's transform
            }
            isChasing = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            rb.velocity = Vector2.zero; // Stop moving when player exits trigger
            isChasing = false;
        }
    }
}
