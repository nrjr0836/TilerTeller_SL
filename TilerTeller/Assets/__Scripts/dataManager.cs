using UnityEngine;
using System.Collections;
using System;


public class dataManager : MonoBehaviour {

	public bleManager ble;

	public static dataManager Instance;

	private int puzzleSolved = 0;

	int[] buttonCounter = { 0, 0, 0, 0 };
	int[] buttonState = { 0, 0, 0, 0, 0,0,0,0,0 }; // 0,1,2,3 for star scene, 4 for bunny scene, 5,6,7,8 for panda scene
	int[] lastButtonState = { 0, 0, 0, 0, 0,0,0,0,0}; // 0,1,2,3 for star scene, 4 for bunny scene


	bool bunnyIsPressed = false;
	string s;

	int pandaPressed = -1;

	void Update () {
		
	
		string s = ble.getdataReceived ();
//		Debug.Log (s);
		if (s != null && s.Length>5) {
			string[] values = s.Split (new[]{ ',' }, StringSplitOptions.RemoveEmptyEntries);

			if (values [0] == "S2") {

				if (values.Length >= 6) {

					/* 0,1,2,3 are for star scene */
					for (int i = 0; i < 9; i++) {
					
						buttonState [i] = int.Parse (values [i + 1]);
						if (buttonState [i] != lastButtonState [i]) {
							if (buttonState [i] == 1) {
								if (i < 4) {
									buttonCounter [i]++;
								} else if (i == 4) {
									bunnyIsPressed = true;
								} else {
									pandaPressed = i-4;
								}
							}
						}
						lastButtonState [i] = buttonState [i];
					}
				}


			}

			if (values [0] == "S1") {

				int[] valueNum = new int[15];
				if (values.Length >= 13) {
					for (int i = 0; i < 12; i++) {
						valueNum [i] = int.Parse (values [i + 1]);
					}
				
					if (valueNum [0] < 5 && valueNum [1] < 5 && valueNum [2] < 5 && valueNum [3] < 5) {
						puzzleSolved = 3;
					} else if (valueNum [4] < 5 && valueNum [5] < 5 && valueNum [6] < 5 && valueNum [7] < 5) {
						puzzleSolved = 1;
					} else if (valueNum [8] < 5 && valueNum [9] < 5 && valueNum [10] < 5 && valueNum [11] < 5) {
						puzzleSolved = 2;
					} else
						puzzleSolved = 0;

				}
			}

		}
	}

	public int[] getButtonCounter(){
		return buttonCounter;
	}

	public int getPuzzleNum(){
		return puzzleSolved;
	}

	public bool getBunnyIsPressed(){
		return bunnyIsPressed;
	}
	public void setBunnyIsPressed(){
		bunnyIsPressed = false;
	}
	public int getPandaPressed(){
		return pandaPressed;
	}
}
