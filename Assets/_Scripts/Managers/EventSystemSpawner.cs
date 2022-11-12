using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;

public class EventSystemSpawner : MonoBehaviour
{
    void OnEnable()
    {
        EventSystem sceneEventSystem = FindObjectOfType<EventSystem>();
        if (sceneEventSystem == null)
        {
            GameObject eventSystem = new GameObject("EventSystem");

            eventSystem.AddComponent<EventSystem>();
            //eventSystem.AddComponent<StandaloneInputModule>();
            eventSystem.AddComponent<InputSystemUIInputModule>();
        }
    }
}
