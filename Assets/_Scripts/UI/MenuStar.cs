using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using FMODUnity;

public class MenuStar : MonoBehaviour
{
    public EventReference starSound;
    public GameObject starSprite;
    
    public float size = 0.5f;

    private Animator anim;

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }
    
    [ExecuteInEditMode]
    private void Update()
    {
        starSprite.transform.localScale = new Vector2(size, size);
    }

    public void OnHover()
    {
        anim.SetTrigger("starHover");
        FMODUnity.RuntimeManager.PlayOneShot(starSound);
    }

    public void OnExit()
    {
        anim.SetTrigger("starBob");
    }


}
