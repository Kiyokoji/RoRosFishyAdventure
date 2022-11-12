using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPlayerPos : MonoBehaviour
{
    public Transform respawnPos;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        collision.transform.position = respawnPos.position;
        collision.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }
}
