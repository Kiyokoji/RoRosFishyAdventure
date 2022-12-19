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
    public int ClusterSize => clusterSize;

    private int currentOperatedStar;
    
    [InfoBox("Max Cluster Size - will affect the biggest value of the slider above", InfoMessageType.None)]
    [SerializeField]
    private int maxClusterSize = 10;

    [Title("Radius")]
    [InfoBox("Max Radius - The radius of the star cluster circle -> the distance from the center to the circle", InfoMessageType.None)]
    [PropertyRange(0, "maxRadius")]
    [SerializeField]
    private float radius = 5;

    [InfoBox("Max Radius - will affect the biggest value of the slider above", InfoMessageType.None)]
    [SerializeField]
    private float maxRadius = 1000;

    [Title("Speed")]
    [InfoBox("Star Speed - The speed at which the stars rotate around the center", InfoMessageType.None)]
    [PropertyRange(0, "maxSpeed")]
    [SerializeField]
    private float starSpeed = 5f;
    
    [InfoBox("Max Speed - will affect the biggest value of the slider above", InfoMessageType.None)]
    [SerializeField]
    private float maxSpeed = 10;
    
    [Title("Misc")]
    [ReadOnly]
    [SerializeField]
    private List<Star> stars;

    [InfoBox("Put the unplaced container (found in the children) in here", InfoMessageType.Error, "IsUnplacedContainerUsed")]
    [SerializeField]
    public Transform unplacedContainer;
    private bool IsUnplacedContainerUsed() { return !(unplacedContainer != null); }

    [InfoBox("Put the placed container (found in the children) in here", InfoMessageType.Error, "IsPlacedContainerUsed")]
    [SerializeField] 
    public Transform placedContainer;
    private bool IsPlacedContainerUsed() { return !(placedContainer != null); }

    private Vector3 center;
    
    private bool updated = false;
    private bool starPlaced = false;
    
    
    // Start is called before the first frame update
    void Start()
    {
        currentOperatedStar = 0;
    }

    // Update is called once per frame
    void Update()
    {
        center = unplacedContainer.transform.position;
        //stars[currentOperatedStar].transform.position
        
        CheckActualClusterProperties();

        for (int i = 0 + currentOperatedStar; i < stars.Count; i++)
        {
            //stars[i].transform.RotateAround(unplacedContainer.transform.position, Vector3.forward, starSpeed * Time.deltaTime);
            stars[i].transform.RotateAround(center, transform.forward, starSpeed * Time.deltaTime);

        }
    }

    private void CheckActualClusterProperties()
    {
        updated = false;
        
        if (stars.Count < clusterSize)
        {
            for (int i = 0; i < clusterSize - stars.Count; i++)
            {
                Star star = Instantiate(starPrefab, unplacedContainer);
                stars.Add(star);
                //stars[stars.Count - 2].nextStar = star;
            }

            updated = true;
        }

        if (stars.Count > clusterSize)
        {
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
        
        //if (updated || distanceBetweenCenterAndEdgeStars < radius || distanceBetweenCenterAndEdgeStars > radius)
            //SetStarPositions();
        if (updated || starPlaced) SetStarPositions();
    }

    private void DeleteStar(int index)
    {
        Destroy(stars[index - 1].gameObject);
        stars.RemoveAt(index - 1);
    }

    private void SetStarPositions()
    {
        starPlaced = false;
        // ReSharper disable once PossibleLossOfFraction
        double angle = 360.0 / (clusterSize - 1 - currentOperatedStar);

        if (0 + currentOperatedStar < stars.Count) stars[0 + currentOperatedStar].transform.localPosition = Vector3.zero;
        //else stars[currentOperatedStar].transform.localPosition = Vector3.zero;
        
        if (1 + currentOperatedStar < stars.Count) stars[1 + currentOperatedStar].transform.localPosition = Vector2.up * radius;
        //else stars[currentOperatedStar].transform.localPosition = Vector3.zero;
        
        for (int i = 2 + currentOperatedStar; i < stars.Count; i++)
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

    public void NextStar()
    {
        if (stars.Count == currentOperatedStar)
        {
            Debug.Log("bruh");
            return;
        }
        stars[currentOperatedStar].placed = true;
        stars[currentOperatedStar].transform.SetParent(placedContainer);
        //if (currentOperatedStar + 1 != stars.Count) 
        currentOperatedStar++;
        Debug.Log(currentOperatedStar + " " + stars.Count);
        starPlaced = true;
        //updated = true;
    }
}
