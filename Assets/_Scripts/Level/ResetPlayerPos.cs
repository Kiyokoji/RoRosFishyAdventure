using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPlayerPos : MonoBehaviour
{
    public Transform respawnPos;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Object"))
        {
            collision.transform.parent.position = respawnPos.position;
            collision.GetComponentInParent<Rigidbody2D>().velocity = Vector2.zero;
        }
        
        if (collision.CompareTag("Player1") || collision.CompareTag("Player2"))
        {
            collision.transform.position = respawnPos.position;
            collision.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
            
    }
}
