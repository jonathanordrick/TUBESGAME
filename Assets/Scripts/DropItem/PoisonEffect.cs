using System.Collections;
using UnityEngine;

public class PoisonEffect : MonoBehaviour
{
    public float slowDuration = 3f; // Lama efek racun
    public float slowAmount = 0.5f; // Persentase pelambatan (0.5 = 50%)

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMovement movement = other.GetComponent<PlayerMovement>();
            if (movement != null)
            {
                StartCoroutine(ApplyPoison(movement));
            }

            Destroy(gameObject); // Hancurkan objek racun setelah diambil
        }
    }

    IEnumerator ApplyPoison(PlayerMovement movement)
    {
        float originalSpeed = movement.speed;

        movement.speed *= slowAmount; // Percepat atau perlambat

        yield return new WaitForSeconds(slowDuration);

        movement.speed = originalSpeed; // Kembalikan ke normal
    }
}
