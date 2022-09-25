using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Sticky : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") || collision.CompareTag("Player2") || collision.CompareTag("Crate"))
        {
            collision.transform.SetParent(transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            UnParentPlayer1();
        }
        if (collision.CompareTag("Player2"))
        {
            UnParentPlayer2();
        }
        if (collision.CompareTag("Crate"))
        {
            UnParentCrate();
        }
    }

    public void UnParentAll()
    {
        foreach (Transform child in transform)
        {
            child.transform.SetParent(null);
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

    private void UnParentPlayer1()
    {
        foreach(Transform child in transform)
        {
            if (child.CompareTag("Player"))
            {
                child.transform.SetParent(null);
            }
        }
    }

    private void UnParentPlayer2()
    {
        foreach (Transform child in transform)
        {
            if (child.CompareTag("Player2"))
            {
                child.transform.SetParent(null);
            }
        }
    }

}
