using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;


public class S2PageManager : MonoBehaviour {

	public GameObject[] pages;
	public notebookGenerator notebook;
	public SoundManager sound;

	private int currentPage;
	private int bookLength;

	private GameObject lastBtn;
	private GameObject nextBtn;
	private GameObject homeBtn_1;

	private bool[] isStarted = { false, false, false };

	private float last_start_time;


	void Start () {

		lastBtn = GameObject.Find ("/Canvas/last");
		nextBtn = GameObject.Find ("/Canvas/next");
		homeBtn_1 = GameObject.Find ("/Canvas/home1");

		bookLength = pages.Length;
		currentPage = 0;

		last_start_time = Time.time;
	}



	void Update () {


		lastBtn.SetActive (true);
		nextBtn.SetActive (true);
		homeBtn_1.SetActive (false);

		for (int i = 0; i < bookLength; i++) {
			if (i == currentPage && pages [i] != null) {
				pages [i].SetActive (true);
			} else {
				pages [i].SetActive (false);
			}
		}

		if (currentPage == 0) {
			homeBtn_1.SetActive (true);
			lastBtn.SetActive (false);
		}

		if (currentPage >= 1) {
			nextBtn.SetActive (false);
		}

		if (currentPage == 2 && !isStarted[0]) {
			GameObject.Find ("Page3").GetComponent<StarLighter> ().start = true;
			isStarted [0] = true;
		}

		if (currentPage == 3 && !isStarted[1] ) {
			GameObject.Find ("Page4").GetComponent<BunnyManager> ().start = true;
			isStarted [1] = true;
		}

		if (currentPage == 4 && !isStarted[2]) {
			GameObject.Find ("Page5").GetComponent<PandaManager> ().start = true;
			isStarted [2] = true;
		}


	}


	public void turnNextPage(){
		string levelName = "Story2-Page" + currentPage;
		if (GameObject.Find ("MetricManager") != null) {
			MetricManager.Instance.AddToLevelAndTimeMetric (levelName, (Time.time - last_start_time));
		}
		last_start_time = Time.time;

		if (currentPage == 0) {
			sound.PlaySound ("Ev_Story2_Intro_DLG_Stop");
			sound.PlaySound ("Ev_Story2_Posts_DLG_Start");
		}

		currentPage++;

	}

	public void turnLastPage(){
		
		string levelName = "Story2-Page" + currentPage;
		if (GameObject.Find ("MetricManager") != null) {
			MetricManager.Instance.AddToLevelAndTimeMetric (levelName, (Time.time - last_start_time));
		}

		last_start_time = Time.time;

		sound.StopPlaying ();



		if (currentPage == 4 && isStarted[2]) {
			GameObject.Find ("Page5").GetComponent<PandaManager> ().reset (); 
			notebook.destroyPandas ();
			isStarted[2] = false;
		}
		if (currentPage == 3 && isStarted [1]) {
			GameObject.Find ("Page4").GetComponent<BunnyManager> ().leave = true;
			GameObject.Find ("Page4").GetComponent<BunnyManager> ().reset (); 
			isStarted [1] = false;
		}
		if (currentPage == 2 && isStarted [0]) {
			GameObject.Find ("Page3").GetComponent<StarLighter> ().leave = true;
			GameObject.Find ("Page3").GetComponent<StarLighter> ().reset (); 
			isStarted [0] = false;
		}

		sound.PlaySound ("EV_Story2_Opening_Music_Start");



		if (currentPage > 1) {
			if (GameObject.Find ("BluetoothManager") != null) {
				bluetoothManager.Instance.ble.sendBluetooth("1");
			}
			currentPage = 1;
			sound.PlaySound ("Ev_Story2_Posts_DLG_Start");
		} else {
			currentPage--;
			sound.PlaySound ("Ev_Story2_Intro_DLG_Start");
		}

	}

	public void showJobDetail(int jobNum){

		sound.PlaySound ("Ev_Story2_Posts_DLG_Stop");

		string levelName = "Story2-Page" + currentPage;
		if (GameObject.Find ("MetricManager") != null) {
			MetricManager.Instance.AddToLevelAndTimeMetric (levelName, (Time.time - last_start_time));
		}

//		ble.sendBluetooth (jobNum.ToString());
		if (GameObject.Find ("BluetoothManager") != null) {
			bluetoothManager.Instance.ble.sendBluetooth(jobNum.ToString());
		}
		currentPage = jobNum;
		sound.StopPlaying ();

		switch (currentPage) {
		case 2:
			sound.PlaySound ("EV_Story2_Firefly_Music_Start");
			break;
		case 3:
			sound.PlaySound ("EV_Story2_Rabbit_Music_Start");
			break;
		case 4:
			sound.PlaySound ("EV_Story2_Panda_Music_Start");
			break;
		}
		last_start_time = Time.time;
	}

	public void logTime(){
		string levelName = "Story2-Page0";
		if (GameObject.Find ("MetricManager") != null) {
			MetricManager.Instance.AddToLevelAndTimeMetric (levelName, (Time.time - last_start_time));
		}

	}


}
