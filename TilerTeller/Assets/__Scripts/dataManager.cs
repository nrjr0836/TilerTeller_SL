using UnityEngine;
using System.Collections;
using System;
using System.Linq;


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


	int[] valueNum = new int[15];
	int threshold = 0;

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
									pandaPressed = i-5;
								}
							}
						}
						lastButtonState [i] = buttonState [i];
					}
				}


			}

			if (values [0] == "S1") {

			
				if (values.Length >= 13) {
					for (int i = 0; i < 12; i++) {
						valueNum [i] = int.Parse (values [i + 1]);
					}
				
					if ((valueNum [0]-threshold) < 3 && (valueNum [1]-threshold) < 3 && (valueNum [2]-threshold) < 3 && (valueNum [3]-threshold) < 3) {
						puzzleSolved = 3;
					} else if ((valueNum [4]-threshold) < 3 && (valueNum [5]-threshold) < 3 && (valueNum [6]-threshold) < 3 && (valueNum [7]-threshold) < 3) {
						puzzleSolved = 1;
					} else if ((valueNum [8]-threshold) < 3 && (valueNum [9]-threshold) < 3 && (valueNum [10]-threshold) < 3 && valueNum [11] < 3) {
						puzzleSolved = 2;
					} else
						puzzleSolved = 0;
				}
			}

		}
	}

	public void setThreshold(){
		float[] average = new float[3];
		average[0] = (valueNum [0] + valueNum [1] + valueNum [2] + valueNum [3]) / 4f;
		average[1] = (valueNum [4] + valueNum [5] + valueNum [6] + valueNum [7]) / 4f;
		average[2] = (valueNum [8] + valueNum [8] + valueNum [10] + valueNum [11]) / 4f;

		int minAverage = (int)average.Max ();
		if (minAverage < threshold) {
			threshold = minAverage;
		}
	}

	public int[] getButtonCounter(){
		return buttonCounter;
	}

	public void setButtonCounter(){
		for (int i = 0; i < 4; i++) {
			buttonCounter [i] = 0;
		}
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
	public void setPandaPressed(){
		pandaPressed = -1;
	}
}
