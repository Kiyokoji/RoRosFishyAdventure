using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Level;

[ExecuteInEditMode]
public class LineController : Activatable
{
    private LineRenderer lr;
    [SerializeField] private Transform[] points;
    //[SerializeField] private Texture[] textures;
    //[SerializeField] float fps = 30f;
    
    //private int animationStep;
    //private float fpsCounter;

    public bool active = false;

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
    }

    private void Start()
    {
        Transform[] childPoints = transform.Cast<Transform>().ToArray();
        SetUpLine(childPoints);

    }

    private void Update()
    {
        //if (!active) return;
        DrawLine();
    }

    private void DrawLine()
    {
        for (int i = 0; i < points.Length; i++)
        {
            lr.SetPosition(i, points[i].position);
        }/*

        fpsCounter += Time.deltaTime;
        if (fpsCounter >= 1f / fps)
        {
            animationStep++;

            if (animationStep == textures.Length)
            {
                animationStep = 0;
            }
            //this.lr.sharedMaterial.SetTexture("_MainTex", textures[animationStep]);
            fpsCounter = 0f;
        }

        */
    }

    private void SetUpLine(Transform[] points)
    {
        lr.positionCount = points.Length;
        this.points = points;
    }

}
