using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sticky : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player1") || collision.CompareTag("Player2"))
        {
            collision.transform.SetParent(transform);
        }
        
        if(collision.CompareTag("Object"))
        {
            collision.transform.parent.SetParent(transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player1") || collision.CompareTag("Player2"))
        {
            collision.transform.SetParent(null);
        }
        
        if (collision.CompareTag("Object"))
        {
            collision.transform.parent.SetParent(null);
        }
    }

    private void UnParentPlayer()
    {
        foreach(Transform child in transform)
        {
            if (child.CompareTag("Player1") || child.CompareTag("Player2"))
            {
                child.transform.SetParent(null);
            }
        }
    }
}
