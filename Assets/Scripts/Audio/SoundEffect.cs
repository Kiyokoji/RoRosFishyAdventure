using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffect : MonoBehaviour
{
    //public AudioSource footstep;
    //public AudioSource jump;

    void FootstepSound()
    {
        FindObjectOfType<AudioManager>().Play("Footstep");
        //footstep.Play();
    }

    void JumpSound()
    {
        FindObjectOfType<AudioManager>().Play("Jump");
        //jump.Play();
    }
}
