using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {

	public bool isPlaying = false;

	public void PlaySound(string wwiseEvent){
		isPlaying = true;
		AkSoundEngine.PostEvent (wwiseEvent,gameObject);
	}
	public void StopPlaying(){
		AkSoundEngine.StopAll ();
	}
}
