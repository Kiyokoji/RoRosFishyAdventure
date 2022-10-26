using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffsetAnimator : MonoBehaviour
{
    private Animator anim;  

    void Start()
    {
        anim = GetComponent<Animator>();
        AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);//could replace 0 by any other animation layer index
        anim.Play(state.fullPathHash, -1, Random.Range(0f, 1f));
    }
}