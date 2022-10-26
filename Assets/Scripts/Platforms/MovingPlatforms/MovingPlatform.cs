using System;
using System.Collections;
using UnityEngine;

public class MovingPlatform : Activatable
{
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private float speed = 2f;
    [SerializeField] private float checkDistance = 0.05f;
    [SerializeField] private bool loop;
    
    public bool isElevator;

    private Transform targetWaypoint;
    private int currentWaypointIndex = 0;
    
    private bool active;
    
    //[SerializeField] private GameObject[] waypoints;
    [SerializeField] private Transform platform;

    private void Start()
    {
        targetWaypoint = waypoints[0];
    }

    private Transform GetNextWaypoint()
    {
        currentWaypointIndex++;

        if (currentWaypointIndex >= waypoints.Length)
        {
            currentWaypointIndex = 0;
        }

        return waypoints[currentWaypointIndex];
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
        else 
        {
            if (loop)
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
        platform.transform.position = 
                Vector2.MoveTowards(
                platform.position, 
                targetWaypoint.position, 
                speed * Time.fixedDeltaTime
                );

        if (Vector2.Distance(platform.position, targetWaypoint.position) < checkDistance)
        {
            targetWaypoint = GetNextWaypoint();
        }
        /*
        if (Vector2.Distance(waypoints[currentWaypointIndex].transform.position, platform.position) < .1f)
        {
            currentWaypointIndex++;

            if (currentWaypointIndex >= waypoints.Length)
            {
                currentWaypointIndex = 0;
            }
        }

        platform.position = Vector2.MoveTowards(platform.position, waypoints[currentWaypointIndex].transform.position, speed * Time.deltaTime);
        */
    }

    private void MoveUp()
    {
        platform.position = Vector2.MoveTowards(platform.position, waypoints[0].transform.position, speed * Time.deltaTime);
    }

    private void MoveDown()
    {
        platform.position = Vector2.MoveTowards(platform.position, waypoints[1].transform.position, speed * Time.deltaTime);
    }

    public void Activate()
    {
        active = true;
    }

    public void Deactivate()
    {
        active = false;
    }

    public IEnumerator Wait()
    {
        yield return new WaitForSeconds(1);
    }
}




