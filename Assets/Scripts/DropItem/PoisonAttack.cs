using UnityEngine;

public class PoisonAttack : MonoBehaviour
{
    public float buffDuration = 3f;    // Lama efek dalam detik
    public int bonusDamage = 2;        // Tambahan attack sementara

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerAttack playerAttack = other.GetComponent<PlayerAttack>();

            if (playerAttack != null)
            {
                Debug.Log("Poison Attack activated!");
                StartCoroutine(BoostAttack(playerAttack));
            }

            Destroy(gameObject); // Hapus objek setelah disentuh
        }
    }

    private System.Collections.IEnumerator BoostAttack(PlayerAttack playerAttack)
    {
        int originalDamage = playerAttack.damage;
        playerAttack.damage += bonusDamage;

        Debug.Log("Attack buff aktif! Damage sekarang: " + playerAttack.damage);

        yield return new WaitForSeconds(buffDuration);

        playerAttack.damage = originalDamage;
        Debug.Log("Attack kembali normal: " + playerAttack.damage);
    }
}
