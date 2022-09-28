using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchNew : MonoBehaviour
{
    private bool active;

    public float speed = 2f;

    public List<Activatable> activatableList;

    public bool weighted = false;
    private bool toggled;

    public GameObject waypoint1;
    public GameObject waypoint2;


    void Start()
    {

    }

    private void FixedUpdate()
    {
        if (active)
        {
            transform.position = Vector2.MoveTowards(transform.position, waypoint2.transform.position, speed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, waypoint1.transform.position, speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!weighted)
        {
            if (!toggled && (collision.CompareTag("Player1") || collision.CompareTag("Player2") || collision.CompareTag("Crate")))
            {
                ActivateSwitch();
                toggled = true;
            }
            else if (toggled && (collision.CompareTag("Player1") || collision.CompareTag("Player2") || collision.CompareTag("Crate")))
            {
                DeactivateSwitch();
                toggled = false;
            }
        }
        else
        {
            if (collision.CompareTag("Player1") || collision.CompareTag("Player2") || collision.CompareTag("Crate"))
            {
                ActivateSwitch();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // || collision.CompareTag("Crate")
        if (collision.CompareTag("Player1") || collision.CompareTag("Player2"))
        {

            if (weighted)
            {
                StartCoroutine(Cooldown());
                
            }
        }
    }

    private IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(0.5f);
        DeactivateSwitch();
    }
    
    private void ActivateSwitch()
    {
        active = true;
        
        foreach (var a in activatableList)
        {
            a.isActive = true;
        }
    }
    
    private void DeactivateSwitch()
    {
        active = false;

        foreach (var a in activatableList)
        {
            a.isActive = false;
        }
    }
}

