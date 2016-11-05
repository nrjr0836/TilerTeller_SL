
using UnityEngine;
using System;
using System.Collections;
using System.IO.Ports;
using System.Linq;



public class ArduinoManager : MonoBehaviour {

	/* The serial port where the Arduino is connected. */
	[Tooltip("The serial port where the Arduino is connected")]
	public string port = "/dev/cu.usbmodem1431";
	/* The baudrate of the serial port. */
	[Tooltip("The baudrate of the serial port")]
	public int baudrate = 9600;



	private SerialPort stream ;
	[SerializeField]private int  puzzleSolved;

	int[] buttonCounter = {0,0,0,0};
	int[] buttonState = {0,0,0,0};
	int[] lastButtonState = {0,0,0,0};

	public bool isConnected;

	void Start () {
		stream = new SerialPort (port, baudrate);
		if (stream != null) {
			stream.Open ();
			stream.ReadTimeout = 25;
			isConnected = true;
		} else {
			isConnected = false;
		}

	}

	// Update is called once per frame
	void Update () {
		//		if (Input.GetKeyDown (KeyCode.Alpha1)) {
		//			WriteToArduino ("led");
		//		}
		StartCoroutine (
			AsynchronousReadFromArduino(
				(string s) => { 
					string[] values = s.Split(new[]{','},StringSplitOptions.RemoveEmptyEntries);


					if(values[0]=="S2"){
						for(int i=0; i<4; i++){
							buttonState[i] = int.Parse(values[i+1]);
							if(buttonState[i] != lastButtonState[i]){
								if(buttonState[i] == 1){
									buttonCounter[i] ++;
								}
							}
							lastButtonState[i] = buttonState[i];}
						//						Debug.Log(buttonCounter[0]+","+buttonCounter[1]+","+buttonCounter[2]+","+buttonCounter[3]);
					}


					if(values[0] == "S1"){

						int[] valueNum = new int[12];
						for(int i=0; i<12; i++){
							//						Debug.Log(i+": "+ int.Parse(values[i+1]));
							valueNum[i] = int.Parse(values[i+1]);
						}
						if(valueNum[0]<3 && valueNum[1]<3 && valueNum[2]<3 && valueNum[3]<3){
							puzzleSolved = 3;
						}
						else if(valueNum[4]<3 && valueNum[5]<3 && valueNum[6]<3 && valueNum[7]<3){
							puzzleSolved = 2;
						}
						else if(valueNum[8]<3 && valueNum[9]<3 && valueNum[10]<3 && valueNum[11]<3){
							puzzleSolved = 1;
						}
						else puzzleSolved = 0;

					}




				},
				null,
				25f
			));

	}

	public void WriteToArduino(string message)
	{
		// Send the request
		stream.Write(message);
		stream.BaseStream.Flush();
	}

	public IEnumerator AsynchronousReadFromArduino(Action<string> callback, Action fail = null, float timeout = float.PositiveInfinity)
	{
		DateTime initialTime = DateTime.Now;
		DateTime nowTime;
		TimeSpan diff = default(TimeSpan);

		string dataString = null;

		do
		{
			// A single read attempt
			try
			{
				dataString = stream.ReadLine();
			}
			catch (TimeoutException)
			{
				dataString = null;
			}

			if (dataString != null)
			{
				callback(dataString);
				yield return null;
			} else
				yield return new WaitForSeconds(0.05f);

			nowTime = DateTime.Now;
			diff = nowTime - initialTime;

		} while (diff.Milliseconds < timeout);

		if (fail != null)
			fail();
		yield return null;
	}

	public int getPuzzleNum(){
		return puzzleSolved;
	}

	public void setPuzzleNum(int puzzleNum){
		puzzleSolved = puzzleNum;
	}

	public int[] getButtonCounter(){
		return buttonCounter;
	}

}
