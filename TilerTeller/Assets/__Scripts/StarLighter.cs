using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class StarLighter : MonoBehaviour {

	class Star{

		public GameObject mystar;
		public ArrayList fireflys;
		public bool isCompleted;

		private int starNum;
		private	Dictionary<string,Sprite> dictSprites = new Dictionary<string,Sprite> ();


		public Star(int num){
			fireflys = new ArrayList(4);
			starNum = num;

			Sprite[] sprites = Resources.LoadAll<Sprite>("S2_P3_atlas1");
			foreach (Sprite sprite in sprites) {
				dictSprites.Add (sprite.name, sprite);
				//				Debug.Log (sprite.name);
			}
		}


		public void lightFirfly(int num){
			//			if (starNum == 0)
			//				return;

			if (num > starNum)
				return;

			for (int i = 1; i < num + 1; i++) {
				if (fireflys [i] != null) {
					SpriteRenderer myfirefly = (SpriteRenderer)fireflys [i];
					myfirefly.DOFade (1, 1).SetDelay (2.2f);
				}
			}
		}

		public void lightStar(int num){

			if (num == starNum) {
				if (!isCompleted) {
					AkSoundEngine.PostEvent ("EV_Story2_Firefly_MoonLightUp", mystar);
				}
				isCompleted = true;

				switch (starNum) {
				case 1:
					mystar.GetComponent<SpriteRenderer> ().sprite = dictSprites ["S2_P3_star1"];
					break;
				case 2:
					mystar.GetComponent<SpriteRenderer> ().sprite = dictSprites ["S2_P3_star2"];
					break;
				case 3:
					mystar.GetComponent<SpriteRenderer> ().sprite = dictSprites ["S2_P3_star3"];
					break;
				case 4:
					mystar.GetComponent<SpriteRenderer> ().sprite = dictSprites ["S2_P3_star4"];
					break;
				}

			}
		}
			

	}

//	public dataManager datamanager;
	public GameObject[] stars;
	private  int[] buttonCounter = { 0, 0, 0, 0 };
	private int[] lastButtonCounter = { 0, 0, 0, 0 };
	private Star[] mystars = new Star[4];
	public Animator Eggy;
	public SoundManager sound;

	private bool levelCompleted = false;


	void Awake(){
		mystars = new Star[4];

		for (int i = 0; i < 4; i++) {

			mystars [i] = new Star (i+1);
			mystars [i].mystar = stars [i];

			Component[] myfireflys = mystars[i].mystar.GetComponentsInChildren (typeof(SpriteRenderer));
			if (myfireflys != null) {
				foreach (SpriteRenderer firefly in myfireflys)
					mystars [i].fireflys.Add (firefly);
			}

		}
	}


	void Start () {


	}


	void Update () {
		if (Input.GetKeyDown (KeyCode.Alpha1)) {
			
			buttonCounter [0]++;
		}
		if (Input.GetKeyDown (KeyCode.Alpha2)) {
			buttonCounter [1]++;
		}
		if (Input.GetKeyDown (KeyCode.Alpha3)) {
			buttonCounter [2]++;
		}
		if (Input.GetKeyDown (KeyCode.Alpha4)) {
			buttonCounter [3]++;
		}
			
//		if (datamanager != null) {
//			buttonCounter = datamanager.getButtonCounter();
//		}

		if (GameObject.Find ("BluetoothManager") != null) {
			buttonCounter = bluetoothManager.Instance.datamanager.getButtonCounter ();
		}


		for (int i = 0; i < 4; i++) {
			if (buttonCounter [i] != lastButtonCounter [i]) {
				Debug.Log ("hello!");
				sound.PlaySound ("EV_Story2_Firefly_Release");
				Eggy.SetTrigger ("release");
				mystars [i].mystar.transform.DOScale (0.85f, 0.2f);
				mystars [i].mystar.transform.DOScale (0.75f, 0.2f).SetDelay (0.2f);
				lastButtonCounter [i] = buttonCounter [i];
				break;
			}
		}


		for (int i = 0; i < 4; i++) {
			if (mystars [i] != null) {
				mystars [i].lightFirfly (buttonCounter [i]);
				mystars [i].lightStar (buttonCounter [i]);
			}
		}


	
		if (mystars [0].isCompleted && mystars [1].isCompleted && mystars [2].isCompleted && mystars [3].isCompleted && !levelCompleted) {
			sound.PlaySound ("EV_Story2_Firefly_LevelComplete");
			Debug.Log ("Level Complete!");
			levelCompleted = true;
		}

//		Debug.Log(buttonCounter[0]+","+buttonCounter[1]+","+buttonCounter[2]+","+buttonCounter[3]);
	}




}
