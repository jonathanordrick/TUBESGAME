using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float StartingHealth;
    public float currentHealth { get; private set; }
    private Animator anim;
    private bool isDead;

    private void Awake()
    {
        currentHealth = StartingHealth;
        anim = GetComponent<Animator>();
    }

    public void TakeDamage(float damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, StartingHealth);
        if (currentHealth > 0)
        {
            anim.SetTrigger("hurt");
        }
        else
            anim.SetTrigger("die");
            if (!isDead)
            {
                anim.SetTrigger("die");
                GetComponent<PlayerMovement>().enabled = false; // Nonaktifkan gerakan player
                isDead = true;
            }
        }
}
