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

    public int CurrentOperatedStar { get; private set; }

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

    [InfoBox("Put the connector container (found in the children) in here", InfoMessageType.Error, "IsConnectorContainerUsed")]
    [SerializeField] 
    public Transform connectorContainer;
    private bool IsConnectorContainerUsed() { return !(connectorContainer != null); }

    [InfoBox("Put the connector container (found in the children) in here", InfoMessageType.Error, "IsConnectorPrefabUsed")]
    [SerializeField] 
    public Transform connectorPrefab;
    private bool IsConnectorPrefabUsed() { return !(connectorPrefab != null); }
    
    private Vector3 center;
    
    private bool updated;
    private bool starPlaced;
    public bool FinishedPlacing { get; private set; }


    // Start is called before the first frame update
    void Start()
    {
        CurrentOperatedStar = 0;

        updated = false;
        starPlaced = false;
        FinishedPlacing = false;
    }

    // Update is called once per frame
    void Update()
    {
        center = unplacedContainer.transform.position;
        
        CheckActualClusterProperties();

        for (int i = 0 + CurrentOperatedStar; i < stars.Count; i++)
        {
            stars[i].transform.RotateAround(center, transform.forward, starSpeed * Time.deltaTime);
        }
    }

    /// <summary>
    /// Checks if any of the star cluster properties has been changed in the editor or if a star has been placed. Checks every frame and adjusts the rotating star cluster's alignment.
    /// </summary>
    private void CheckActualClusterProperties()
    {
        updated = false;
        
        if (stars.Count < clusterSize)
        {
            for (int i = 0; i < clusterSize - stars.Count; i++)
            {
                Star star = Instantiate(starPrefab, unplacedContainer);
                stars.Add(star);
            }

            updated = true;
        }

        if (stars.Count > clusterSize)
        {
            for (int i = stars.Count; i > clusterSize; i--)
            {
                DeleteStar(i);
            }

            updated = true;
        }
        
        /*
        float distanceBetweenCenterAndEdgeStars = Mathf.Round(
            Vector2.Distance(
                stars[0].transform.localPosition, 
                stars[1].transform.localPosition) 
            * 100.0f) / 100.0f;
        
        //if (updated || distanceBetweenCenterAndEdgeStars < radius || distanceBetweenCenterAndEdgeStars > radius)
            //SetStarPositions();
            */
        
        if (updated || starPlaced) SetStarPositions();
    }

    /// <summary>
    /// Deletes a star if the Star Cluster was updated in the Unity Editor.
    /// </summary>
    /// <param name="index">The star in question</param>
    private void DeleteStar(int index)
    {
        Destroy(stars[index - 1].gameObject);
        stars.RemoveAt(index - 1);
    }

    /// <summary>
    /// Calculates the angles relative to the amount of the stars of the outer circle and places them accordingly.
    /// </summary>
    private void SetStarPositions()
    {
        starPlaced = false;
        // ReSharper disable once PossibleLossOfFraction
        double angle = 360.0 / (clusterSize - 1 - CurrentOperatedStar);

        if (0 + CurrentOperatedStar < stars.Count) stars[0 + CurrentOperatedStar].transform.localPosition = Vector3.zero;
        //else stars[currentOperatedStar].transform.localPosition = Vector3.zero;
        
        if (1 + CurrentOperatedStar < stars.Count) stars[1 + CurrentOperatedStar].transform.localPosition = Vector2.up * radius;
        //else stars[currentOperatedStar].transform.localPosition = Vector3.zero;
        
        for (int i = 2 + CurrentOperatedStar; i < stars.Count; i++)
        {
            stars[i].transform.localPosition = CalculateVectorFromAngle(stars[i - 1].transform.localPosition, (float)angle);
        }
    }

    /// <summary>
    /// Helping method to calculate the Vector from a given angle. Used in SetStarPosition()
    /// </summary>
    /// <param name="vector"></param>
    /// <param name="angle"></param>
    /// <returns>The new Vector2 calculated from the given vector and angle.</returns>
    private Vector2 CalculateVectorFromAngle(Vector2 vector, float angle)
    {
        float angleDeg2Rad = angle * Mathf.Deg2Rad;
        
        return new Vector2(
            (Mathf.Cos(angleDeg2Rad) * vector.x) - (Mathf.Sin(angleDeg2Rad) * vector.y),
            (Mathf.Sin(angleDeg2Rad) * vector.x) + (Mathf.Cos(angleDeg2Rad) * vector.y)
        );
    }

    /// <summary>
    /// Places a star and goes to the next one. Creates a connector if at least one star has been placed prior.
    /// </summary>
    public void NextStar()
    {
        if (FinishedPlacing)
        {
            return;
        }
        stars[CurrentOperatedStar].placed = true;
        stars[CurrentOperatedStar].transform.SetParent(placedContainer);
        
        Transform starConnector; 
        
        if (CurrentOperatedStar == 0)
        {
            Debug.Log("first star placed");
        }
        else
        {
            starConnector = Instantiate(connectorPrefab, connectorContainer);
            starConnector.GetComponent<StarConnector>().InitializeConnectorLine(stars[CurrentOperatedStar - 1], stars[CurrentOperatedStar], this);
            
            if (CurrentOperatedStar + 1 == stars.Count)
            {
                EndClusterPlacing();
            }
        }

        CurrentOperatedStar++;
        Debug.Log(CurrentOperatedStar + " " + stars.Count);
        starPlaced = true;
    }

    public void EndClusterPlacing()
    {
        FinishedPlacing = true;
    }
}
