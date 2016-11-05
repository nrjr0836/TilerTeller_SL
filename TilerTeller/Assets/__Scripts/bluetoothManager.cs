using UnityEngine;
using System.Collections;

public class bluetoothManager : MonoBehaviour {

	public static bluetoothManager Instance;

	public bleManager ble;
	public dataManager datamanager;

	void Awake(){
		if (Instance == null) {
			DontDestroyOnLoad (gameObject);
			Instance = this;
		} else if (Instance != this) {
			Destroy (gameObject);
		}
	}

//	public bleManager getBleManager(){
//		return this.ble;
//	}
//
//	public dataManager getDataManager(){
//		return this.datamanager;
//	}



}
