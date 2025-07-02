using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombHolder : MonoBehaviour
{
    [SerializeField] private Transform enemy; // Referensi ke transform Enemy
 
    private void Update()
    {
        transform.localScale = enemy.localScale;
    }
}
