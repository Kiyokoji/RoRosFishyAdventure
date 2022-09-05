using System;
using UnityEngine;

namespace Level
{
    public class Activatable : MonoBehaviour
    {
        public bool isActive = false;
        
        public void SetActive(bool active)
        {
            isActive = active;
        }
    }
}
