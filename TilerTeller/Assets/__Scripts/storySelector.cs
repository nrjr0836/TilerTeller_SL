using UnityEngine;
using System.Collections;

public class storySelector : MonoBehaviour {

	public int storyNum;


	void Start(){
		
	}
	public void selectStory(int story){
		Application.LoadLevel (story);
	}

	public void playSound(){
//		AudioSource audio = GameObject.Find ("PlayButtonSound").GetComponent<AudioSource>();
//		if (audio != null) {
//			audio.Play ();
//		}

	}
}
