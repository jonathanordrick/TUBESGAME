using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header ("Enemy Patrol Points")]
    [SerializeField] private Transform BatasKiri;
    [SerializeField] private Transform BatasKanan;

    [Header ("Enemy")]
    [SerializeField] private Transform enemy;

    [Header ("Enemy Movement")]
    [SerializeField] private float speed;
    private bool movingleft;
    private Vector3 initScale;

    [Header ("Enemy Animator")]
    [SerializeField]private Animator anim;

    private void Update()
{
    if (anim.GetCurrentAnimatorStateInfo(0).IsName("MeleeAttack"))
    {
        anim.SetBool("Moving", false);
        return;
    }

    if (movingleft)
    {
        if (enemy.position.x >= BatasKiri.position.x)
            MoveInDirection(-1);
        else
            DirectionChange();
    }
    else
    {
        if (enemy.position.x <= BatasKanan.position.x)
            MoveInDirection(1);
        else
            DirectionChange();
    }
}

    private void DirectionChange()
    {
        anim.SetBool("Moving", false);
        movingleft = !movingleft;
        MoveInDirection(movingleft ? -1 : 1);
    }

    private void Awake()
    {
        initScale = enemy.localScale;
    }

    private void MoveInDirection(int _direction)
    {
        anim.SetBool("Moving", true);   
        enemy.localScale = new Vector3(Mathf.Abs(initScale.x) * -_direction, initScale.y, initScale.z);
        enemy.position = new Vector3(enemy.position.x + Time.deltaTime * _direction * speed,
            enemy.position.y, enemy.position.z);
    }
}
