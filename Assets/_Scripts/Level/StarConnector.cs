using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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

    public GameObject circle1, circle2;

    private List<Star> starsFromConnectorLit;
    
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
        triggerPoints[0] = transform.InverseTransformPoint(points[0]);
        triggerPoints[1] = transform.InverseTransformPoint(points[1]);

        edgeColliderTrigger.points = triggerPoints;
    }

    /// <summary>
    /// Sets the active edge collider bounds when lit with a flashlight.
    /// </summary>
    /// <param name="points">Contact points calculated with Raycasts</param>
    public void UpdateActiveColliderSpace(Vector2?[] points, List<Star> starsLit)
    {
        if (!starCluster.FinishedPlacing) return;
        
        //Debug.Log(points[0] + " || " + points[1]);

        foreach (var star in starsLit.ToList())
        {
            if (star != star1 && star != star2) starsLit.Remove(star);
        }
        
        /*cases
        
        0 and 1 dont hit
        0 hits but 1 doesnt, star 1 is triggered
        0 hits but 1 doesnt, star 2 is triggered
        0 doesnt hit but 1 does, star 1 is triggered
        0 doesnt hit but 1 does, star 2 is triggered
        0 and 1 hit
        0 and 1 dont hit, but star 1 and 2 are triggered
        
        */

        Vector2[] colliderPoints = new Vector2[2];
        
        // 0 and 1 dont hit
        if (points[0] == null && points[1] == null) 
        {
            edgeCollider.enabled = false;
            return;
        }


        if (starsLit.Count != 0)
        {
            // 0 hits but 1 doesnt, star 1 is triggered
            if (points[0] == null && points[1] != null && starsLit[0] == star1)
            {
                colliderPoints[0] = transform.InverseTransformPoint(star1.transform.position);
                colliderPoints[1] = transform.InverseTransformPoint((Vector2)points[1]);
            }
            // 0 hits but 1 doesnt, star 2 is triggered
            else if (points[0] == null && points[1] != null && starsLit[0] == star2)
            {
                colliderPoints[0] = transform.InverseTransformPoint(star2.transform.position);
                colliderPoints[1] = transform.InverseTransformPoint((Vector2)points[1]);
            }
            // 0 doesnt hit but 1 does, star 1 is triggered
            else if (points[0] != null && points[1] == null && starsLit[0] == star1)
            {
                colliderPoints[0] = transform.InverseTransformPoint((Vector2)points[0]);
                colliderPoints[1] = transform.InverseTransformPoint(star1.transform.position);
            }
            // 0 doesnt hit but 1 does, star 2 is triggered
            else if (points[0] != null && points[1] == null && starsLit[0] == star2)
            {
                colliderPoints[0] = transform.InverseTransformPoint((Vector2)points[0]);
                colliderPoints[1] = transform.InverseTransformPoint(star2.transform.position);
            }
            // 0 and 1 dont hit, but star 1 and 2 are triggered
            else if (points[0] == null && points[1] == null && starsLit.Count == 2)
            {
                colliderPoints[0] = transform.InverseTransformPoint(star1.transform.position);
                colliderPoints[1] = transform.InverseTransformPoint(star2.transform.position);
            }
        }
        // 0 and 1 hit
        else if (starsLit.Count == 0 && points[0] != null && points[1] != null)
        {
            colliderPoints[0] = transform.InverseTransformPoint((Vector2)points[0]);
            colliderPoints[1] = transform.InverseTransformPoint((Vector2)points[1]);
        }
        
        if (starCluster.FinishedPlacing) edgeCollider.enabled = true;
        edgeCollider.points = colliderPoints;
        if (circle1 != null) circle1.transform.position = colliderPoints[0];
        if (circle2 != null) circle2.transform.position = colliderPoints[1];
    }
}
