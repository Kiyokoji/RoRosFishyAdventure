using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class StarCluster : MonoBehaviour
{
    //
    //  TODO list
    //  
    //  - todo Center Star
    //
    
    [Title("Star Prefab")]
    [InfoBox("Put the star prefab in here", InfoMessageType.Error, "IsStarPrefabUsed")]
    [SerializeField]
    private Star starPrefab;

    private bool IsStarPrefabUsed() { return !(starPrefab != null); }

    [Title("Size")]
    [InfoBox("Cluster Size - Variable that sets the amount of stars in the cluster", InfoMessageType.None)]
    [PropertyRange(2, "maxClusterSize")]
    [SerializeField]
    private int clusterSize;
    
    //[InfoBox("Max Cluster Size - will affect the biggest value of the slider above", InfoMessageType.None)]
    //[SerializeField]
    //private int maxClusterSize = 10;

    [Title("Radius")]
    [InfoBox("Max Radius - The radius of the star cluster circle -> the distance from the center to the circle", InfoMessageType.None)]
    [PropertyRange(0, "maxRadius")]
    [SerializeField]
    private float radius = 5;

    //[InfoBox("Max Radius - will affect the biggest value of the slider above", InfoMessageType.None)]
    //[SerializeField]
    //private float maxRadius = 1000;

    [Title("Speed")]
    [InfoBox("Star Speed - The speed at which the stars rotate around the center", InfoMessageType.None)]
    [PropertyRange(0, "maxSpeed")]
    [SerializeField]
    private float starSpeed = 5f;
    
    //[InfoBox("Max Speed - will affect the biggest value of the slider above", InfoMessageType.None)]
    //[SerializeField]
    //private float maxSpeed = 10;
    
    [Title("Misc")]
    [ReadOnly]
    [SerializeField]
    private List<Star> stars;

    private bool updated = false;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckActualClusterProperties();

        for (int i = 0; i < stars.Count; i++)
        {
            stars[i].transform.RotateAround(transform.position, Vector3.forward, starSpeed * Time.deltaTime);
        }
    }

    private void CheckActualClusterProperties()
    {
        updated = false;
        
        if (stars.Count < clusterSize)
        {
            for (int i = 0; i < clusterSize - stars.Count; i++)
            {
                Star star = Instantiate(starPrefab, transform);
                stars.Add(star);
                //stars[stars.Count - 2].nextStar = star;
            }

            updated = true;
        }

        if (stars.Count > clusterSize)
        {
            int starsCountTemp = stars.Count;
            for (int i = stars.Count; i > clusterSize; i--)
            {
                //Debug.Log(stars.Count + " " + i);
                DeleteStar(i);
            }

            updated = true;
        }
        
        float distanceBetweenCenterAndEdgeStars = Mathf.Round(
            Vector2.Distance(
                stars[0].transform.localPosition, 
                stars[1].transform.localPosition) 
            * 100.0f) / 100.0f;
        
        if (updated || distanceBetweenCenterAndEdgeStars < radius || distanceBetweenCenterAndEdgeStars > radius)
            SetStarPositions();
    }

    private void DeleteStar(int index)
    {
        Destroy(stars[index - 1].gameObject);
        stars.RemoveAt(index - 1);
    }

    private void SetStarPositions()
    {
        // ReSharper disable once PossibleLossOfFraction
        double angle = 360.0 / (clusterSize - 1.0);

        stars[0].transform.localPosition = Vector3.zero;
        stars[1].transform.localPosition = Vector2.up * radius;
        
        for (int i = 2; i < stars.Count; i++)
        {
            stars[i].transform.localPosition = CalculateVectorFromAngle(stars[i - 1].transform.localPosition, (float)angle);
        }
    }

    private Vector2 CalculateVectorFromAngle(Vector2 vector, float angle)
    {
        float angleDeg2Rad = angle * Mathf.Deg2Rad;
        
        return new Vector2(
            (Mathf.Cos(angleDeg2Rad) * vector.x) - (Mathf.Sin(angleDeg2Rad) * vector.y),  // 
            (Mathf.Sin(angleDeg2Rad) * vector.x) + (Mathf.Cos(angleDeg2Rad) * vector.y)   //
        );
    }
}
