using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sticky : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") || collision.CompareTag("Crate"))
        {
            collision.transform.SetParent(transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            UnParentPlayer();
        }
    }

    private void UnParentCrate()
    {
        foreach (Transform child in transform)
        {
            if (child.CompareTag("Crate"))
            {
                child.transform.SetParent(null);
            }
        }
    }

    private void UnParentPlayer()
    {
        foreach(Transform child in transform)
        {
            if (child.CompareTag("Player"))
            {
                child.transform.SetParent(null);
            }
        }
    }
}
