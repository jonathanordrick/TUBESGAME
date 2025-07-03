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
            if (movement != null && !movement.IsUnderSpeedEffect()) // Cek apakah pemain sudah terkena efek
            {
                StartCoroutine(ApplySpeedBoost(movement));
            }

            Destroy(gameObject); // Hancurkan objek setelah diambil
        }
    }

    IEnumerator ApplySpeedBoost(PlayerMovement movement)
    {
        Debug.Log($"Applying speed boost. Original speed: {movement.speed}");
        float originalSpeed = movement.speed;

        movement.SetSpeedEffect(true); // Tandai bahwa pemain sedang terkena efek
        movement.speed *= speedBoostAmount; // Tingkatkan kecepatan

        yield return new WaitForSeconds(speedBoostDuration);

        movement.speed = originalSpeed; // Kembalikan ke kecepatan asli
        movement.SetSpeedEffect(false); // Tandai bahwa efek selesai
        Debug.Log($"Speed boost ended. Restored speed: {movement.speed}");
    }
}