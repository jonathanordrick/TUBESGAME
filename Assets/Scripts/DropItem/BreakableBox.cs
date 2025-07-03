using UnityEngine;

public class BreakableBox : MonoBehaviour
{
    public GameObject heartPrefab;         // ❤️ Tambah nyawa
    public GameObject poisonSpeedPrefab;   // ☠️ Kurangi/mempercepat speed
    public GameObject poisonAttackPrefab;  // 💥 Tambah attack sementara
    public int boxHealth = 1;

    public void TakeDamage(int amount)
    {
        boxHealth -= amount;

        if (boxHealth <= 0)
        {
            BreakBox();
        }
    }

    private void BreakBox()
    {
        DropRandomItem();
        Destroy(gameObject);
    }

    private void DropRandomItem()
    {
        float roll = Random.value; // angka antara 0.0 dan 1.0

        if (roll < 0.33f && heartPrefab != null)
        {
            Instantiate(heartPrefab, transform.position, Quaternion.identity);
            Debug.Log("Drop: Heart");
        }
        else if (roll < 0.66f && poisonSpeedPrefab != null)
        {
            Instantiate(poisonSpeedPrefab, transform.position, Quaternion.identity);
            Debug.Log("Drop: Poison Speed");
        }
        else if (poisonAttackPrefab != null)
        {
            Instantiate(poisonAttackPrefab, transform.position, Quaternion.identity);
            Debug.Log("Drop: Poison Attack");
        }
    }
}
