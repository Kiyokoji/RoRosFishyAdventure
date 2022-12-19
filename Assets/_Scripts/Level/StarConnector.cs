using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class StarConnector : MonoBehaviour
{
    [SerializeField]
    [ReadOnly]
    private Star star1;
    
    [SerializeField]
    [ReadOnly]
    private Star star2;

    private LineRenderer lineRenderer;
    private EdgeCollider2D edgeColliderTrigger;
   
    [SerializeField]
    private EdgeCollider2D edgeCollider;
    
    // Start is called before the first frame update
    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        edgeColliderTrigger = GetComponent<EdgeCollider2D>();
        
        InitializeConnectorLine();
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    private void InitializeConnectorLine()
    {
        Vector3[] points = new Vector3[2];
        points[0] = star1.transform.position;
        points[1] = star2.transform.position;
        
        lineRenderer.SetPositions(points);
        
        //edgeColliderTrigger
    }

    public void UpdateActiveColliderSpace(Vector2[] points)
    {
        edgeCollider.points = points;
    }
}
