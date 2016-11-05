using UnityEngine;
using System.Collections;

public class playSound : MonoBehaviour {

	public string[] wwiseEvent;
	public GameObject gameObj;

	//Commented to test Wwise Audio Engine Instead
	//private AudioSource audio{get{ return GetComponent<AudioSource> ();}}

	void Awake(){
		
		DontDestroyOnLoad (gameObject);
	}
	
	public void PlaySound(){
		//Commented to test Wwise Audio Engine Instead
		/*if (audio != null) {
			audio.Play ();
		}
		*/
		AkSoundEngine.PostEvent (wwiseEvent[0],this.gameObject);

	}

	public void StopSound(){
		//Commented to test Wwise Audio Engine Instead
		/*if (audio != null) {
			audio.Play ();
		}
		*/
		AkSoundEngine.PostEvent (wwiseEvent[1],gameObj);

	}
}
