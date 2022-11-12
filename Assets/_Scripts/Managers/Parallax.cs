using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Parallax : MonoBehaviour
{
    public Plane PlaneDistance = Plane.VeryFar;

    public enum Plane
    {
        VeryFar = 40,
        QuiteFar = 30,
        Far = 20,
        Near = 10,
        VeryNear = 0,
    }

    public CinemachineVirtualCamera cam;

    public Transform subject;

    Vector2 startPosition;
    float startZ;

    float clippingPlane => (cam.transform.position.z + (distanceFromSubject > 0 ? cam.m_Lens.FarClipPlane : cam.m_Lens.NearClipPlane));

    float parallaxFactor => Mathf.Abs(distanceFromSubject) / clippingPlane;

    float distanceFromSubject => transform.position.z;
    //float distanceFromSubject => transform.position.z - subject.position.z;
    
    Vector2 travel => (Vector2)cam.transform.position - startPosition;

    Vector2 parallaxVector;

    void Start()
    {
        startPosition = transform.position;
        //startZ = transform.position.z;
        startZ = (int)PlaneDistance;
    }

    void Update()
    {

        Vector2 newPos = startPosition + travel * parallaxFactor;
        transform.position = new Vector3(newPos.x, newPos.y, startZ);

    }

    public void FindCam()
    {
        cam = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<CinemachineVirtualCamera>();
    }
}
