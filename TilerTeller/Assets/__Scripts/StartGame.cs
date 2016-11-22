using UnityEngine;
using System.Collections;

public class StartGame : MonoBehaviour {



	public void startGame(){
		if (GameObject.Find ("BluetoothManager") != null) {
			bluetoothManager.Instance.ble.sendBluetooth("1");
		}
		Application.LoadLevel (3);

	}



	public void continueGame(){
		Application.LoadLevel (4);
	}


	

		
}
