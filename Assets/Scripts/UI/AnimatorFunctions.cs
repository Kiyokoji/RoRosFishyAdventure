using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorFunctions : MonoBehaviour
{
	void PlaySound(string sound){
		FindObjectOfType<AudioManager>().Play(sound);
	}
}	
