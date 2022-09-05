using System;
using System.Collections;
using System.Collections.Generic;
using Level;
using UnityEngine;
using Photon.Pun;

public class MovingPlatform : Activatable
{
    public bool isElevator = false;

    private Rigidbody2D rb;
    private bool active = false;
    [SerializeField] private bool alwaysMoving = false;
    private PhotonView view;

    [SerializeField] private GameObject[] waypoints;

    private int currentWaypointIndex = 0;
    [SerializeField] private float speed = 2f;

    private void Start()
    {
        view = GetComponent<PhotonView>();

        rb = GetComponent<Rigidbody2D>();
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

        transform.position = Vector2.MoveTowards(transform.position, waypoints[currentWaypointIndex].transform.position, speed * Time.deltaTime);
    }

    public void MoveUp()
    {
        transform.position = Vector2.MoveTowards(transform.position, waypoints[0].transform.position, speed * Time.deltaTime);
    }

    public void MoveDown()
    {
        transform.position = Vector2.MoveTowards(transform.position, waypoints[1].transform.position, speed * Time.deltaTime);
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

    public IEnumerator Wait()
    {
        yield return new WaitForSeconds(1);
        FindObjectOfType<AudioManager>().Play("GateThud");
    }
}




