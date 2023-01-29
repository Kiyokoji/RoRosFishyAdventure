using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class AnimatorFunctions : MonoBehaviour
{
	//public string starSound;
	public EventReference hoverSound;
	public EventReference clickSound;
	
	void PlaySound(){
		
		FMODUnity.RuntimeManager.PlayOneShot(hoverSound);
	}

	void OnClickSound()
	{
		FMODUnity.RuntimeManager.PlayOneShot(clickSound);
	}
}	
