using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class Star : MonoBehaviour {


//		public GameObject mystar;
//		public ArrayList fireflys;
//		public bool isCompleted;
//
//		private int starNum;
//		private	Dictionary<string,Sprite> dictSprites = new Dictionary<string,Sprite> ();
//
//
//		public Star(int num){
//			fireflys = new ArrayList(4);
//			starNum = num;
//
//			Sprite[] sprites = Resources.LoadAll<Sprite>("S2_P3_atlas1");
//			foreach (Sprite sprite in sprites) {
//				dictSprites.Add (sprite.name, sprite);
//			}
//		}

	public int starNum;
	public bool isCompleted;

	private ArrayList fireflys;
	private	Dictionary<string,Sprite> dictSprites = new Dictionary<string,Sprite> ();
		
	void Awake(){
		fireflys = new ArrayList(4);

		Component[] myfireflys = gameObject.GetComponentsInChildren (typeof(SpriteRenderer));
		if (myfireflys != null) {
			foreach (SpriteRenderer firefly in myfireflys)
				fireflys.Add (firefly);
		}

		Sprite[] sprites = Resources.LoadAll<Sprite>("S2_P3_atlas1");
		foreach (Sprite sprite in sprites) {
			dictSprites.Add (sprite.name, sprite);
		}
		
	}



	public void lightFirfly(int num){

			if (num > starNum)
				return;

			for (int i = 1; i < num + 1; i++) {
				if (fireflys [i] != null) {
					SpriteRenderer myfirefly = (SpriteRenderer)fireflys [i];
				if (starNum == 4) {
					myfirefly.DOFade (1, 1).SetDelay (2.5f);
				} else if (starNum == 1) {
					myfirefly.DOFade (1, 1).SetDelay (2f);
				}else if (starNum == 2) {
					myfirefly.DOFade (1, 1).SetDelay (1.5f);
				}
				else{
				myfirefly.DOFade (1, 1).SetDelay (1f);
				}
			}
		}
	}

	public void lightStar(int num){

			if (num == starNum) {
				if (!isCompleted) {
				AkSoundEngine.PostEvent ("EV_Story2_Firefly_MoonLightUp", gameObject);
				}
				isCompleted = true;

				switch (starNum) {
				case 1:
					gameObject.GetComponent<SpriteRenderer> ().sprite = dictSprites ["S2_P3_star1"];
					break;
				case 2:
					gameObject.GetComponent<SpriteRenderer> ().sprite = dictSprites ["S2_P3_star2"];
					break;
				case 3:
					gameObject.GetComponent<SpriteRenderer> ().sprite = dictSprites ["S2_P3_star3"];
					break;
				case 4:
					gameObject.GetComponent<SpriteRenderer> ().sprite = dictSprites ["S2_P3_star4"];
					break;
				}

			}
	}



}
