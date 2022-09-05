using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    public float speed;
    public float stoppingDistance;
    public float awakeDistance;

    private Vector2 moveDirection;

    private Rigidbody2D rb;

    private Transform target;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //target = GameObject.FindGameObjectWithTag("Player1").GetComponent<Transform>();
    }

    void FixedUpdate()
    {
        MoveDirection();

        if (target != null)
        {
            if (Vector2.Distance(transform.position, target.position) > stoppingDistance && Vector2.Distance(transform.position, target.position) < awakeDistance)
            {
                transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            }
        }
    }

    private void MoveDirection()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(target == null)
        {
            if (collision.transform.CompareTag("Player1") || collision.transform.CompareTag("Player2"))
            {
                target = collision.transform;
            }
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        if (target != null)
        {
            if (Vector2.Distance(transform.position, target.position) > awakeDistance)
            {
                target = null;
            }
        }
        
    }
}
