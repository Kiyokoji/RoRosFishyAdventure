using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class AnimatorFunctions : MonoBehaviour
{
	//public string starSound;
	public EventReference sound;
	
	void PlaySound(){
		
		FMODUnity.RuntimeManager.PlayOneShot(sound);
	}
}	
