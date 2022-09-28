using System.Collections;
using UnityEngine;

public class MovingPlatform : Activatable
{
    public bool isElevator;
    [SerializeField] private bool alwaysMoving;
    [SerializeField] private float speed = 2f;
    
    private bool active;
    
    [SerializeField] private GameObject[] waypoints;
    [SerializeField] private Transform platform;

    private int currentWaypointIndex = 0;
    
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
        if (Vector2.Distance(waypoints[currentWaypointIndex].transform.position, platform.position) < .1f)
        {
            currentWaypointIndex++;

            if (currentWaypointIndex >= waypoints.Length)
            {
                currentWaypointIndex = 0;
            }
        }

        platform.position = Vector2.MoveTowards(platform.position, waypoints[currentWaypointIndex].transform.position, speed * Time.deltaTime);
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




