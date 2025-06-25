using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Animator anim;
    public void Attack()
    {
        anim.SetBool("IsAttacking", true);
        StartCoroutine(ResetAttack());
    }

    IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(2f); // durasi animasi serang
        anim.SetBool("IsAttacking", false);
    }
}