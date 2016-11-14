using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class BunnyManager : MonoBehaviour {

	public GameObject whiteBunny;
	public GameObject whiteBunnyRed;
	public GameObject brownBunny;
	public float duration;
	public int totalBunnyNum = 16;
	public float timeInterval = 2;
	public GameObject mylight;

	public bool start = false;
	public bool leave = false;

	public GameObject[] goldCarrot;
	public GameObject[] grayCarrot;
	public GameObject progressBar;

	public Text[] instructions;
	public Image nextIcon;

	private int count = 0;
	private SoundManager sound;
	private GameObject lastBunny;
	private float progressBarLength;
	private float originalTimeInterval;


	public enum State
	{
		InstructionOne = 0,
		InstructionTwo = 2, 
		Start = 3,
		End = 4,
		Reset = 5,
	}


	private State m_state = State.InstructionOne;
	public State state
	{
		get{ return m_state;}
		set{
			if (value == State.InstructionOne) {
				instructions [3].DOFade (0, 0);
				instructions [0].DOFade (1, 1f);
				nextIcon.DOFade (1, 0.5f).SetDelay (0.5f);
			}
			if (value == State.InstructionTwo) {
				sound.PlaySound ("EV_GUI_ButtonClick");
				nextIcon.DOFade (0, 0f);
				nextIcon.DOFade (1, 0.5f).SetDelay(1);
				instructions [0].DOFade (0, 0);
				instructions [1].DOFade (1, 0);
				nextIcon.DOFade (1, 0.5f).SetDelay (0.5f);
			}
			if (value == State.Start) {
				sound.PlaySound ("EV_GUI_ButtonClick");
				nextIcon.DOFade (0, 1f);
				instructions [1].DOFade (0, 1f);
				instructions [2].DOFade (0, 0);
				instructions [2].DOFade (1, 1f);

				mylight.GetComponent<Animator> ().SetTrigger ("light");

				StartCoroutine (m_Spawn ());
			}
			if (value == State.End) {
				if (lastBunny!=null && lastBunny.transform.position.x < -8f) {
					gameObject.GetComponent<Animator> ().SetTrigger ("end");
					mylight.SetActive (false);
					instructions [2].DOFade (0, 0);
					instructions [3].DOFade (0, 0);
					instructions [3].DOFade (1, 1);
				}
			}
			if (value == State.Reset) {
				gameObject.GetComponent<Animator> ().SetTrigger ("restart");
				BunnyMov.correctNum = 0;
				count = 0;
				timeInterval = originalTimeInterval;
				GameObject[] bunnys = GameObject.FindGameObjectsWithTag ("bunny");
				foreach (GameObject bunny in bunnys) {
					Destroy (bunny);
				}
				foreach (GameObject carrot in goldCarrot) {
					carrot.SetActive(false);
				}
				progressBar.transform.localScale = new Vector3(progressBarLength,1,1);
			}

			int textIndex = (int)value;
			if (textIndex < textList.Length) {
				sound.PlaySound ("EV_GUI_ButtonClick");
				instructions[0].text = textList [textIndex];
				nextIcon.DOFade (0, 0f);
				nextIcon.DOFade (1, 0.5f).SetDelay(1);
			}

			m_state = value;

		}

	}

	[SerializeField] string[] textList;


	void Awake(){
		for (int i = 0; i < 4; i++) {
			instructions [i].DOFade (0, 0);
		}
		sound = GameObject.Find ("SoundManager").GetComponent<SoundManager> ();
		progressBarLength = progressBar.transform.localScale.x;
		foreach (GameObject carrot in goldCarrot) {
			carrot.SetActive(false);
		}
		originalTimeInterval = timeInterval;
	}



	void Update(){
		if (start) {
			state = State.InstructionOne;
			start = false;
		}

		if (Input.GetMouseButtonDown (0)&&(int)m_state<3) {
			state++;
		}

		if ((int)state >= (int)State.Start) {
			if (progressBar.transform.localScale.x > 0) {
				progressBar.transform.localScale = new Vector3((1.0f - 1.0f*count / totalBunnyNum)*progressBarLength,1,1);
			}
			if (BunnyMov.correctNum >= 4) {
				goldCarrot [0].SetActive (true);
			}
			if (BunnyMov.correctNum >= 8) {
				goldCarrot [1].SetActive (true);
			}
			if (BunnyMov.correctNum >= 12) {
				goldCarrot [2].SetActive (true);
			}

			Debug.Log (BunnyMov.correctNum);
		}


		if (count >= totalBunnyNum) {
			state = State.End;
		}
			
//		Debug.Log (count);

	}
	



	IEnumerator m_Spawn(){
		
		while (count<totalBunnyNum) {
			if (timeInterval > 2) {
				timeInterval = timeInterval - 0.25f;
			}
			yield return new WaitForSeconds (timeInterval);


			count++;
			SpawnBunny ( duration - count*0.2f);
		}
	}

	void SpawnBunny( float duration ){
		
		int bunnyType = Random.Range(0,6);
	
		if (bunnyType < 1) {
			GameObject brownBunnyClone = (GameObject)Instantiate (brownBunny, brownBunny.transform.position, brownBunny.transform.rotation);
			brownBunnyClone.transform.parent = gameObject.transform;
			brownBunnyClone.GetComponent<BunnyMov> ().duration = duration;
			if (count == totalBunnyNum) {
				lastBunny = brownBunnyClone;
			}
		}
		if (bunnyType >= 1 && bunnyType < 3) {
			GameObject whiteBunnyClone = (GameObject)Instantiate (whiteBunny, whiteBunny.transform.position, whiteBunny.transform.rotation);
			whiteBunnyClone.transform.parent = gameObject.transform;
			whiteBunnyClone.GetComponent<BunnyMov> ().duration = duration;
			if (count == totalBunnyNum) {
				lastBunny = whiteBunnyClone;
			}
		}
		if (bunnyType >= 3) {
			GameObject whiteBunnyRedClone = (GameObject)Instantiate (whiteBunnyRed, whiteBunnyRed.transform.position, whiteBunnyRed.transform.rotation);
			whiteBunnyRedClone.transform.parent = gameObject.transform;
			whiteBunnyRedClone.GetComponent<BunnyMov> ().duration = duration;
			if (count == totalBunnyNum) {
				lastBunny = whiteBunnyRedClone;
			}
		}

	}

	public void reset(){
		state = State.Reset;
	}

}	
