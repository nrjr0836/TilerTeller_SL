using UnityEngine;
using System.Collections;

public class storySelector : MonoBehaviour {

	public int storyNum;


	void Start(){
		if (GameObject.Find ("BluetoothManager") != null) {
			bluetoothManager.Instance.ble.sendBluetooth("1");
		}
		
	}


	public void selectStory(int story){
		if (GameObject.Find ("BluetoothManager") != null) {
			bluetoothManager.Instance.ble.sendBluetooth("1");
		}
		Application.LoadLevel (story);
	}

	public void playSound(){
//		AudioSource audio = GameObject.Find ("PlayButtonSound").GetComponent<AudioSource>();
//		if (audio != null) {
//			audio.Play ();
//		}

	}
}
