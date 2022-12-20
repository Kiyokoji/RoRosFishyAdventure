using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class StarConnector : MonoBehaviour
{
    [Title("Connecting Stars")]
    [InfoBox("Star 1 will be connected with Star 2", InfoMessageType.None)]
    [SerializeField]
    [ReadOnly]
    private Star star1;
    
    [SerializeField]
    [ReadOnly]
    private Star star2;

    private StarCluster starCluster;

    private LineRenderer lineRenderer;
    private EdgeCollider2D edgeColliderTrigger;
    
    [Title("Edge Collider")]
    [InfoBox("Put the Edge Collider (found in the only child of this prefab) in here", InfoMessageType.Error, "IsEdgeColliderUsed")]
    [InfoBox("This is a reference to the actual collider. The Edge Collider in this GameObject is merely a trigger used to determine from where to where the collider should form.", InfoMessageType.None)]
    [HideInPlayMode]
    [SerializeField]
    private EdgeCollider2D edgeCollider;
    private bool IsEdgeColliderUsed() { return !(edgeCollider != null); }
    
    // Start is called before the first frame update
    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        edgeColliderTrigger = GetComponent<EdgeCollider2D>();

        edgeCollider.enabled = false;
    }

    /// <summary>
    /// Sets the two points which will be used to create a line with the LineRenderer as well as the bounds of the EdgeCollider trigger.
    /// </summary>
    /// <param name="starOne">The first star</param>
    /// <param name="starTwo">The connecting star</param>
    /// <param name="starCluster">The Star Cluster in which the stars are</param>
    public void InitializeConnectorLine(Star starOne, Star starTwo, StarCluster starCluster)
    {
        if (starCluster.CurrentOperatedStar == 0) return;
        
        star1 = starOne;
        
        star2 = starTwo;

        this.starCluster = starCluster;
        
        Vector3[] points = new Vector3[2];
        points[0] = starOne.transform.position;
        points[1] = starTwo.transform.position;

        lineRenderer.SetPositions(points);
        
        Vector2[] triggerPoints = new Vector2[2];
        triggerPoints[0] = points[0];
        triggerPoints[1] = points[1];
        
        edgeColliderTrigger.points = triggerPoints;
    }

    /// <summary>
    /// Sets the active edge collider bounds when lit with a flashlight.
    /// </summary>
    /// <param name="points">Contact points calculated with Raycasts</param>
    public void UpdateActiveColliderSpace(Vector2?[] points)
    {
        if (!starCluster.FinishedPlacing) return;
        
        Vector2[] colliderPoints = new Vector2[2];
        if (points[0] == null && points[1] == null)
        {
            edgeCollider.enabled = false;
            return;
        }
        
        if (points[0] == null && points[1] != null)
        {
            colliderPoints[0] = edgeCollider.points[0];
            colliderPoints[1] = (Vector2)points[1];
        }
        else if (points[0] != null && points[1] == null)
        {
            colliderPoints[0] = (Vector2)points[0];
            colliderPoints[1] = edgeCollider.points[1];
        }
        else
        {
            colliderPoints[0] = (Vector2)points[0];
            colliderPoints[1] = (Vector2)points[1];
        }

        if (starCluster.FinishedPlacing) edgeCollider.enabled = true;
        edgeCollider.points = colliderPoints;
    }
}
