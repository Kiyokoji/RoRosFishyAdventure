using System;
using UnityEngine;

public class Activatable : MonoBehaviour
{
    public bool isActive = false;
        
    public void SetActive(bool active)
    {
        isActive = active;
    }
}