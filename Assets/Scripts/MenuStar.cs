using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using FMODUnity;

public class MenuStar : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public EventReference EventReference;

    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        anim.SetTrigger("starHover");
        
        Debug.Log("STAR GO BING");
        
        FMODUnity.RuntimeManager.PlayOneShot(EventReference);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        anim.SetTrigger("starBob");
        
        Debug.Log("pointer exit");
    }

}
