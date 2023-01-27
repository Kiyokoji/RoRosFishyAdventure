using System;
using System.Collections;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using STOP_MODE = FMOD.Studio.STOP_MODE;

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
    
    [SerializeField] private Transform platform;

    public EventReference platformSound;
    private EventInstance platformEvent;

    private bool isMoving;
    private bool isPlaying;
    
    private void Start()
    {
        platformEvent = FMODUnity.RuntimeManager.CreateInstance(platformSound);
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
        if (isActive)
        {
            if (!isMoving)
            {
                platformEvent.start();
                isMoving = true;
            }
        }
        else
        {
            platformEvent.stop(STOP_MODE.ALLOWFADEOUT);
            isMoving = false;
        }

        if (!isActive)
        {
            return;
        }

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




