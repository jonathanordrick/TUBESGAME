using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private bool isActivated = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isActivated && collision.CompareTag("Player"))
        {
            var respawnSystem = collision.GetComponent<RespawnSystem>();
            if (respawnSystem != null)
            {
                respawnSystem.UpdateCheckpoint(transform.position);
                isActivated = true;
                Debug.Log("Checkpoint activated at: " + transform.position);
            }
        }
    }
}
