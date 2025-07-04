using System.Collections;
using UnityEngine;

public class PoisonEffect : MonoBehaviour
{
    public float speedBoostDuration = 3f; // Lama efek percepatan
    public float speedBoostAmount = 1.5f; // Persentase percepatan (1.5 = 150%)

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMovement movement = other.GetComponent<PlayerMovement>();
            if (movement != null)
            {
                movement.ApplyOrRefreshSpeedBoost(speedBoostAmount, speedBoostDuration);
            }
            Destroy(gameObject); // Hancurkan objek setelah diambil
        }
    }
}