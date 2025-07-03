using UnityEngine;

public class HealthDrop : MonoBehaviour
{
    public int healAmount = 2; // Jumlah nyawa yang akan ditambahkan ke player

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player menyentuh item!");

            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                Debug.Log("Menambahkan nyawa ke player...");
                playerHealth.ChangeHealth(healAmount);
                Destroy(gameObject); // Hapus item dari scene
            }
        }
    }
}
