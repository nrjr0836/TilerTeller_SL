using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;

public class BunnyMov : MonoBehaviour {

	public bool isRed = false;
	public Transform endMarker;
	public float duration = 1.0f;
	public Sprite[] lights;

	public static int correctNum = 0;


	private Sequence bunnyMov;
	private	GameObject bunnyOpenEye;
	private GameObject bunnyCry;
	private GameObject bunnyBlackEye;
	private GameObject medicine;
	private GameObject Light;
	private GameObject eggy;

	private bool isClicked = false;

	private float openEyePos;
	private SoundManager sound;

//	private dataManager datamanager;


	void Start () {


		sound = GameObject.Find ("SoundManager").GetComponent<SoundManager>();

		Light = GameObject.Find ("Page4/Light");
		eggy = GameObject.Find ("Page4/S2_P4_eggy");
	
		openEyePos = (float)(1.5 - Random.Range (0, 0.5f));

		if (isRed) {
			bunnyOpenEye = transform.GetChild (2).gameObject;
			bunnyCry = transform.GetChild (1).gameObject;
			bunnyBlackEye = transform.GetChild (0).gameObject;
			medicine = transform.GetChild (4).gameObject;
		}
		else{
			bunnyOpenEye = transform.GetChild (0).gameObject;
			bunnyCry = transform.GetChild (1).gameObject;
			medicine = transform.GetChild (3).gameObject;
		}

		bunnyOpenEye.SetActive (false);
		bunnyCry.SetActive (false);


		if (bunnyBlackEye != null) {
			bunnyBlackEye.SetActive (false);
		}

		medicine.GetComponent<SpriteRenderer> ().DOFade (0, 0);

		transform.DOMoveX (-8.55f, duration * 3).SetEase (Ease.Linear);

//		Debug.Log (openEyePos);
	}


	bool soundPlayed = false;

	void Update () {

//		Debug.Log (transform.position.x);
		if (transform.position.x < openEyePos) {
			bunnyOpenEye.SetActive (true);
		}

		if (transform.position.x < 1.8 && transform.position.x > -2) {

			Vector3 rotateTo = Light.transform.rotation.eulerAngles + new Vector3 (0, 0, 180);
			Light.transform.DORotate (rotateTo, 3f);



			if (GameObject.Find("BluetoothManager")!=null && bluetoothManager.Instance.datamanager.getBunnyIsPressed () == true) {


					isClicked = true;
					if (isRed) {

					StartCoroutine (isRedClicked());
	
					} else {
					StartCoroutine (isWhiteClicked ());
					}
					bluetoothManager.Instance.datamanager.setBunnyIsPressed ();
				}
			 else if (Input.GetMouseButtonDown (0)) {
					
					isClicked = true;

					if (isRed) {
						StartCoroutine (isRedClicked());

					} else {
						StartCoroutine (isWhiteClicked ());
					}
				}

		}
			

		if (transform.position.x < -2.2 && isRed && !isClicked) {
			bunnyCry.SetActive (true);
			if (!soundPlayed) {
				sound.PlaySound ("EV_Story2_Rabbit_Wrong");
				soundPlayed = true;
			}
		}

		if (transform.position.x < -8.35f) {
			if (!bunnyCry.activeSelf) {
				correctNum++;
			}
			Destroy (gameObject);
		}
	}

	IEnumerator LightControl( int lightType){
		
		if (lights[lightType] != null) {
			Light.GetComponent<SpriteRenderer> ().sprite = lights[lightType]; }

		yield return new WaitForSeconds (2);
		Light.GetComponent<SpriteRenderer> ().sprite = lights[0];
	}

	IEnumerator isRedClicked( ){

		if (lights[1] != null) {
			Light.GetComponent<SpriteRenderer> ().sprite = lights[1]; }
		eggy.GetComponent<Animator> ().SetTrigger ("drop");


		medicine.GetComponent<Animator> ().SetTrigger ("start");
		sound.PlaySound ("EV_Story2_Rabbit_Squeeze");

		yield return new WaitForSeconds (1f);
		sound.PlaySound ("EV_Story2_Rabbit_Correct");
		bunnyBlackEye.SetActive (true);

		yield return new WaitForSeconds (1f);
		Light.GetComponent<SpriteRenderer> ().sprite = lights[0];
	}

	IEnumerator isWhiteClicked( ){

		if (lights[2] != null) {
			Light.GetComponent<SpriteRenderer> ().sprite = lights[2]; }
		eggy.GetComponent<Animator> ().SetTrigger ("drop");


		medicine.GetComponent<Animator> ().SetTrigger ("start");
		sound.PlaySound ("EV_Story2_Rabbit_Squeeze");

		yield return new WaitForSeconds (1f);
		sound.PlaySound ("EV_Story2_Rabbit_Wrong");
		bunnyCry.SetActive (true);

		yield return new WaitForSeconds (1f);
		Light.GetComponent<SpriteRenderer> ().sprite = lights[0];
	}

}
