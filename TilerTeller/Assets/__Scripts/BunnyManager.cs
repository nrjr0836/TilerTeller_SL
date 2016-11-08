using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class BunnyManager : MonoBehaviour {

	public GameObject whiteBunny;
	public GameObject whiteBunnyRed;
	public GameObject brownBunny;
	public float duration;
	public int totalBunnyNum = 25;
	public float timeInterval = 2;

	public bool start = false;
	public bool leave = false;

	public Text[] instructions;
	public Image nextIcon;

	private int count = 0;
	private SoundManager sound;

	public enum State
	{
		InstructionOne = 0,
		InstructionTwo = 1, 
		Start = 2,
		End = 3,
	}


	private State m_state = State.InstructionOne;
	public State state
	{
		get{ return m_state;}
		set{

			if (value == State.InstructionOne) {
				instructions [0].DOFade (1, 1f).SetDelay(1f);
				nextIcon.DOFade (1, 0.5f).SetDelay (5f);
			}
			if (value == State.InstructionTwo) {
				sound.PlaySound ("EV_GUI_ButtonClick");
				nextIcon.DOFade (0, 1f);
				instructions [0].DOFade (0, 1f);
				instructions [1].DOFade (0, 0);
				instructions [1].DOFade (1, 1f).SetDelay (1.5f);
				nextIcon.DOFade (1, 0.5f).SetDelay (5f);
			}
			if (value == State.Start) {
				sound.PlaySound ("EV_GUI_ButtonClick");
				nextIcon.DOFade (0, 1f);
				instructions [1].DOFade (0, 1f);
				instructions [2].DOFade (0, 0);
				instructions [2].DOFade (1, 1f).SetDelay (1.5f);

				GameObject.Find ("S2_P4_light").GetComponent<Animator> ().SetTrigger ("light");

				StartCoroutine (m_Spawn ());
			}
			if (value == State.End) {
				Debug.Log ("end");
			}

			m_state = value;

		}

	}




	void Awake(){
		for (int i = 0; i < 3; i++) {
			instructions [i].DOFade (0, 0);
		}
		sound = GameObject.Find ("SoundManager").GetComponent<SoundManager> ();

	}



	void Update(){
		if (start) {
			state = State.InstructionOne;
			start = false;
		}

		if (Input.GetMouseButtonDown (0)&&(int)m_state<2) {
			state++;
		}
		if (count >= totalBunnyNum) {
			state = State.End;
		}
			
		Debug.Log (count);

	}
	



	IEnumerator m_Spawn(){
		
		while (count<totalBunnyNum) {
			if (timeInterval > 4) {
				timeInterval = timeInterval - 0.1f * count;
			}
			yield return new WaitForSeconds (timeInterval);

			SpawnBunny ( duration - count*0.1f);

			count++;
		}
	}

	void SpawnBunny( float duration ){
		
		int bunnyType = Random.Range(0,6);
	
		if (bunnyType < 1) {
			GameObject brownBunnyClone = (GameObject)Instantiate (brownBunny, brownBunny.transform.position, brownBunny.transform.rotation);
			brownBunnyClone.transform.parent = gameObject.transform;
			brownBunnyClone.GetComponent<BunnyMov> ().duration = duration;
		}
		if (bunnyType >= 1 && bunnyType < 3) {
			GameObject whiteBunnyClone = (GameObject)Instantiate (whiteBunny, whiteBunny.transform.position, whiteBunny.transform.rotation);
			whiteBunnyClone.transform.parent = gameObject.transform;
			whiteBunnyClone.GetComponent<BunnyMov> ().duration = duration;
		}
		if (bunnyType >= 3) {
			GameObject whiteBunnyRedClone = (GameObject)Instantiate (whiteBunnyRed, whiteBunnyRed.transform.position, whiteBunnyRed.transform.rotation);
			whiteBunnyRedClone.transform.parent = gameObject.transform;
			whiteBunnyRedClone.GetComponent<BunnyMov> ().duration = duration;
		}
			
	}

}	
