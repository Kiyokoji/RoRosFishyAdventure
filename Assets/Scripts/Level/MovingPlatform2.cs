using System;
using System.Collections;
using System.Collections.Generic;
using Level;
using UnityEngine;
using Photon.Pun;

public class MovingPlatform2 : Activatable
{
    public bool isElevator = false;

    private Vector2 direction;

    public int id;
    private bool active = false;
    [SerializeField] private bool alwaysMoving = false;
    private PhotonView view;

    [SerializeField] private GameObject[] waypoints;

    private int currentWaypointIndex = 0;
    [SerializeField] private float speed = 2f;

    private Rigidbody2D rb;

    private void Start()
    {
        view = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody2D>();

        direction = Vector2.zero;
    }

    private void FixedUpdate()
    {
        if (isElevator)
        {
            if (active || isActive)
            {
                MoveUp();
            } else
            {
                MoveDown();
            }
        }

        if (!isElevator)
        {
            if (alwaysMoving)
            {
                active = true;
                SimpleMove();
            } else if (active || isActive)
            {
                SimpleMove();
            }      
        }
    }

    private void SimpleMove()
    {
        if (Vector2.Distance(waypoints[currentWaypointIndex].transform.position, transform.position) < .1f)
        {
            currentWaypointIndex++;
            if (currentWaypointIndex >= waypoints.Length)
            {
                currentWaypointIndex = 0;
            }
        }

        Vector2 newPosition = Vector2.MoveTowards(transform.position, waypoints[currentWaypointIndex].transform.position, speed * Time.deltaTime);
        rb.MovePosition(newPosition);

        //rb.MovePosition(waypoints[currentWaypointIndex].transform.position);
        //transform.position = Vector2.MoveTowards(transform.position, waypoints[currentWaypointIndex].transform.position, speed * Time.deltaTime);
    }

    public void MoveUp()
    {
        Vector2 newPosition = Vector2.MoveTowards(transform.position, waypoints[0].transform.position, speed * Time.deltaTime);
        rb.MovePosition(newPosition);

        //rb.MovePosition(waypoints[0].transform.position);

        //transform.position = Vector2.MoveTowards(transform.position, waypoints[0].transform.position, speed * Time.deltaTime);
    }

    public void MoveDown()
    {
        Vector2 newPosition = Vector2.MoveTowards(transform.position, waypoints[1].transform.position, speed * Time.deltaTime);
        rb.MovePosition(newPosition);

        //rb.MovePosition(waypoints[1].transform.position * speed * Time.deltaTime);

        //transform.position = Vector2.MoveTowards(transform.position, waypoints[1].transform.position, speed * Time.deltaTime);
    }

    [PunRPC]
    public void Activate()
    {
        active = true;
    }

    [PunRPC]
    public void Deactivate()
    {
        active = false;
    }
}




