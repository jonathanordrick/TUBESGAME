using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Enemy Patrol Points")]
    [SerializeField] private Transform BatasKiri;
    [SerializeField] private Transform BatasKanan;

    [Header("Enemy")]
    [SerializeField] private Transform enemy;

    [Header("Enemy Movement")]
    [SerializeField] private float speed;
    private bool movingLeft;
    private Vector3 initScale;

    [Header("Enemy Animator")]
    [SerializeField] private Animator anim;

    [Header("Idle Behaviour")]
    [SerializeField] private float idleDuration;
    private float idleTimer;

    [Header("Melee Enemy Reference")]
    [SerializeField] private MeleeEnemy meleeEnemy; // Referensi ke MeleeEnemy

    private void Awake()
    {
        initScale = enemy.localScale;
        if (meleeEnemy == null)
        {
            meleeEnemy = GetComponent<MeleeEnemy>(); // Coba ambil dari komponen yang sama
            if (meleeEnemy == null) Debug.LogError("MeleeEnemy tidak ditemukan pada " + gameObject.name);
        }
    }

    private void OnDisable()
    {
        anim.SetBool("Moving", false);
    }

    private void Update()
    {
        // Cek apakah player terdeteksi oleh MeleeEnemy
        if (meleeEnemy != null && meleeEnemy.PlayerInSight())
        {
            anim.SetBool("Moving", false); // Hentikan moving jika player terdeteksi
            return; // Keluar dari Update untuk menghentikan patrol
        }

        if (movingLeft)
        {
            if (enemy.position.x >= BatasKiri.position.x)
            {
                MoveInDirection(-1);
            }
            else
            {
                DirectionChange();
            }
        }
        else
        {
            if (enemy.position.x <= BatasKanan.position.x)
            {
                MoveInDirection(1);
            }
            else
            {
                DirectionChange();
            }
        }
    }

    private void DirectionChange()
    {
        anim.SetBool("Moving", false);
        idleTimer += Time.deltaTime;

        if (idleTimer > idleDuration)
        {
            movingLeft = !movingLeft;
            idleTimer = 0; // Reset timer setelah ganti arah
        }
    }

    private void MoveInDirection(int _direction)
    {
        idleTimer = 0;
        anim.SetBool("Moving", true);

        // Pastikan arah gerak sesuai dengan skala
        enemy.localScale = new Vector3(Mathf.Abs(initScale.x) * -_direction, initScale.y, initScale.z); // Invert _direction
        enemy.position = new Vector3(enemy.position.x + Time.deltaTime * _direction * speed, enemy.position.y, enemy.position.z);
    }
}