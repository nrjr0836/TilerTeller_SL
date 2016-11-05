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


	private bool isCreated = false;
	private bool isBunnySpawn = false;


	void Start () {

		lastBtn = GameObject.Find ("/Canvas/last");
		nextBtn = GameObject.Find ("/Canvas/next");
		homeBtn_1 = GameObject.Find ("/Canvas/home1");

		bookLength = pages.Length;
		currentPage = 0;


	}



	void Update () {


		lastBtn.SetActive (true);
		nextBtn.SetActive (true);
		homeBtn_1.SetActive (false);

		for (int i = 0; i < bookLength; i++) {
			if (i == currentPage && pages [i] != null) {
				pages [i].SetActive (true);
				if (pages [i].GetComponent<AudioSource> () != null && pages [i].GetComponent<AudioSource> ().isPlaying != true) {
					pages [i].GetComponent<AudioSource> ().Play ();
				}
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

		if (currentPage == 3 && !isBunnySpawn ) {
			GameObject.Find ("Page4").GetComponent<BunnyManager> ().start = true;
			isBunnySpawn = true;
		}

		if (currentPage == 4 && !isCreated) {
			notebook.GetComponent<CanvasGroup> ().DOFade (1, 0);
			notebook.createPandas ();
			isCreated = true;
		}


	}


	public void turnNextPage(){
		gameObject.GetComponent<AudioSource> ().Play ();

		currentPage++;

	}

	public void turnLastPage(){
		sound.StopPlaying ();
		if (currentPage == 4 && isCreated) {
			notebook.destroyPandas ();
			isCreated = false;
		}
		gameObject.GetComponent<AudioSource> ().Play ();
		if (currentPage > 1) {
			if (GameObject.Find ("BluetoothManager") != null) {
				bluetoothManager.Instance.ble.sendBluetooth("1");
			}
			currentPage = 1;
		} else {
			currentPage--;
		}

	}

	public void showJobDetail(int jobNum){
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
			sound.PlaySound ("");
			break;
		}
	}
}
