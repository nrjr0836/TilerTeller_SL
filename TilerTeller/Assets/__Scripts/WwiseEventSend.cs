using UnityEngine;
using System.Collections;

public class WwiseEventSend : MonoBehaviour {
	public string wwiseEvent;
	public GameObject gameObj;
	// Use this for initialization
	void Start () {
		playSound ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void playSound() {
		AkSoundEngine.PostEvent (wwiseEvent,gameObj);
	}
}
