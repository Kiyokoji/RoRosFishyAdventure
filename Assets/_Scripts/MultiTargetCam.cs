using System;
using System.Collections;
using System.Collections.Generic;
using FMOD;
using TMPro.Examples;
using UnityEngine;

public class MultiTargetCam : MonoBehaviour
{
    private Vector3 velocity;
    private Camera cam;
    
    public List<Transform> targets;
    public float smoothTime = .5f;
    public Vector3 offset;

    public float minZoom = 40f;
    public float maxZoom = 10f;
    public float zoomLimiter = 50f;

    private void Start()
    {
        cam = GetComponent<Camera>();
    }

    private void LateUpdate()
    {
        if (targets.Count == 0) return;
        
        Move();
        Zoom();
    }

    public void Zoom()
    {
        float newZoom = Mathf.Lerp(maxZoom, minZoom, (GetDistance() + 10f)/ zoomLimiter);
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, newZoom, Time.deltaTime);
    }
    
    public void Move()
    {
        
        
        Vector3 centerPoint = GetCenterPoint();

        Vector3 newPosition = centerPoint + offset;

        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);
    }

    private float GetDistance()
    {
        var bounds = new Bounds(targets[0].position, Vector3.zero);
        
        for (int i = 0; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }
        
        return bounds.size.x;
    }
    
    Vector3 GetCenterPoint()
    {

        if (targets.Count == 1)
        {
            return targets[0].position;
        }

        var bounds = new Bounds(targets[0].position, Vector3.zero);

        for (int i = 0; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }
        
        return bounds.center;
    }

}
